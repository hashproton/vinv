using Application.Repositories;
using Application.Repositories.Shared;
using Infra.Repositories;
using Infra.Repositories.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Extensions.DependencyInjection
{
    public static class RepositoriesDependencyInjection
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        }
    }
}
