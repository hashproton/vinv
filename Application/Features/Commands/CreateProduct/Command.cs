using Application.Repositories;
using Domain;
using FluentValidation;
using MediatR;

namespace Application.Features.Commands.CreateProduct
{
    public static class CreateProduct
    {
        public class Command : IRequest<int>
        {
            public string Name { get; set; }
            public int CategoryId { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(ICategoriesRepository categoriesRepository)
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MaximumLength(200);

                RuleFor(x => x.CategoryId)
                    .GreaterThan(0)
                    .MustAsync(async (categoryId, cancellation) =>
                        await categoriesRepository.GetByIdAsync(categoryId, cancellation) != null)
                    .WithMessage("Category not found");
            }
        }

        public class Handler(IProductsRepository productsRepository) : IRequestHandler<Command, int>
        {
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = new Product
                {
                    Name = request.Name,
                    CategoryId = request.CategoryId
                };

                await productsRepository.CreateAsync(product, cancellationToken);
                return product.Id;
            }
        }
    }
}