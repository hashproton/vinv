using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace vinv.Pages.ProductStocks;

public class CreateProductStockRequest
{
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Stock must be a non-negative number.")]
    public decimal Stock { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Minimal stock must be a non-negative number.")]
    public decimal MinimalStock { get; set; }

    [Required]
    [Display(Name = "Product")]
    public int ProductId { get; set; }
}

public class CreateModel(AppDbContext context, ILogger<CreateModel> logger) : PageModel
{
    [BindProperty]
    public CreateProductStockRequest CreateRequest { get; set; } = default!;

    public SelectList ProductOptions { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await PopulateProductOptionsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateProductOptionsAsync();
            return Page();
        }

        try
        {
            var productStock = new Entities.ProductStock
            {
                Stock = CreateRequest.Stock,
                MinimalStock = CreateRequest.MinimalStock,
                ProductId = CreateRequest.ProductId
            };

            context.ProductStocks.Add(productStock);
            await context.SaveChangesAsync();

            logger.LogInformation("Created new product stock for ProductId {ProductId}", productStock.ProductId);

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while creating a new product stock");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the product stock. Please try again.");
            await PopulateProductOptionsAsync();
            return Page();
        }
    }

    private async Task PopulateProductOptionsAsync()
    {
        var products = await context.Products.ToListAsync();
        ProductOptions = new SelectList(products, "Id", "Name");
    }
}