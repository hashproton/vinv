using Application.Errors;
using Application.Repositories;
using Application.Shared;
using FluentValidation;
using MediatR;

namespace Application.Features.Commands.UpdateTenant;

public static class UpdateTenant
{
    public class Command : IRequest<Result>
    {
        public required int Id { get; init; }

        public required string Name { get; init; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Id).GreaterThan(0);

            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(200);
        }
    }

    public class Handler(ITenantsRepository tenantsRepository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var tenant = await tenantsRepository.GetByIdAsync(request.Id, cancellationToken);
            if (tenant == null)
            {
                return Result.Failure(TenantErrors.NotFoundById(request.Id));
            }

            var existingTenantWithName = await tenantsRepository.GetByNameAsync(request.Name, cancellationToken);
            if (existingTenantWithName != null)
            {
                return Result.Failure(TenantErrors.AlreadyExists(request.Name));
            }

            tenant.Name = request.Name;
            await tenantsRepository.UpdateAsync(tenant, cancellationToken);

            return Result.Success();
        }
    }
}