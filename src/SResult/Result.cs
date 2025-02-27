namespace SResult;

public static class Result
{
    public static Result<TResult, TReason> Fail<TResult, TReason>(TReason reason) => reason;

    public static Result<TResult, TReason> Success<TResult, TReason>(TResult result) => result;
}