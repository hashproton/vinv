using Application.Repositories.Shared;

namespace Application.Repositories;

public interface ITenantsRepository : IGenericRepository<Tenant>
{
    Task<Tenant?> GetByNameAsync(string name, CancellationToken cancellationToken);
}