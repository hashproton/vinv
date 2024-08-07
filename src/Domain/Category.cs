using Domain.Shared;

namespace Domain;

public sealed class Category : BaseEntity
{
    public string Name { get; set; } = null!;

    public List<Product> Products { get; init; } = [];
}
