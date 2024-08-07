using Application.Shared;
using FluentValidation.Results;

namespace Infra.Errors;

public static class ValidationError
{
    public static Error Create(IEnumerable<ValidationFailure> failures)
    {
        var errorMessages = failures
            .Select(f => f.ErrorMessage)
            .ToArray();

        return Error.Create(ErrorType.ValidationError, string.Join(", ", errorMessages));
    }
}