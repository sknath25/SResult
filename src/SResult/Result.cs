namespace SResult;

public static class Result
{
    public static Result<TGoodResult, TReasonForBad> Bad<TGoodResult, TReasonForBad>(TReasonForBad reasonForBad) => reasonForBad;

    public static Result<object, TReasonForBad> Bad<TReasonForBad>(TReasonForBad reasonForBad) => reasonForBad;

    public static Result<TGoodResult, TReasonForBad> Good<TGoodResult, TReasonForBad>(TGoodResult goodResult) => goodResult;

    public static Result<TGoodResult, object> Good<TGoodResult>(TGoodResult goodResult) => goodResult;
}