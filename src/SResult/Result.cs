namespace SResult;

public static class Result
{
    public static Result<TSuccessResult, TFailureReason> CreateFailureReason<TSuccessResult, TFailureReason>(TFailureReason failureReason) => failureReason;

    public static Result<object, TFailureReason> CreateFailureReason<TFailureReason>(TFailureReason failureReason) => failureReason;

    public static Result<TSuccessResult, TFailureReason> CreateSuccessResult<TSuccessResult, TFailureReason>(TSuccessResult successResult) => successResult;

    public static Result<TSuccessResult, object> CreateSuccessResult<TSuccessResult>(TSuccessResult successResult) => successResult;
}