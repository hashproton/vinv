using Application.Shared;

namespace Api.Extensions
{
    public static class ResultExtensions
    {
        public static IResult MapError<T>(this Result<T> result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Cannot map a successful result to an error.");
            }

            return MapErrorInternal(result.Error!);
        }

        public static IResult MapError(this Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Cannot map a successful result to an error.");
            }

            return MapErrorInternal(result.Error!);
        }

        private static IResult MapErrorInternal(Error error)
        {
            return error.Type switch
            {
                ErrorType.AlreadyExists => Results.Conflict(error.Description),
                ErrorType.NotFound => Results.NotFound(error.Description),
                ErrorType.ValidationError => Results.BadRequest(error.Description),
                ErrorType.Unauthorized => Results.Unauthorized(),
                ErrorType.Forbidden => Results.Forbid(),
                _ => Results.StatusCode(StatusCodes.Status500InternalServerError)
            };
        }
    }
}