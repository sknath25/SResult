using System.Diagnostics.CodeAnalysis;

namespace SResult;

public sealed class Result<TSuccessResult, TFailureReason>
{
    private readonly TSuccessResult? _successResult;
    private readonly TFailureReason? _failureReason;
    private readonly bool _isSuccess;

    private Result(bool isSuccess, TSuccessResult? successResult, TFailureReason? failureReason)
    {
        if (isSuccess && successResult == null) throw new ArgumentNullException(nameof(successResult));
        if (!isSuccess && failureReason == null) throw new ArgumentNullException(nameof(failureReason));

        _successResult = successResult;
        _failureReason = failureReason;
        _isSuccess = isSuccess;
    }

    public bool IsSuccess()
    {
        return _isSuccess;
    }

    public bool IsSuccess([NotNullWhen(true)] out TSuccessResult? successResult)
    {
        successResult = _successResult;
        return _isSuccess;
    }

    public bool IsSuccess([NotNullWhen(true)] out TSuccessResult? successResult, [NotNullWhen(false)] out TFailureReason? failureReason)
    {
        successResult = _successResult;
        failureReason = _failureReason;
        return _isSuccess;
    }

    public Result<TSuccessResult, TFailureReason> OnSuccess(Action actionOnSuccess)
    {
        if (actionOnSuccess == null) throw new ArgumentNullException(nameof(actionOnSuccess));

        if (IsSuccess(out _))
        {
            actionOnSuccess();
        }

        return this;
    }

    public Result<TSuccessResult, TFailureReason> OnSuccess(Action<TSuccessResult> actionOnSuccess)
    {
        if (actionOnSuccess == null) throw new ArgumentNullException(nameof(actionOnSuccess));

        if (IsSuccess(out var successResponse))
        {
            actionOnSuccess(successResponse);
        }

        return this;
    }

    public Result<TSuccessResult, TFailureReason> OnFailure(Action actionOnFailure)
    {
        if (actionOnFailure == null) throw new ArgumentNullException(nameof(actionOnFailure));

        if (!IsSuccess(out _, out _))
        {
            actionOnFailure();
        }

        return this;
    }

    public Result<TSuccessResult, TFailureReason> OnFailure(Action<TFailureReason> actionOnFailure)
    {
        if (actionOnFailure == null) throw new ArgumentNullException(nameof(actionOnFailure));

        if (!IsSuccess(out _, out var failureResponse))
        {
            actionOnFailure(failureResponse);
        }

        return this;
    }

    public static implicit operator Result<TSuccessResult, TFailureReason>(TSuccessResult successResult)
        =>  new (true, successResult, default);

    public static implicit operator Result<TSuccessResult, TFailureReason>(TFailureReason failureReason)
        => new (false, default, failureReason);

}

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

