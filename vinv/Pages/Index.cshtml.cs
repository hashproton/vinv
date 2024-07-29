using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using vinv.Entities;

namespace vinv.Pages
{
    public class IndexModel(ILogger<IndexModel> logger, AppDbContext context) : PageModel
    {
        [BindProperty]
        public List<Product> Products { get; set; }

        public async Task OnGet()
        {
            Products = await context.Products.ToListAsync();

            logger.LogInformation("Products fetched successfully");
        }
    }
}
