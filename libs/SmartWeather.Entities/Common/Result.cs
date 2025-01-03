namespace SmartWeather.Entities.Common;

public class Result
{
    public Result(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public string ErrorMessage { get; }
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true, string.Empty);

    public static Result Failure(string errorMessage) => new(false, errorMessage);
}

public class Result<T> : Result where T : class
{
    public Result(bool isSuccess, string errorMessage, T value)
        : base(isSuccess, errorMessage)
    {
        Value = value;
    }

    public T Value { get; }

    public static Result<T> Success(T value) => new(true, string.Empty, value);

    public new static Result<T> Failure(string errorMessage) => new(false, errorMessage, null!);
}
