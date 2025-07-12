using System.Diagnostics.CodeAnalysis;

namespace SResult;

public class Result<TValue>
{
    public TValue? Value { get; }
    public IFailure? Failure { get; }

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Failure))]
    public bool IsSuccess { get; }

    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(true, nameof(Failure))]
    public bool IsFailed { get; }

    public Result(IFailure failure)
    {
        Failure = failure ?? throw new ArgumentNullException(nameof(failure));
        IsSuccess = false;
        IsFailed = !IsSuccess;
    }

    public Result(TValue value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
        IsSuccess = true;
        IsFailed = !IsSuccess;
    }

    public Result<TValue> OnSuccess(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsSuccess)
        {
            action();
        }

        return this;
    }

    public Result<TValue> OnSuccess(Action<TValue> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (IsSuccess)
        {
            action(Value);
        }

        return this;
    }

    public Result<TValue> OnFail(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (!IsSuccess)
        {
            action();
        }

        return this;
    }

    public Result<TValue> OnFail(Action<IFailure> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        if (!IsSuccess)
        {
            action(Failure);
        }

        return this;
    }

    public static implicit operator Result<TValue>(TValue value) => new(value);

    public static implicit operator Result<TValue>(Failure failure) => new(failure);
}

public sealed class Result
{
    public static Result<TValue> Success<TValue>(TValue value) => new(value);
    public static Result<TValue> Fail<TValue>(IFailure failure) => new(failure);
    public static Result<TValue> Fail<TValue>(Failure failure) => new(failure);
    public static Result<TValue> Fail<TValue>(string failure) => new(Failure.Error(failure));
}
