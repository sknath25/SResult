using System.Diagnostics.CodeAnalysis;

namespace SResult;

public sealed class Result<TResult, TReason>
{
    private readonly TResult? _result;
    private readonly TReason? _reason;
    private readonly bool _isSuccess;

    private Result(TReason? reasonForBad)
    {
        if (reasonForBad == null) throw new ArgumentNullException(nameof(reasonForBad));
        _reason = reasonForBad;
        _isSuccess = false;
    }

    private Result(TResult successResult)
    {
        if (successResult == null) throw new ArgumentNullException(nameof(successResult));
        _result = successResult;
        _isSuccess = true;
    }

    public bool IsGood()
    {
        return _isSuccess;
    }

    public bool IsBad()
    {
        return !IsGood();
    }

    public bool IsGood([NotNullWhen(true)] out TResult? result)
    {
        result = _result;
        return IsGood();
    }

    public bool IsGood([NotNullWhen(true)] out TResult? result, [NotNullWhen(false)] out TReason? reason)
    {
        result = _result;
        reason = _reason;
        return IsGood();
    }

    public bool IsBad([NotNullWhen(true)] out TReason? reason)
    {
        reason = _reason;
        return IsBad();
    }

    public Result<TResult, TReason> WhenGood(Action goodAction)
    {
        if (goodAction == null) throw new ArgumentNullException(nameof(goodAction));

        if (IsGood())
        {
            goodAction();
        }

        return this;
    }

    public Result<TResult, TReason> WhenGood(Action<TResult> goodAction)
    {
        if (goodAction == null) throw new ArgumentNullException(nameof(goodAction));

        if (IsGood(out var successResponse))
        {
            goodAction(successResponse);
        }

        return this;
    }

    public Result<TResult, TReason> WhenBad(Action badAction)
    {
        if (badAction == null) throw new ArgumentNullException(nameof(badAction));

        if (IsBad())
        {
            badAction();
        }

        return this;
    }

    public Result<TResult, TReason> WhenBad(Action<TReason> badAction)
    {
        if (badAction == null) throw new ArgumentNullException(nameof(badAction));

        if (IsBad(out var failureResponse))
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