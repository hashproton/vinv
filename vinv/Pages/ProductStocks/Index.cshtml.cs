using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace vinv.Pages.ProductStocks;

public class ProductStockListViewModel
{
    public IList<Entities.ProductStock> ProductStocks { get; set; }
    public string ProductNameFilter { get; set; }
    public string StockStatusFilter { get; set; }
}

public class IndexModel(AppDbContext context, ILogger<IndexModel> logger) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public ProductStockListViewModel ViewModel { get; set; } = new();

    public async Task OnGetAsync()
    {
        var query = context.ProductStocks
            .Include(ps => ps.Product)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(ViewModel.ProductNameFilter))
        {
            query = query.Where(ps => ps.Product.Name.ToLower().Contains(ViewModel.ProductNameFilter.ToLower().Trim()));
        }

        if (!string.IsNullOrWhiteSpace(ViewModel.StockStatusFilter))
        {
            if (ViewModel.StockStatusFilter == "Available")
            {
                query = query.Where(ps => ps.Stock > ps.MinimalStock);
            }
            else if (ViewModel.StockStatusFilter == "LowStock")
            {
                query = query.Where(ps => ps.Stock <= ps.MinimalStock);
            }
        }

        ViewModel.ProductStocks = await query.ToListAsync();
    }
}