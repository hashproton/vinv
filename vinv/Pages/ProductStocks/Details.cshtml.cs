using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace vinv.Pages.ProductStocks;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _context;

    public DetailsModel(AppDbContext context)
    {
        _context = context;
    }

    public Entities.ProductStock ProductStock { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var productStock = await _context.ProductStocks
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