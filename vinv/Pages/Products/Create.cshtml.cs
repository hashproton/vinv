using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using vinv;
using vinv.Entities;

namespace vinv.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly vinv.AppDbContext _context;

        public CreateModel(vinv.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public SelectList CategoryOptions { get; set; }

        public IActionResult OnGet()
        {
            PopulateCategoryOptions();
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateCategoryOptions();
                return Page();
            }

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        private void PopulateCategoryOptions()
        {
            CategoryOptions = new SelectList(_context.Categories.ToList(), "Id", "Name");
        }
    }
}