using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace vinv.Pages.Categories;

public class CategoryEditRequest
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}

public class EditModel(AppDbContext context) : PageModel
{
    [BindProperty]
    public CategoryEditRequest CategoryEditRequest { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category =  await context.Categories.FirstOrDefaultAsync(m => m.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        CategoryEditRequest = new CategoryEditRequest
        {
            Id = category.Id,
            Name = category.Name
        };

        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            context.Categories.Update(new()
            {
                Id = CategoryEditRequest.Id,
                Name = CategoryEditRequest.Name
            });
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryExists(CategoryEditRequest.Id))
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

    private bool CategoryExists(int id)
    {
        return context.Categories.Any(e => e.Id == id);
    }
}