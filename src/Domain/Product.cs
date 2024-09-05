using Domain.Shared;

namespace Domain;

public sealed class Product : BaseEntity
{
    public string Name { get; set; } = null!;

    public Category Category { get; init; } = null!;

    public int CategoryId { get; set; }
}