namespace SResult;

public static class Result
{
    public static Result<TSuccessResult, TFailureReason> CreateFailureReason<TSuccessResult, TFailureReason>(TFailureReason failureReason)
    {
        return failureReason;
    }

    public static Result<object, TFailureReason> CreateFailureReason<TFailureReason>(TFailureReason failureReason)
    {
        return failureReason;
    }

    public static Result<TSuccessResult, TFailureReason> CreateSuccessResult<TSuccessResult, TFailureReason>(TSuccessResult successResult)
    {
        return successResult;
    }

    public static Result<TSuccessResult, object> CreateSuccessResult<TSuccessResult>(TSuccessResult successResult)
    {
        return successResult;
    }
}