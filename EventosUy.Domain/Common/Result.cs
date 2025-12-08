namespace EventosUy.Domain.Common
{
    public class Result 
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public List<string> Errors { get; } = [];

        private Result (bool isSuccess, List<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Success() => new (true, []);
        public static Result Failure(List<string> errors) => new (false, errors);
        public static Result Failure(string error) => new(false, [error]);
    }

    public class Result<T>
    {
        public T? Value { get; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public List<string> Errors { get; }

        private Result(T? value, bool isSuccess, List<string> errors) 
        {
            Value = value;
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result<T> Success(T value) => new (value, true, []);
        public static Result<T> Failure(List<string> errors) => new (default, false, errors);
        public static Result<T> Failure(string error) => new(default, false, [error]);
    }
}
