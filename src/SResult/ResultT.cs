using System.Diagnostics.CodeAnalysis;

namespace SResult;

public sealed class Result<TGoodResult, TReasonForBad>
{
    private readonly TGoodResult? _goodResult;
    private readonly TReasonForBad? _reasonForBad;
    private readonly bool _isSuccess;

    private Result(TReasonForBad? reasonForBad)
    {
        if (reasonForBad == null) throw new ArgumentNullException(nameof(reasonForBad));
        _reasonForBad = reasonForBad;
        _isSuccess = false;
    }

    private Result(TGoodResult successResult)
    {
        if (successResult == null) throw new ArgumentNullException(nameof(successResult));
        _goodResult = successResult;
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

    public bool IsGood([NotNullWhen(true)] out TGoodResult? goodResult)
    {
        goodResult = _goodResult;
        return IsGood();
    }

    public bool IsBad([NotNullWhen(true)] out TGoodResult? goodResult, [NotNullWhen(false)] out TReasonForBad? reasonForBad)
    {
        goodResult = _goodResult;
        reasonForBad = _reasonForBad;
        return IsGood();
    }

    public bool IsBad([NotNullWhen(true)] out TReasonForBad? reasonForBad)
    {
        reasonForBad = _reasonForBad;
        return IsBad();
    }

    public Result<TGoodResult, TReasonForBad> WhenGood(Action goodAction)
    {
        if (goodAction == null) throw new ArgumentNullException(nameof(goodAction));

        if (IsGood())
        {
            goodAction();
        }

        return this;
    }

    public Result<TGoodResult, TReasonForBad> WhenGood(Action<TGoodResult> goodAction)
    {
        if (goodAction == null) throw new ArgumentNullException(nameof(goodAction));

        if (IsGood(out var successResponse))
        {
            goodAction(successResponse);
        }

        return this;
    }

    public Result<TGoodResult, TReasonForBad> WhenBad(Action badAction)
    {
        if (badAction == null) throw new ArgumentNullException(nameof(badAction));

        if (IsBad())
        {
            badAction();
        }

        return this;
    }

    public Result<TGoodResult, TReasonForBad> WhenBad(Action<TReasonForBad> badAction)
    {
        if (badAction == null) throw new ArgumentNullException(nameof(badAction));

        if (IsBad(out var failureResponse))
        {
            badAction(failureResponse);
        }

        return this;
    }

    public static implicit operator Result<TGoodResult, TReasonForBad>(TGoodResult goodResult)
        =>  new (goodResult);

    public static implicit operator Result<TGoodResult, TReasonForBad>(TReasonForBad reasonForBad)
        => new (reasonForBad);
}