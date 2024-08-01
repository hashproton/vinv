using Application.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Features.Queries.GetProductById
{
    public static class GetProductById
    {
        public class Query : IRequest<ProductDto?>
        {
            public int Id { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator(IProductsRepository productsRepository)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .MustAsync(async (id, cancellation) =>
                        await productsRepository.GetByIdAsync(id, cancellation) != null)
                    .WithMessage("Product not found");
            }
        }

        public class Handler(IProductsRepository productsRepository) : IRequestHandler<Query, ProductDto?>
        {
            public async Task<ProductDto?> Handle(Query request, CancellationToken cancellationToken)
            {
                var product = await productsRepository.GetByIdAsync(request.Id, cancellationToken);
                return product == null ? null : new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    CategoryId = product.CategoryId
                };
            }
        }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }
}