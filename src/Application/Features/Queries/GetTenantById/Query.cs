using Application.Errors;
using Application.Repositories;
using Application.Shared;
using FluentValidation;
using MediatR;

namespace Application.Features.Queries.GetTenantById;

public static class GetTenantById
{
    public class Query : IRequest<Result<TenantDto>>
    {
        public int Id { get; init; }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0);
        }
    }

    public class Handler(ICategoriesRepository categoriesRepository) : IRequestHandler<Query, Result<TenantDto>>
    {
        public async Task<Result<TenantDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var tenant = await categoriesRepository.GetByIdAsync(request.Id, cancellationToken);
            if (tenant == null)
            {
                return Result.Failure<TenantDto>(TenantErrors.NotFoundById(request.Id));
            }

            var categoryDto = new TenantDto
            {
                Id = tenant.Id,
                Name = tenant.Name
            };

            return Result.Success(categoryDto);
        }
    }

    public class TenantDto
    {
        public required int Id { get; init; }

        public required string Name { get; init; }
    }
}