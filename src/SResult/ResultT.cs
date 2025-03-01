using System.Diagnostics.CodeAnalysis;

namespace SResult;

public class Result<TValue, TReason>
{
    private readonly TValue? _value;
    private readonly TReason? _reason;
    private readonly bool _isSuccess;

    protected Result(TReason? reason)
    {
        if (reason == null) throw new ArgumentNullException(nameof(reason));
        _reason = reason;
        _isSuccess = false;
    }

    protected Result(TValue value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        _value = value;
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

    public bool IsSuccess([NotNullWhen(true)] out TValue? value)
    {
        value = _value;
        return IsSuccess();
    }

    public bool IsSuccess([NotNullWhen(true)] out TValue? value, [NotNullWhen(false)] out TReason? reason)
    {
        value = _value;
        reason = _reason;
        return IsSuccess();
    }

    public bool IsFail([NotNullWhen(true)] out TReason? reason)
    {
        reason = _reason;
        return IsFail();
    }

    public bool IsFail([NotNullWhen(false)] out TValue? value, [NotNullWhen(true)] out TReason? reason)
    {
        value = _value;
        reason = _reason;
        return IsFail();
    }

    public Result<TValue, TReason> OnSuccess(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsSuccess())
        {
            action();
        }

        return this;
    }

    public Result<TValue, TReason> OnSuccess(Action<TValue> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsSuccess(out var successResponse))
        {
            action(successResponse);
        }

        return this;
    }

    public Result<TValue, TReason> OnFail(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsFail())
        {
            action();
        }

        return this;
    }

    public Result<TValue, TReason> OnFail(Action<TReason> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsFail(out var failureResponse))
        {
            action(failureResponse);
        }

        return this;
    }

    public static implicit operator Result<TValue, TReason>(TValue value)
        => new(value);

    public static implicit operator Result<TValue, TReason>(TReason reason)
        => new(reason);
}

public class Result<TResult> : Result<TResult, Reason>
{
    protected Result(TResult result) : base(result)
    {
    }

    protected Result(Reason reason) : base(reason)
    {
    }

    public static implicit operator Result<TResult>(TResult result) => new(result);

    public static implicit operator Result<TResult>(Reason reason) => new(reason);
}

public static class Result
{
    public static Result<TResult, TReason> Fail<TResult, TReason>(TReason reason) => reason;
    public static Result<TResult, Reason> Fail<TResult>(Reason reason) => reason;
    public static Result<TResult, TReason> Success<TResult, TReason>(TResult result) => result;
    public static Result<TResult, Reason> Success<TResult>(TResult result) => result;
}
