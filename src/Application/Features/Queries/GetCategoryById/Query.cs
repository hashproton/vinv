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
                RuleFor(x => x.Id).GreaterThan(0);
            }
        }

        public class Handler(ICategoriesRepository categoriesRepository) : IRequestHandler<Query, Result<CategoryDto>>
        {
            public async Task<Result<CategoryDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var category = await categoriesRepository.GetByIdAsync(request.Id, cancellationToken);
                if (category == null)
                {
                    return Result<CategoryDto>.Failure(CategoryErrors.NotFoundById(request.Id));
                }

                var categoryDto = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                };

                return Result<CategoryDto>.Success(categoryDto);
            }
        }
        public class CategoryDto
        {
            public int Id { get; set; }

            public string Name { get; set; } = null!;
        }
    }
}