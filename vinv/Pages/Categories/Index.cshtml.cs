﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using vinv.Entities;

namespace vinv.Pages.Categories;

public class IndexModel(vinv.AppDbContext context) : PageModel
{
    private readonly vinv.AppDbContext _context = context;

    public IList<Category> Category { get;set; } = default!;

    public async Task OnGetAsync()
    {
        Category = await _context.Categories.ToListAsync();
    }
}