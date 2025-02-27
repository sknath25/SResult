namespace SResult;

public static class Result
{
    public static Result<TGoodResult, TReasonForBad> Bad<TGoodResult, TReasonForBad>(TReasonForBad reason) => reason;

    public static Result<object, TReasonForBad> Bad<TReasonForBad>(TReasonForBad reason) => reason;

    public static Result<TGoodResult, TReasonForBad> Good<TGoodResult, TReasonForBad>(TGoodResult result) => result;

    public static Result<TGoodResult, object> Good<TGoodResult>(TGoodResult result) => result;
}