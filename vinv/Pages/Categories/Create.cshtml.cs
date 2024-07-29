using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using vinv;
using vinv.Entities;

namespace vinv.Pages.Categories;

public class CategoryRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}
public class CreateModel(AppDbContext context) : PageModel
{
    private readonly AppDbContext _context = context;

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public CategoryRequest CategoryRequest { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Categories.Add(new()
        {
            Name = CategoryRequest.Name
        });
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}