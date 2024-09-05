using Application.Features.Queries.Shared;
using Application.Repositories.Shared;

namespace Application.Features.Queries.GetTenantsPaged;

public static class GetTenantsPaged
{
    public class Query : BaseQuery, IRequest<Result<PagedResult<TenantDto>>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(q => q.PagedQuery.Skip)
                .GreaterThanOrEqualTo(0);

            RuleFor(q => q.PagedQuery.Take)
                .GreaterThanOrEqualTo(1);
        }
    }

    public class Handler(ITenantsRepository tenantsRepository) 
        : IRequestHandler<Query, Result<PagedResult<TenantDto>>>
    {
        public async Task<Result<PagedResult<TenantDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var pagedResult = await tenantsRepository.GetPagedAsync(
                request.PagedQuery,
                cancellationToken);
            
            return Result.Success(new PagedResult<TenantDto>
            {
                Skip = pagedResult.Skip,
                Take = pagedResult.Take,
                Total = pagedResult.Total,
                Items = pagedResult.Items.Select(t => new TenantDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
            });
        }
    }

    public class TenantDto
    {
        public required int Id { get; init; }

        public required string Name { get; init; }
    }
}