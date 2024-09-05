using Application.Repositories.Shared;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using QueryKit;

namespace Infra.Repositories.Shared;

public class GenericRepository<TEntity>(
    AppDbContext context) : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await context.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
        
    public async Task CreateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await context.AddRangeAsync(entities, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        context.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken) => 
        await context.FindAsync<TEntity>([id], cancellationToken);
    
    public async Task<PagedResult<TEntity>> GetPagedAsync(PagedQuery pagedQuery, CancellationToken cancellationToken)
    {
        var query = context.Set<TEntity>().AsNoTracking();

        if (pagedQuery.Filter is not null)
        {
            query = query.ApplyQueryKitFilter(pagedQuery.Filter);
        }
        
        var totalCount = await query
            .CountAsync(cancellationToken);
        
        var items = await query
            .Skip(pagedQuery.Skip)
            .Take(pagedQuery.Take)
            .ToListAsync(cancellationToken);
        
        return new PagedResult<TEntity>
        {
            Skip = pagedQuery.Skip,
            Take = pagedQuery.Take,
            Total = totalCount,
            Items = items
        };
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        context.Update(entity);

        await context.SaveChangesAsync(cancellationToken);
    }
}