using Application.Errors;
using Application.Repositories;
using Application.Shared;
using FluentValidation;
using MediatR;

namespace Application.Features.Commands.UpdateCategory
{
    public static class UpdateCategory
    {
        public class Command : IRequest<Result>
        {
            public int Id { get; set; }

            public string Name { get; set; } = null!;
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required.")
                    .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");
            }
        }

        public class Handler(ICategoriesRepository categoriesRepository) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var category = await categoriesRepository.GetByIdAsync(request.Id, cancellationToken);
                if (category == null)
                {
                    return Result.Failure(CategoryErrors.NotFoundById(request.Id));
                }

                var existingCategoryWithName = await categoriesRepository.GetByNameAsync(request.Name, cancellationToken);
                if (existingCategoryWithName != null && existingCategoryWithName.Id != request.Id)
                {
                    return Result.Failure(CategoryErrors.AlreadyExists);
                }

                category.Name = request.Name;
                await categoriesRepository.UpdateAsync(category, cancellationToken);
                return Result.Success();
            }
        }
    }
}