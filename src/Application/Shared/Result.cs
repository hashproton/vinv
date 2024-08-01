namespace Application.Shared
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public Error? Error { get; }

        private Result(bool isSuccess, T? value, Error? error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value) => new(true, value, null);

        public static Result<T> Failure(Error error) => new(false, default, error);
    }

    public class Result
    {
        public bool IsSuccess => Error == null;
        public Error? Error { get; }

        private Result(Error? error)
        {
            Error = error;
        }

        public static Result Success() => new(null);

        public static Result Failure(Error error) => new(error);
    }
}
