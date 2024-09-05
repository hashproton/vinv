namespace Application.Features.Commands.UpdateCategory;

public static class UpdateCategory
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
            if (existingCategoryWithName != null)
            {
                return Result.Failure(CategoryErrors.AlreadyExists(request.Name));
            }

            category.Name = request.Name;
            await categoriesRepository.UpdateAsync(category, cancellationToken);

            return Result.Success();
        }
    }
}