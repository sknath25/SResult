using System.Diagnostics.CodeAnalysis;

namespace SResult;

public class Result<TValue>
{
    private readonly TValue? _value;
    private readonly IReason? _reason;
    private readonly bool _isSuccess;

    public Result(IReason reason)
    {
        _reason = reason ?? throw new ArgumentNullException(nameof(reason));
        _isSuccess = false;
    }

    public Result(TValue value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
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

    public bool IsSuccess([NotNullWhen(true)] out TValue? value, [NotNullWhen(false)] out IReason? reason)
    {
        value = _value;
        reason = _reason;
        return IsSuccess();
    }

    public bool IsFail([NotNullWhen(true)] out IReason? value)
    {
        value = _reason;
        return IsFail();
    }

    public bool IsFail([NotNullWhen(false)] out TValue? value, [NotNullWhen(true)] out IReason? reason)
    {
        value = _value;
        reason = _reason;
        return IsFail();
    }

    public Result<TValue> OnSuccess(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsSuccess())
        {
            action();
        }

        return this;
    }

    public Result<TValue> OnSuccess(Action<TValue> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsSuccess(out var value))
        {
            action(value);
        }

        return this;
    }
    
    public Result<TValue> OnFail(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsFail())
        {
            action();
        }

        return this;
    }

    public Result<TValue> OnFail(Action<IReason> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsFail(out var reason))
        {
            action(reason);
        }

        return this;
    }

    public static implicit operator Result<TValue>(TValue value)
    => new(value);

    public static implicit operator Result<TValue>(Reason reason)
    => new(reason);    
}

public sealed class Result 
{
    public static Result<TValue> Fail<TValue>(IReason reason) => new(reason);
    public static Result<TValue> Fail<TValue>(Reason reason) => new(reason);
    public static Result<TValue> Success<TValue>(TValue value) => new(value);
}
