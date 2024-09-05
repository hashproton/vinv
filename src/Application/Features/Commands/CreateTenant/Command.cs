using Application.Errors;
using Application.Repositories;
using Application.Shared;
using Domain;
using FluentValidation;
using MediatR;

namespace Application.Features.Commands.CreateTenant;

public static class CreateTenant
{
    public class Command : IRequest<Result<Response>>
    {
        public required string Name { get; init; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(200);
        }
    }

    public class Handler(
        ITenantsRepository tenantsRepository) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await tenantsRepository.GetByNameAsync(request.Name, cancellationToken) != null)
            {
                return Result.Failure<Response>(TenantErrors.AlreadyExists(request.Name));
            }

            var tenant = new Tenant { Name = request.Name };

            await tenantsRepository.CreateAsync(tenant, cancellationToken);

            return Result.Success(new Response(tenant.Id));
        }
    }
    
    public record Response(int Id);
}