using Application.Repositories;
using Domain;
using Infra.Repositories.Shared;

namespace Infra.Repositories
{
    public class ProductsRepository(AppDbContext context) : GenericRepository<Product>(context), IProductsRepository
    {
    }
}
