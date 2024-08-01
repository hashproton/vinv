using Application.Repositories.Shared;
using Domain;

namespace Application.Repositories
{
    public interface ICategoriesRepository : IGenericRepository<Category>
    {
        Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken);
    }
}
