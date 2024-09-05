using Application.Repositories.Shared;
using Domain;

namespace Application.Repositories;

public interface ITenantsRepository : IGenericRepository<Tenant>
{
    Task<Tenant?> GetByNameAsync(string name, CancellationToken cancellationToken);
}