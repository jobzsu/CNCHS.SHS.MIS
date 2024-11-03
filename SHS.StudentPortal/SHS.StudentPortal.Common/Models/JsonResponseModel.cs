namespace SHS.StudentPortal.Common.Models;

public class JsonResponseModel
{
    public bool IsSuccess { get; set; } = false;

    public object? Data { get; set; }

    public string? Message { get; set; }

    public int Status { get; set; }

    JsonResponseModel(bool isSuccess, object? data, string? message, int status)
    {
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
        Status = status;
    }

    public static JsonResponseModel Error(string? message, int status = 500)
    {
        return new JsonResponseModel(false, null, message, status);
    }

    public static JsonResponseModel Success(object? data, string? message, int status = 200)
    {
        return new JsonResponseModel(true, data, message, status);
    }
}
