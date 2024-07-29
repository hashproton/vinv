﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using vinv.Entities;

namespace vinv.Pages.Products;

public class DeleteModel(AppDbContext context) : PageModel
{
    private readonly AppDbContext _context = context;

    [BindProperty]
    public Product Product { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        Product = product;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            Product = product;
            _context.Products.Remove(Product);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}