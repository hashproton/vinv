using Application.Shared;

namespace Application.Errors;

public static class TenantErrors
{
    public static Error AlreadyExists(string name) => Error.Create(ErrorType.AlreadyExists, $"Tenant with name {name} already exists.");

    public static Error NotFound => Error.Create(ErrorType.NotFound, "Tenant not found.");

    public static Error NotFoundById(int id) => Error.Create(ErrorType.NotFound, $"Tenant with id {id} not found.");
}