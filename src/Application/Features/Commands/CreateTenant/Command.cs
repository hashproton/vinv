namespace Application.Features.Commands.CreateTenant;

public static class CreateTenant
{
    public class Command : IRequest<Result<Response>>
    {
        public required string Name { get; init; }
        
        public TenantStatus Status { get; init; } = TenantStatus.Inactive;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(200);
            
            RuleFor(c => c.Status)
                .IsInEnum();
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

            var tenant = new Tenant
            {
                Name = request.Name,
                Status = request.Status
            };

            await tenantsRepository.CreateAsync(tenant, cancellationToken);

            return Result.Success(new Response(tenant.Id));
        }
    }
    
    public record Response(int Id);
}