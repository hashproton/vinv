using Application.Errors;
using Application.Repositories;
using Application.Shared;
using FluentValidation;
using MediatR;

namespace Application.Features.Queries.GetCategoryById
{
    public static class GetCategoryById
    {
        public class Query : IRequest<Result<CategoryDto>>
        {
            public int Id { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(c => c.Id)
                    .GreaterThan(0);
            }
        }

        public class Handler(ICategoriesRepository categoriesRepository) : IRequestHandler<Query, Result<CategoryDto>>
        {
            public async Task<Result<CategoryDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var category = await categoriesRepository.GetByIdAsync(request.Id, cancellationToken);
                if (category == null)
                {
                    return Result.Failure<CategoryDto>(CategoryErrors.NotFoundById(request.Id));
                }

                var categoryDto = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                };

                return Result.Success(categoryDto);
            }
        }

        public class CategoryDto
        {
            public required int Id { get; init; }

            public required string Name { get; init; }
        }
    }
}