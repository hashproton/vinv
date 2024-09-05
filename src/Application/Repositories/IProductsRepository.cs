using Application.Repositories.Shared;

namespace Application.Repositories;

public interface IProductsRepository : IGenericRepository<Product>
{
    Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken);
}