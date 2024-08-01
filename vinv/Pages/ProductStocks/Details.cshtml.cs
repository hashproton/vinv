using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace vinv.Pages.ProductStocks;

public class DetailsModel(AppDbContext context) : PageModel
{
    public Entities.ProductStock ProductStock { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productStock = await context.ProductStocks
            .Include(ps => ps.Product)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (productStock == null)
        {
            return NotFound();
        }

        ProductStock = productStock;
        return Page();
    }
}