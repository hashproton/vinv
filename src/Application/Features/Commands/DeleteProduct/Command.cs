using Application.Errors;
using Application.Repositories;
using Application.Shared;
using FluentValidation;
using MediatR;

namespace Application.Features.Commands.DeleteProduct
{
    public static class DeleteProduct
    {
        public class Command : IRequest<Result>
        {
            public required int Id { get; init; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Id)
                    .GreaterThan(0);
            }
        }

        public class Handler(IProductsRepository productsRepository) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await productsRepository.GetByIdAsync(request.Id, cancellationToken);
                if (product == null)
                {
                    return Result.Failure(ProductErrors.NotFoundById(request.Id));
                }

                await productsRepository.DeleteAsync(product, cancellationToken);
                
                return Result.Success();
            }
        }
    }
}