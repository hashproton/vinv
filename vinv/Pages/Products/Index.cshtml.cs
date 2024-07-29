﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using vinv;
using vinv.Entities;

namespace vinv.Pages.Products
{
    public class IndexModel(AppDbContext context) : PageModel
    {
        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Product = await context.Products.ToListAsync();
        }
    }
}
