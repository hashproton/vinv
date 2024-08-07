using Application.Errors;
using Application.Repositories;
using Application.Shared;
using Domain;
using FluentValidation;
using MediatR;

namespace Application.Features.Commands.CreateCategory;

public static class CreateCategory
{
    public class Command : IRequest<Result<Response>>
    {
        public required string Name { get; init; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(200);
        }
    }

    public class Handler(ICategoriesRepository categoriesRepository) : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
        {
            var existingCategory = await categoriesRepository.GetByNameAsync(request.Name, cancellationToken);
            if (existingCategory != null)
            {
                return Result.Failure<Response>(CategoryErrors.AlreadyExists(request.Name));
            }

            var category = new Category { Name = request.Name };

            await categoriesRepository.CreateAsync(category, cancellationToken);

            return Result.Success(new Response(category.Id));
        }
    }

    public record Response(int Id);
}