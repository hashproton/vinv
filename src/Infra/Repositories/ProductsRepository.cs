using Application.Repositories;
using Domain;
using Infra.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class ProductsRepository(AppDbContext context) : GenericRepository<Product>(context), IProductsRepository
{
    private readonly AppDbContext context = context;

    public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await context.Products.FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
    }
}