using Application;
using FluentValidation;
using Infra.Extensions.DependencyInjection;
using Infra.Pipelines;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(ApplicationReference.Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(ApplicationReference.Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }); 
            
        services.AddDbContext(configuration);
        services.AddRepositories();

        return services;
    }
}