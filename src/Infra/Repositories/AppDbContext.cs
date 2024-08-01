using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }
}