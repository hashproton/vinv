using Application.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Features.Commands.DeleteProduct
{
    public static class DeleteProduct
    {
        public class Command : IRequest<bool>
        {
            public int Id { get; set; }
        }

        public class Validator : AbstractValidator<Command>
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

        public class Handler(IProductsRepository productsRepository) : IRequestHandler<Command, bool>
        {
            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await productsRepository.GetByIdAsync(request.Id, cancellationToken);
                if (product == null)
                    return false;

                await productsRepository.DeleteAsync(product, cancellationToken);
                return true;
            }
        }
    }
}