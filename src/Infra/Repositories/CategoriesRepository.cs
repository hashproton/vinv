using Application.Repositories;
using Domain;
using Infra.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class CategoriesRepository(AppDbContext context) : GenericRepository<Category>(context), ICategoriesRepository
{
    private readonly AppDbContext context = context;

    public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await context.Categories.FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
    }
}