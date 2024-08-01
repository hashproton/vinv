using Application.Errors;
using Application.Repositories;
using Application.Shared;
using FluentValidation;
using MediatR;

namespace Application.Features.Commands.DeleteCategory
{
    public static class DeleteCategory
    {
        public class Command : IRequest<Result>
        {
            public int Id { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
            }
        }

        public class Handler(ICategoriesRepository categoriesRepository) : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var category = await categoriesRepository.GetByIdAsync(request.Id, cancellationToken);
                if (category == null)
                {
                    return Result.Failure(CategoryErrors.NotFoundById(request.Id));
                }

                await categoriesRepository.DeleteAsync(category, cancellationToken);

                return Result.Success();
            }
        }
    }
}