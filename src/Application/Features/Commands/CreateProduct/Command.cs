namespace Application.Features.Commands.CreateProduct;

public static class CreateProduct
{
    public class Command : IRequest<Result<int>>
    {
        public required string Name { get; init; }
        public required int CategoryId { get; init; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(c => c.CategoryId)
                .GreaterThan(0);
        }
    }

    public class Handler(
        IProductsRepository productsRepository,
        ICategoriesRepository categoriesRepository) : IRequestHandler<Command, Result<int>>
    {
        public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await productsRepository.GetByNameAsync(request.Name, cancellationToken) != null)
            {
                return Result.Failure<int>(ProductErrors.AlreadyExists(request.Name));
            }

            if (await categoriesRepository.GetByIdAsync(request.CategoryId, cancellationToken) == null)
            {
                return Result.Failure<int>(CategoryErrors.NotFoundById(request.CategoryId));
            }

            var product = new Product
            {
                Name = request.Name,
                CategoryId = request.CategoryId
            };

            await productsRepository.CreateAsync(product, cancellationToken);

            return Result.Success(product.Id);
        }
    }
}