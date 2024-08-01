using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using vinv.Entities;

namespace vinv.Pages.Products;

public class EditProductRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Name field is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The Category field is required.")]
    public int CategoryId { get; set; }
}

public class EditModel(AppDbContext context, ILogger<EditModel> logger) : PageModel
{
    [BindProperty]
    public EditProductRequest EditRequest { get; set; } = default!;

    public Product Product { get; set; } = default!;

    public SelectList CategorySelectList { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (product == null)
        {
            return NotFound();
        }
        
        Product = product;
        EditRequest = new EditProductRequest
        {
            Id = product.Id,
            Name = product.Name,
            CategoryId = product.CategoryId
        };
        
        CategorySelectList = new SelectList(await context.Categories.ToListAsync(), "Id", "Name", Product.CategoryId);
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            foreach (var modelState in ModelState.Values) 
            {
                foreach (var error in modelState.Errors) 
                {
                    logger.LogError($"Model State Error: {error.ErrorMessage}");
                }
            }

            CategorySelectList = new SelectList(await context.Categories.ToListAsync(), "Id", "Name", EditRequest.CategoryId);
            return Page();
        }

        var productToUpdate = await context.Products.FindAsync(EditRequest.Id);

        if (productToUpdate == null)
        {
            return NotFound();
        }

        productToUpdate.Name = EditRequest.Name;
        productToUpdate.CategoryId = EditRequest.CategoryId;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(EditRequest.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool ProductExists(int id)
    {
        return context.Products.Any(e => e.Id == id);
    }
}