using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace vinv.Pages.ShoppingCarts
{
    public class CreateModel(AppDbContext context) : PageModel
    {
        public List<ProductStockViewModel> LowStockProducts { get; set; } = new List<ProductStockViewModel>();

        public class ProductStockViewModel
        {
            public int Id { get; set; }
            public string ProductName { get; set; }
            [Required]
            public decimal Stock { get; set; }
            public decimal MinimalStock { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var lowStockProducts = await context.ProductStocks
                .Where(ps => ps.Stock < ps.MinimalStock)
                .Include(ps => ps.Product)
                .ToListAsync();

            LowStockProducts = lowStockProducts.Select(ps => new ProductStockViewModel
            {
                Id = ps.Id,
                ProductName = ps.Product.Name,
                Stock = ps.Stock,
                MinimalStock = ps.MinimalStock
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromForm] List<ProductStockViewModel> LowStockProducts)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            foreach (var product in LowStockProducts)
            {
                var dbProduct = await context.ProductStocks.FindAsync(product.Id);
                if (dbProduct != null)
                {
                    dbProduct.Stock = product.Stock;
                }
            }

            await context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}