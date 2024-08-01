using Application.Shared;

namespace Application.Errors
{
    public class CategoryErrors
    {
        public static Error AlreadyExists => Error.Create(ErrorType.AlreadyExists, "Category already exists.");

        public static Error NotFound => Error.Create(ErrorType.NotFound, "Category not found.");

        public static Error NotFoundById(int id) => Error.Create(ErrorType.NotFound, $"Category with id {id} not found.");
    }
}
