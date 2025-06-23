using System.Diagnostics.CodeAnalysis;

namespace SResult;

public class Result<TValue>
{
    private readonly TValue? _value;
    private readonly IFailure? _failure;
    private readonly bool _isSuccess;

    public Result(IFailure failure)
    {
        _failure = failure ?? throw new ArgumentNullException(nameof(failure));
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

    public bool IsSuccess([NotNullWhen(true)] out TValue? value, [NotNullWhen(false)] out IFailure? failure)
    {
        value = _value;
        failure = _failure;
        return IsSuccess();
    }

    public bool IsFail([NotNullWhen(true)] out IFailure? failure)
    {
        failure = _failure;
        return IsFail();
    }

    public bool IsFail([NotNullWhen(false)] out TValue? value, [NotNullWhen(true)] out IFailure? failure)
    {
        value = _value;
        failure = _failure;
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

    public Result<TValue> OnFail(Action<IFailure> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsFail(out var failure))
        {
            action(failure);
        }

        return this;
    }

    public static implicit operator Result<TValue>(TValue value)
    => new(value);

    public static implicit operator Result<TValue>(Failure failure)
    => new(failure);    
}

public sealed class Result 
{
    public static Result<TValue> Fail<TValue>(IFailure failure) => new(failure);
    public static Result<TValue> Fail<TValue>(Failure failure) => new(failure);
    public static Result<TValue> Success<TValue>(TValue value) => new(value);
}
