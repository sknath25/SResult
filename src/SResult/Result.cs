using System.Diagnostics.CodeAnalysis;

namespace SResult;

public class Result2<TValue>
{
    private readonly TValue? _value;
    private readonly IReason? _reason;
    private readonly bool _isSuccess;

    public Result2(IReason reason)
    {
        _reason = reason ?? throw new ArgumentNullException(nameof(reason));
        _isSuccess = false;
    }

    public Result2(TValue value)
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

    public Result2<TValue> OnSuccess(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsSuccess())
        {
            action();
        }

        return this;
    }

    public Result2<TValue> OnSuccess(Action<TValue> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsSuccess(out var value))
        {
            action(value);
        }

        return this;
    }
    
    public Result2<TValue> OnFail(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsFail())
        {
            action();
        }

        return this;
    }

    public Result2<TValue> OnFail(Action<IReason> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsFail(out var reason))
        {
            action(reason);
        }

        return this;
    }

    public static implicit operator Result2<TValue>(TValue value)
    => new(value);

    public static implicit operator Result2<TValue>(Reason reason)
    => new(reason);    
}

public sealed class Result 
{
    public static Result2<TValue> Fail<TValue>(IReason reason) => new(reason);
    public static Result2<TValue> Fail<TValue>(Reason reason) => new(reason);
    public static Result2<TValue> Success<TValue>(TValue value) => new(value);
}
