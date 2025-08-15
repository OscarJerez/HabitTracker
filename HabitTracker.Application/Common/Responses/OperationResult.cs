namespace HabitTracker.Application.Common.Responses;

public class OperationResult<T>
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public T? Data { get; set; }

    //  Static factory method for success
    public static OperationResult<T> Success(T data)
    {
        return new OperationResult<T>
        {
            IsSuccess = true,
            Data = data
        };
    }

    //  Static factory method for OK
    public static OperationResult<T> Ok(T data)
    {
        return new OperationResult<T>
        {
            IsSuccess = true,
            Data = data
        };
    }

    //  Static factory method for failure
    public static OperationResult<T> Fail(string errorMessage)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };


    }
}
