using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using vinv.Entities;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

namespace vinv.Pages;

public class IndexModel(AppDbContext context, IConfiguration configuration) : PageModel
{
    public List<ProductStock> ProductStocks { get; set; }

    public async Task OnGetAsync()
    {
        ProductStocks = await context.ProductStocks
            .Where(ps => ps.Stock < ps.MinimalStock)
            .Include(ps => ps.Product)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostCreateShoppingListAsync()
    {
        var lowStockProducts = await context.ProductStocks
            .Where(ps => ps.Stock < ps.MinimalStock)
            .ToListAsync();

        if (lowStockProducts.Any())
        {
            var shoppingCart = new ShoppingCart
            {
                ProductStocks = lowStockProducts
            };

            context.ShoppingCarts.Add(shoppingCart);
            await context.SaveChangesAsync();

            return RedirectToPage("/ShoppingCart", new { id = shoppingCart.Id });
        }

        return RedirectToPage("/Index");
    }


    public async Task<IActionResult> OnPostSendEmailAsync(string emailAddress)
    {
        try
        {
            var smtpServer = configuration["SmtpSettings:Server"];
            var smtpPort = int.Parse(configuration["SmtpSettings:Port"]);
            var smtpUsername = configuration["SmtpSettings:Username"];
            var smtpPassword = configuration["SmtpSettings:Password"];

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                var message = new MailMessage
                {
                    From = new MailAddress(smtpUsername),
                    Subject = "Shopping List",
                    Body = await GenerateShoppingListEmail(),
                    IsBodyHtml = true,
                };
                message.To.Add(emailAddress);

                await client.SendMailAsync(message);
            }

            TempData["SuccessMessage"] = "Email sent successfully!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Failed to send email. Please try again later.";
        }

        return RedirectToPage();
    }

    private async Task<string> GenerateShoppingListEmail()
    {
        var products = await context.ProductStocks
            .Where(ps => ps.Stock < ps.MinimalStock)
            .Include(ps => ps.Product)
            .ToListAsync();

        var emailBody = @"
        <h2>Shopping List</h2>
        <table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse;'>
            <tr style='background-color: #f2f2f2;'>
                <th>Product Name</th>
                <th>Current Stock</th>
                <th>Minimal Stock</th>
                <th>Quantity to Buy</th>
            </tr>";

        foreach (var product in products)
        {
            var quantityToBuy = product.MinimalStock - product.Stock;
            emailBody += $@"
            <tr>
                <td>{product.Product.Name}</td>
                <td>{product.Stock}</td>
                <td>{product.MinimalStock}</td>
                <td>{quantityToBuy}</td>
            </tr>";
        }

        emailBody += "</table>";

        var hostname = HttpContext.Request.Host.Value;
        emailBody += "<p>You can check the shopping list <a href='https://" + hostname + "/ShoppingCarts/Create'>here</a></p>";

        return emailBody;
    }

    public async Task<IActionResult> OnPostSendSMSAsync(string phoneNumber)
    {
        try
        {
            var accountSid = configuration["Twilio:AccountSid"];
            var authToken = configuration["Twilio:AuthToken"];
            var fromNumber = configuration["Twilio:FromNumber"];

            TwilioClient.Init(accountSid, authToken);

            var messageBody = await GenerateShoppingListSMS();

            var message = await MessageResource.CreateAsync(
                body: messageBody,
                from: new PhoneNumber(fromNumber),
                to: new PhoneNumber("+351" + phoneNumber)
            );

            TempData["SuccessMessage"] = "SMS sent successfully!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Failed to send SMS. Please try again later.";
        }

        return RedirectToPage();
    }

    private async Task<string> GenerateShoppingListSMS()
    {
        var products = await context.ProductStocks
            .Where(ps => ps.Stock < ps.MinimalStock)
            .Include(ps => ps.Product)
            .ToListAsync();

        var smsBody = "Shopping List:\n";
        foreach (var product in products)
        {
            var quantityToBuy = product.MinimalStock - product.Stock;
            smsBody += $"{product.Product.Name}: Buy {quantityToBuy}\n";
        }

        return smsBody;
    }
}