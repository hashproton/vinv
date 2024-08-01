using Application.Errors;
using Application.Repositories;
using Application.Shared;
using Domain;
using FluentValidation;
using MediatR;

namespace Application.Features.Commands.CreateCategory
{
    public static class CreateCategory
    {
        public class Command : IRequest<Result<int>>
        {
            public string Name { get; set; } = null!;
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                     .NotEmpty().WithMessage("Name is required.")
                     .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");
            }
        }

        public class Handler(ICategoriesRepository categoriesRepository) : IRequestHandler<Command, Result<int>>
        {
            public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
            {
                var existingCategory = await categoriesRepository.GetByNameAsync(request.Name, cancellationToken);
                if (existingCategory != null)
                {
                    return Result<int>.Failure(CategoryErrors.AlreadyExists);
                }

                var category = new Category { Name = request.Name };

                await categoriesRepository.CreateAsync(category, cancellationToken);

                return Result<int>.Success(category.Id);
            }
        }
    }
}
