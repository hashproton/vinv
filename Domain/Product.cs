using Domain.Shared;

namespace Domain;

public sealed class Product : BaseEntity
{
    public string Name { get; set; }

    public Category Category { get; set; }

    public int CategoryId { get; set; }
}
