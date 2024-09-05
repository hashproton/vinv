using Application.Repositories;
using Domain;
using Infra.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class TenantsRepository(AppDbContext context) : GenericRepository<Tenant>(context), ITenantsRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Tenant?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.Tenants.FirstOrDefaultAsync(t => t.Name == name, cancellationToken);
    }
    
    public override async Task<Tenant?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}