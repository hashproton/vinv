using Domain.Shared;

namespace Application.Repositories.Shared;

public class PagedResult<TEntity>
{
    public required int Skip { get; init; }
    public required int Total { get; init; }
    public required int Take { get; init; }
    public required IEnumerable<TEntity> Items { get; init; }
}

public class PagedQuery
{
    public int Skip { get; init; }

    public int Take { get; init; }
    
    public string? Filter { get; init; }
}


public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken);

    Task CreateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);

    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<PagedResult<TEntity>> GetPagedAsync(
        PagedQuery pagedQuery,
        CancellationToken cancellationToken);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
}