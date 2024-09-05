using Application.Repositories;
using Domain;
using Infra.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class CategoriesRepository(AppDbContext context) : GenericRepository<Category>(context), ICategoriesRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }
    
    public override async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}