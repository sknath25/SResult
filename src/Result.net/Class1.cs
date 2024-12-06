using System.Diagnostics.CodeAnalysis;

namespace Result.net;

public sealed class Result<TSuccessResponse, TFailureResponse>
{
    private readonly TSuccessResponse? _successResponse;
    private readonly TFailureResponse? _failureResponse;
    private readonly bool _isSuccess;

    private Result(bool isSuccess, TSuccessResponse? successResponse, TFailureResponse? failureResponse)
    {
        _successResponse = successResponse;
        _failureResponse = failureResponse;
        _isSuccess = isSuccess;
    }

    public bool IsSuccess([NotNullWhen(true)] out TSuccessResponse? successResponse, [NotNullWhen(false)] out TFailureResponse? failureResponse)
    {
        successResponse = _successResponse;
        failureResponse = _failureResponse;
        return _isSuccess;
    }

    public bool IsSuccess([NotNullWhen(true)] out TSuccessResponse? successResponse)
    {
        successResponse = _successResponse;
        return _isSuccess;
    }

    public Result<TSuccessResponse, TFailureResponse> OnSuccess(Action actionOnSuccess)
    {
        if (IsSuccess(out _))
        {
            actionOnSuccess();
        }

        return this;
    }

    public Result<TSuccessResponse, TFailureResponse> OnSuccess(Action<TSuccessResponse> actionOnSuccess)
    {
        if(IsSuccess(out var successResponse))
        {
            actionOnSuccess(successResponse);
        }

        return this;
    }

    public Result<TSuccessResponse, TFailureResponse> OnFailure(Action actionOnFailure)
    {
        if (!IsSuccess(out _, out _))
        {
            actionOnFailure();
        }

        return this;
    }

    public Result<TSuccessResponse, TFailureResponse> OnFailure(Action<TFailureResponse> actionOnFailure)
    {
        if (!IsSuccess(out _, out var failureResponse))
        {
            actionOnFailure(failureResponse);
        }

        return this;
    }

    public static Result<TSuccessResponse, TFailureResponse> CreateSuccessResponse(TSuccessResponse successResponse)
    {
        if(successResponse == null) throw new ArgumentNullException(nameof(successResponse));

        return new Result<TSuccessResponse, TFailureResponse>(true, successResponse, default);
    }

    public static Result<TSuccessResponse, TFailureResponse> CreateFailureResponse(TFailureResponse failureResponse)
    {
        if (failureResponse == null) throw new ArgumentNullException(nameof(failureResponse));

        return new Result<TSuccessResponse, TFailureResponse>(false, default, failureResponse);
    }

    public static implicit operator Result<TSuccessResponse, TFailureResponse> (TSuccessResponse successResponse)
        => CreateSuccessResponse(successResponse);

    public static implicit operator Result<TSuccessResponse, TFailureResponse>(TFailureResponse failureResponse)
        => CreateFailureResponse(failureResponse);
}
