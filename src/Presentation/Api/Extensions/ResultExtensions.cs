using Application.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Presentation.Api.Extensions;

public static class ResultExtensions
{
    public static IResult MapError<T>(this Result<T> result)
    {
        ThowIfSuccess(result);

        return MapErrorInternal(result.Error!);
    }

    public static IResult MapError(this Result result)
    {
        ThowIfSuccess(result);

        return MapErrorInternal(result.Error!);
    }

    private static IResult MapErrorInternal(Error error)
    {
        var statusCode = GetStatusCode(error.Type);
        var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode) ?? "An error occurred";

        var problemDetails = new ProblemDetails
        {
            Title = reasonPhrase,
            Detail = error.Description,
            Status = statusCode,
            Extensions =
            {
                ["errorCode"] = error.Type.ToString()
            }
        };

        return error.Type switch
        {
            ErrorType.AlreadyExists => Results.Conflict(problemDetails),
            ErrorType.NotFound => Results.NotFound(problemDetails),
            ErrorType.ValidationError => Results.BadRequest(problemDetails),
            ErrorType.Unauthorized => Results.Unauthorized(),
            ErrorType.Forbidden => Results.Forbid(),
            _ => Results.StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    private static int GetStatusCode(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.AlreadyExists => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.ValidationError => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static void ThowIfSuccess(ResultBase result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot map a successful result to an error.");
        }
    }
}