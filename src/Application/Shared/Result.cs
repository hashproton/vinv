namespace Application.Shared;

public class Result<T> : ResultBase
{
    public T? Value { get; }

    private Result(T? value, Error? error) : base(error)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(value, null);

    public static Result<T> Failure(Error error) => new(default, error);
}

public class Result : ResultBase
{
    private Result(Error? error) : base(error)
    {
    }

    public static Result Success() => new(null);

    public static Result Failure(Error error) => new(error);

    public static Result<T> Success<T>(T value) => Result<T>.Success(value);

    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);
}

public abstract class ResultBase(Error? error)
{
    public bool IsSuccess => Error == null;
    public Error? Error { get; } = error;
}