
using Domain.Shared;

namespace Application.Repositories.Shared;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken);
        
    Task CreateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
}