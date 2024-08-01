using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace vinv.Pages.ProductStocks;

public class EditProductStockRequest
{
    public int Id { get; set; }

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
public class EditModel(AppDbContext context, ILogger<EditModel> logger) : PageModel
{
    [BindProperty]
    public EditProductStockRequest EditRequest { get; set; } = default!;

    public SelectList ProductOptions { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productStock = await context.ProductStocks
            .Include(ps => ps.Product)
            .FirstOrDefaultAsync(ps => ps.Id == id);

        if (productStock == null)
        {
            return NotFound();
        }

        EditRequest = new EditProductStockRequest
        {
            Id = productStock.Id,
            Stock = productStock.Stock,
            MinimalStock = productStock.MinimalStock,
            ProductId = productStock.ProductId
        };

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

        var productStockToUpdate = await context.ProductStocks.FindAsync(EditRequest.Id);

        if (productStockToUpdate == null)
        {
            return NotFound();
        }

        try
        {
            productStockToUpdate.Stock = EditRequest.Stock;
            productStockToUpdate.MinimalStock = EditRequest.MinimalStock;
            productStockToUpdate.ProductId = EditRequest.ProductId;

            await context.SaveChangesAsync();

            logger.LogInformation("Updated product stock for ProductId {ProductId}", productStockToUpdate.ProductId);

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating the product stock");
            ModelState.AddModelError(string.Empty, "An error occurred while updating the product stock. Please try again.");
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