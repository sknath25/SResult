namespace SResult;

public static class Result
{
    public static Result<TGoodResult, TReasonForBad> CreateFailure<TGoodResult, TReasonForBad>(TReasonForBad reasonForBad) => reasonForBad;

    public static Result<object, TReasonForBad> Bad<TReasonForBad>(TReasonForBad reasonForBad) => reasonForBad;

    public static Result<TGoodResult, TReasonForBad> CreateSuccess<TGoodResult, TReasonForBad>(TGoodResult goodResult) => goodResult;

    public static Result<TGoodResult, object> Good<TGoodResult>(TGoodResult goodResult) => goodResult;
}