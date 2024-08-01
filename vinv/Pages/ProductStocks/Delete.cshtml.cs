using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace vinv.Pages.ProductStocks;

public class DeleteModel(AppDbContext context, ILogger<DeleteModel> logger) : PageModel
{
    [BindProperty]
    public Entities.ProductStock ProductStock { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        ProductStock = await context.ProductStocks
            .Include(ps => ps.Product)
            .FirstOrDefaultAsync(ps => ps.Id == id);

        if (ProductStock == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        ProductStock = await context.ProductStocks.FindAsync(id);

        if (ProductStock != null)
        {
            context.ProductStocks.Remove(ProductStock);
            await context.SaveChangesAsync();
            logger.LogInformation("Deleted product stock for ProductId {ProductId}", ProductStock.ProductId);
        }

        return RedirectToPage("./Index");
    }
}