using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; init; }

    public DbSet<Category> Categories { get; init; }
}