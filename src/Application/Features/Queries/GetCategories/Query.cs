// using Application.Repositories;
// using Application.Repositories.Shared;
// using Application.Shared;
// using FluentValidation;
// using MediatR;
//
// namespace Application.Features.Queries.GetCategories;
//
// public static class GetCategories
// {
//     public class Query : IRequest<Result<PageResult<CategoryDto>>>
//     {
//         public int PageSize { get; set; }
//
//         public int PageNumber { get; set; }
//     }
//
//     public class Validator : AbstractValidator<Query>
//     {
//         public Validator()
//         {
//             RuleFor(q => q.PageSize)
//                 .GreaterThan(0);
//
//             RuleFor(q => q.PageNumber)
//                 .GreaterThan(0);
//         }
//     }
//
//     public class Handler(ICategoriesRepository categoriesRepository)
//         : IRequestHandler<Query, Result<P<CategoryDto>>>
//     {
//         public async Task<Result<PageResult<CategoryDto>>> Handle(Query request, CancellationToken cancellationToken)
//         {
//             var categories = await categoriesRepository.GetAllAsync(
//                 request.PageNumber,
//                 request.PageSize,
//                 cancellationToken);
//
//             return Result.Success(categories.Map(c => new CategoryDto
//             {
//                 Id = c.Id,
//                 Name = c.Name
//             }));
//         }
//     }
//
//     public class CategoryDto
//     {
//         public required int Id { get; init; }
//
//         public required string Name { get; init; }
//     }
// }