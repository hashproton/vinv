using Application.Repositories.Shared;

namespace Application.Repositories;

public interface ICategoriesRepository : IGenericRepository<Category>
{
    Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken);
}