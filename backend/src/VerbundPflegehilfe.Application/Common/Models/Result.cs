namespace VerbundPflegehilfe.Application.Common.Models;

public class Result<T>
{
    public bool Succeeded { get; init; }
    public string[] Errors { get; init; }
    public T? Data { get; init; }
    public string Message { get; init; }

    public static Result<T> Success(T data, string message = "Operation successful")
    {
        return new Result<T>
        {
            Succeeded = true,
            Data = data,
            Message = message,
            Errors = []
        };
    }

    public static Result<T> Failure(IEnumerable<string> errors)
    {
        return new Result<T>
        {
            Succeeded = false,
            Errors = errors.ToArray(),
            Data = default
        };
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>
        {
            Succeeded = false,
            Errors = [error],
            Data = default
        };
    }
}