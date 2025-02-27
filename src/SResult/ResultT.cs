using System.Diagnostics.CodeAnalysis;

namespace SResult;

public sealed class Result<TResult, TReason>
{
    private readonly TResult? _result;
    private readonly TReason? _reason;
    private readonly bool _isSuccess;

    private Result(TReason? reason)
    {
        if (reason == null) throw new ArgumentNullException(nameof(reason));
        _reason = reason;
        _isSuccess = false;
    }

    private Result(TResult result)
    {
        if (result == null) throw new ArgumentNullException(nameof(result));
        _result = result;
        _isSuccess = true;
    }

    public bool IsSuccess()
    {
        return _isSuccess;
    }

    public bool IsFail()
    {
        return !IsSuccess();
    }

    public bool IsSuccess([NotNullWhen(true)] out TResult? result)
    {
        result = _result;
        return IsSuccess();
    }

    public bool IsSuccess([NotNullWhen(true)] out TResult? result, [NotNullWhen(false)] out TReason? reason)
    {
        result = _result;
        reason = _reason;
        return IsSuccess();
    }

    public bool IsFail([NotNullWhen(true)] out TReason? reason)
    {
        reason = _reason;
        return IsFail();
    }

    public bool IsFail([NotNullWhen(false)] out TResult? result, [NotNullWhen(true)] out TReason? reason)
    {
        result = _result;
        reason = _reason;
        return IsFail();
    }

    public Result<TResult, TReason> OnSuccess(Action goodAction)
    {
        if (goodAction == null) throw new ArgumentNullException(nameof(goodAction));

        if (IsSuccess())
        {
            goodAction();
        }

        return this;
    }

    public Result<TResult, TReason> OnSuccess(Action<TResult> goodAction)
    {
        if (goodAction == null) throw new ArgumentNullException(nameof(goodAction));

        if (IsSuccess(out var successResponse))
        {
            goodAction(successResponse);
        }

        return this;
    }

    public Result<TResult, TReason> OnFail(Action badAction)
    {
        if (badAction == null) throw new ArgumentNullException(nameof(badAction));

        if (IsFail())
        {
            badAction();
        }

        return this;
    }

    public Result<TResult, TReason> OnFail(Action<TReason> badAction)
    {
        if (badAction == null) throw new ArgumentNullException(nameof(badAction));

        if (IsFail(out var failureResponse))
        {
            badAction(failureResponse);
        }

        return this;
    }

    public static implicit operator Result<TResult, TReason>(TResult result)
        =>  new (result);

    public static implicit operator Result<TResult, TReason>(TReason reason)
        => new (reason);
}