namespace Application.Features.Commands.DeleteTenant;

public static class DeleteTenant
{
    public class Command : IRequest<Result>
    {
        public required int Id { get; init; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0);
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

            await tenantsRepository.DeleteAsync(tenant, cancellationToken);
                
            return Result.Success();
        }
    }
}