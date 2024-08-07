using Application.Repositories.Shared;
using Domain;

namespace Application.Repositories;

public interface IProductsRepository : IGenericRepository<Product>
{
    Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken);
}