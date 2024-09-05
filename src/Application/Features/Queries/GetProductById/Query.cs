namespace Application.Features.Queries.GetProductById;

public static class GetProductById
{
    public class Query : IRequest<Result<ProductDto>>
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

    public class Handler(IProductsRepository productsRepository) : IRequestHandler<Query, Result<ProductDto>>
    {
        public async Task<Result<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var product = await productsRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
            {
                return Result.Failure<ProductDto>(ProductErrors.NotFoundById(request.Id));
            }

            return Result.Success(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
            });
        }
    }

    public class ProductDto
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required int CategoryId { get; init; }
    }
}