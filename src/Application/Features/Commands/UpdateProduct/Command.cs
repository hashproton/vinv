namespace Application.Features.Commands.UpdateProduct;

public static class UpdateProduct
{
    public class Command : IRequest<Result>
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required int CategoryId { get; init; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0);

            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(c => c.CategoryId)
                .GreaterThan(0);
        }
    }

    public class Handler(
        IProductsRepository productsRepository,
        ICategoriesRepository categoriesRepository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await productsRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
            {
                return Result.Failure(ProductErrors.NotFoundById(request.Id));
            }

            if (await categoriesRepository.GetByIdAsync(request.CategoryId, cancellationToken) == null)
            {
                return Result.Failure(CategoryErrors.NotFoundById(request.CategoryId));
            }

            product.Name = request.Name;
            product.CategoryId = request.CategoryId;
            await productsRepository.UpdateAsync(product, cancellationToken);

            return Result.Success();
        }
    }
}