using Domain.Shared;

namespace Domain;

public enum TenantStatus
{
    Inactive,
    Active,
    Demo
}

public sealed class Tenant : BaseEntity
{
    public string Name { get; set; } = null!;

    public TenantStatus Status { get; set; } = TenantStatus.Inactive;
}