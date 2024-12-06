using System.Diagnostics.CodeAnalysis;

namespace SResult;

public sealed class Result<TSuccessResult, TFailureResult>
{
    private readonly TSuccessResult? _successResult;
    private readonly TFailureResult? _failureResult;
    private readonly bool _isSuccess;

    private Result(bool isSuccess, TSuccessResult? successResult, TFailureResult? failureResult)
    {
        _successResult = successResult;
        _failureResult = failureResult;
        _isSuccess = isSuccess;
    }

    public bool IsSuccess([NotNullWhen(true)] out TSuccessResult? successResult, [NotNullWhen(false)] out TFailureResult? failureResult)
    {
        successResult = _successResult;
        failureResult = _failureResult;
        return _isSuccess;
    }

    public bool IsSuccess([NotNullWhen(true)] out TSuccessResult? successResult)
    {
        successResult = _successResult;
        return _isSuccess;
    }

    public Result<TSuccessResult, TFailureResult> OnSuccess(Action actionOnSuccess)
    {
        if (IsSuccess(out _))
        {
            actionOnSuccess();
        }

        return this;
    }

    public Result<TSuccessResult, TFailureResult> OnSuccess(Action<TSuccessResult> actionOnSuccess)
    {
        if(IsSuccess(out var successResponse))
        {
            actionOnSuccess(successResponse);
        }

        return this;
    }

    public Result<TSuccessResult, TFailureResult> OnFailure(Action actionOnFailure)
    {
        if (!IsSuccess(out _, out _))
        {
            actionOnFailure();
        }

        return this;
    }

    public Result<TSuccessResult, TFailureResult> OnFailure(Action<TFailureResult> actionOnFailure)
    {
        if (!IsSuccess(out _, out var failureResponse))
        {
            actionOnFailure(failureResponse);
        }

        return this;
    }

    public static Result<TSuccessResult, TFailureResult> CreateSuccessResult(TSuccessResult successResult)
    {
        if(successResult == null) throw new ArgumentNullException(nameof(successResult));

        return new Result<TSuccessResult, TFailureResult>(true, successResult, default);
    }

    public static Result<TSuccessResult, TFailureResult> CreateFailureResult(TFailureResult failureResult)
    {
        if (failureResult == null) throw new ArgumentNullException(nameof(failureResult));

        return new Result<TSuccessResult, TFailureResult>(false, default, failureResult);
    }

    public static implicit operator Result<TSuccessResult, TFailureResult> (TSuccessResult successResult)
        => CreateSuccessResult(successResult);

    public static implicit operator Result<TSuccessResult, TFailureResult>(TFailureResult failureResult)
        => CreateFailureResult(failureResult);
}
