﻿namespace Application.Features.Commands.RemoveProductFromCategory;

public static class RemoveProductFromCategory
{
    public class Command : IRequest<Result>
    {
        public required int ProductId { get; init; }
        
        public required int CategoryId { get; init; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.CategoryId)
                .GreaterThan(0);

            RuleFor(c => c.ProductId)
                .GreaterThan(0);
        }
    }

    public class Handler(
        IProductsRepository productsRepository,
        ICategoriesRepository categoriesRepository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var existingCategory = await categoriesRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            if (existingCategory == null)
            {
                return Result.Failure(CategoryErrors.NotFoundById(request.CategoryId));
            }

            var existingProduct = await productsRepository.GetByIdAsync(request.ProductId, cancellationToken);
            if (existingProduct == null)
            {
                return Result.Failure(ProductErrors.NotFoundById(request.ProductId));
            }

            var productToRemove = existingCategory.Products.Find(p => p.Id == existingProduct.Id);
            if (productToRemove == null)
            {
                return Result.Failure(ProductErrors.NotFoundById(request.ProductId));
            }

            existingCategory.Products.Remove(productToRemove);

            await categoriesRepository.UpdateAsync(existingCategory, cancellationToken);
            
            return Result.Success();
        }
    }
}