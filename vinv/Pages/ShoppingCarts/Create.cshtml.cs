using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using vinv.Entities;

namespace vinv.Pages.ShoppingCarts
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

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
            var lowStockProducts = await _context.ProductStocks
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
                var dbProduct = await _context.ProductStocks.FindAsync(product.Id);
                if (dbProduct != null)
                {
                    dbProduct.Stock = product.Stock;
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}