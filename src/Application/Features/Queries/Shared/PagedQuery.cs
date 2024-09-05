using Application.Repositories.Shared;

namespace Application.Features.Queries.Shared;

public class BaseQuery
{
    public PagedQuery PagedQuery { get; set; } = new();
}