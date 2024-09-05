using Domain.Shared;

namespace Domain;

public sealed class Tenant : BaseEntity
{
    public string Name { get; set; } = null!;
}