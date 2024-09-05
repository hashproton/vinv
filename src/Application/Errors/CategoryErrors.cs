namespace Application.Errors;

public static class CategoryErrors
{
    public static Error AlreadyExists(string name) => Error.Create(ErrorType.AlreadyExists, $"Category with name {name} already exists.");

    public static Error NotFound => Error.Create(ErrorType.NotFound, "Category not found.");

    public static Error NotFoundById(int id) => Error.Create(ErrorType.NotFound, $"Category with id {id} not found.");
}