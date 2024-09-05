using Application.Repositories.Shared;
using Application.Shared;
using FluentValidation;
using MediatR;

namespace Application.Features.Queries.Shared;

public class BaseQuery
{
    public PagedQuery PagedQuery { get; set; } = new();
}

// public class PagedQueryValidator : AbstractValidator<PagedQuery>
// {
//     public PagedQueryValidator(int maxPageSize)
//     {
//         RuleFor(pq => pq.Skip)
//             .GreaterThanOrEqualTo(0);
//
//         RuleFor(pq => pq.Take)
//             .GreaterThanOrEqualTo(1)
//             .LessThanOrEqualTo(maxPageSize);
//     }
// }