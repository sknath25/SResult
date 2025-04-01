using System.Diagnostics.CodeAnalysis;

namespace SResult;

public class Result2<TResult>
{
    private readonly TResult? _result;
    private readonly IFail? _reason;
    private readonly bool _isSuccess;

    public Result2(IFail reason)
    {
        _reason = reason ?? throw new ArgumentNullException(nameof(reason));
        _isSuccess = false;
    }

    public Result2(TResult result)
    {
        _result = result ?? throw new ArgumentNullException(nameof(result));
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

    public bool IsSuccess([NotNullWhen(true)] out TResult? result, [NotNullWhen(false)] out IFail? reason)
    {
        result = _result;
        reason = _reason;
        return IsSuccess();
    }

    public bool IsFail([NotNullWhen(true)] out IFail? reason)
    {
        reason = _reason;
        return IsFail();
    }

    public bool IsFail([NotNullWhen(false)] out TResult? result, [NotNullWhen(true)] out IFail? reason)
    {
        result = _result;
        reason = _reason;
        return IsFail();
    }

    public Result2<TResult> OnSuccess(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsSuccess())
        {
            action();
        }

        return this;
    }

    public Result2<TResult> OnSuccess(Action<TResult> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsSuccess(out var successResponse))
        {
            action(successResponse);
        }

        return this;
    }
    public Result2<TResult> OnFail(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsFail())
        {
            action();
        }

        return this;
    }

    public Result2<TResult> OnFail(Action<IFail> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsFail(out var failureResponse))
        {
            action(failureResponse);
        }

        return this;
    }

    public static implicit operator Result2<TResult>(TResult result)
    => new(result);

    public static implicit operator Result2<TResult>(Reason reason)
        => new(reason);

    
}

public sealed class Result2 
{
    public static Result2<TValue> Fail<TValue>(IFail reason) => new(reason);
    public static Result2<TValue> Success<TValue>(TValue result) => new(result);
}
