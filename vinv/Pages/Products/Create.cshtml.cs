using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using vinv.Entities;
using System.ComponentModel.DataAnnotations;

namespace vinv.Pages.Products;

public class CreateProductRequest
{
    [Required(ErrorMessage = "The Name field is required.")]
    [StringLength(100, ErrorMessage = "The Name field must be at most 100 characters long.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The Category field is required.")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }
}

public class CreateModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(AppDbContext context, ILogger<CreateModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public CreateProductRequest CreateProductRequest { get; set; } = default!;

    public SelectList CategoryOptions { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await PopulateCategoryOptionsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await PopulateCategoryOptionsAsync();
            return Page();
        }

        try
        {
            var product = new Product
            {
                Name = CreateProductRequest.Name,
                CategoryId = CreateProductRequest.CategoryId,
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Created new product with ID {ProductId}", product.Id);
            
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new product");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the product. Please try again.");
            await PopulateCategoryOptionsAsync();
            return Page();
        }
    }

    private async Task PopulateCategoryOptionsAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        CategoryOptions = new SelectList(categories, "Id", "Name");
    }
}