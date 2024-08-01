using Application.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Features.Commands.UpdateProduct
{
    public static class UpdateProduct
    {
        public class Command : IRequest<bool>
        {
            public int Id { get; set; }

            public string Name { get; set; } = null!;

            public int CategoryId { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IProductsRepository productsRepository, ICategoriesRepository categoriesRepository)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .MustAsync(async (id, cancellation) =>
                        await productsRepository.GetByIdAsync(id, cancellation) != null)
                    .WithMessage("Product not found");

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

        public class Handler(IProductsRepository productsRepository) : IRequestHandler<Command, bool>
        {
            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await productsRepository.GetByIdAsync(request.Id, cancellationToken);
                product.Name = request.Name;
                product.CategoryId = request.CategoryId;
                await productsRepository.UpdateAsync(product, cancellationToken);

                return true;
            }
        }
    }
}