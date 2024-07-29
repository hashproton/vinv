using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace vinv.Pages.ProductStocks;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(AppDbContext context, ILogger<DeleteModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public Entities.ProductStock ProductStock { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        ProductStock = await _context.ProductStocks
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

        ProductStock = await _context.ProductStocks.FindAsync(id);

        if (ProductStock != null)
        {
            _context.ProductStocks.Remove(ProductStock);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted product stock for ProductId {ProductId}", ProductStock.ProductId);
        }

        return RedirectToPage("./Index");
    }
}