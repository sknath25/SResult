using System.Diagnostics.CodeAnalysis;

namespace SResult;

public sealed class Result<TSuccessResult, TFailureReason>
{
    private readonly TSuccessResult? _successResult;
    private readonly TFailureReason? _failureReason;
    private readonly bool _isSuccess;

    private Result(bool isSuccess, TSuccessResult? successResult, TFailureReason? failureReason)
    {
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
        if (IsSuccess(out _))
        {
            actionOnSuccess();
        }

        return this;
    }

    public Result<TSuccessResult, TFailureReason> OnSuccess(Action<TSuccessResult> actionOnSuccess)
    {
        if(IsSuccess(out var successResponse))
        {
            actionOnSuccess(successResponse);
        }

        return this;
    }

    public Result<TSuccessResult, TFailureReason> OnFailure(Action actionOnFailure)
    {
        if (!IsSuccess(out _, out _))
        {
            actionOnFailure();
        }

        return this;
    }

    public Result<TSuccessResult, TFailureReason> OnFailure(Action<TFailureReason> actionOnFailure)
    {
        if (!IsSuccess(out _, out var failureResponse))
        {
            actionOnFailure(failureResponse);
        }

        return this;
    }

    public static Result<TSuccessResult, TFailureReason> CreateSuccessResult(TSuccessResult successResult)
    {
        if(successResult == null) throw new ArgumentNullException(nameof(successResult));

        return new Result<TSuccessResult, TFailureReason>(true, successResult, default);
    }

    public static Result<TSuccessResult, TFailureReason> CreateFailureReason(TFailureReason failureReason)
    {
        if (failureReason == null) throw new ArgumentNullException(nameof(failureReason));

        return new Result<TSuccessResult, TFailureReason>(false, default, failureReason);
    }

    public static implicit operator Result<TSuccessResult, TFailureReason> (TSuccessResult successResult)
        => CreateSuccessResult(successResult);

    public static implicit operator Result<TSuccessResult, TFailureReason>(TFailureReason failureReason)
        => CreateFailureReason(failureReason);
}
