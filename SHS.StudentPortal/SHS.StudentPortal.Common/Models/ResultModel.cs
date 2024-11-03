namespace SHS.StudentPortal.Common.Models;

public class ResultModel
{
    public bool IsSuccess { get; protected set; } = false;

    public ErrorModel? Error { get; protected set; }

    public static ResultModel Success() => new() { IsSuccess = true, Error = null };

    public static ResultModel Fail(ErrorModel error) => new() { Error = error };
}

public class ResultModel<T> : ResultModel
{
    public T? Data { get; private set; }

    public static ResultModel<T> Success(T data) => new() { Data = data, IsSuccess = true, Error = null };

    public static new ResultModel<T> Fail(ErrorModel error) => new() { Error = error };
}

public class ErrorModel
{
    public ErrorModel(string type, string message)
    {
        Type = type;
        Message = message;
    }

    public string Type { get; set; }

    public string Message { get; set; }
}
