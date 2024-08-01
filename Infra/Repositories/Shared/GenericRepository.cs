using Application.Repositories.Shared;
using Domain.Shared;

namespace Infra.Repositories.Shared
{
    public class GenericRepository<TEntity>(
        AppDbContext context) : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await context.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            context.Remove(entity);

            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken) => await context.FindAsync<TEntity>(id, cancellationToken);

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            context.Update(entity);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
