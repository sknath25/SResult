﻿using System.Diagnostics.CodeAnalysis;

namespace SResult;

public sealed class Result<TSuccessResult, TFailureReason>
{
    private readonly TSuccessResult? _successResult;
    private readonly TFailureReason? _failureReason;
    private readonly bool _isSuccess;

    private Result(bool isSuccess, TSuccessResult? successResult, TFailureReason? failureReason)
    {
        if (isSuccess && successResult == null) throw new ArgumentNullException(nameof(successResult));
        if (!isSuccess && failureReason == null) throw new ArgumentNullException(nameof(failureReason));

        _successResult = successResult;
        _failureReason = failureReason;
        _isSuccess = isSuccess;
    }

    public bool IsSuccess()
    {
        return _isSuccess;
    }

    public bool IsFailed()
    {
        return !IsSuccess();
    }

    public bool IsSuccess([NotNullWhen(true)] out TSuccessResult? successResult)
    {
        successResult = _successResult;
        return IsSuccess();
    }

    public bool IsSuccess([NotNullWhen(true)] out TSuccessResult? successResult, [NotNullWhen(false)] out TFailureReason? failureReason)
    {
        successResult = _successResult;
        failureReason = _failureReason;
        return IsSuccess();
    }

    public bool IsFailed([NotNullWhen(true)] out TFailureReason? failureReason)
    {
        failureReason = _failureReason;
        return IsFailed();
    }

    public Result<TSuccessResult, TFailureReason> OnSuccess(Action actionOnSuccess)
    {
        if (actionOnSuccess == null) throw new ArgumentNullException(nameof(actionOnSuccess));

        if (IsSuccess())
        {
            actionOnSuccess();
        }

        return this;
    }

    public Result<TSuccessResult, TFailureReason> OnSuccess(Action<TSuccessResult> actionOnSuccess)
    {
        if (actionOnSuccess == null) throw new ArgumentNullException(nameof(actionOnSuccess));

        if (IsSuccess(out var successResponse))
        {
            actionOnSuccess(successResponse);
        }

        return this;
    }

    public Result<TSuccessResult, TFailureReason> OnFailure(Action actionOnFailure)
    {
        if (actionOnFailure == null) throw new ArgumentNullException(nameof(actionOnFailure));

        if (IsFailed())
        {
            actionOnFailure();
        }

        return this;
    }

    public Result<TSuccessResult, TFailureReason> OnFailure(Action<TFailureReason> actionOnFailure)
    {
        if (actionOnFailure == null) throw new ArgumentNullException(nameof(actionOnFailure));

        if (IsFailed(out var failureResponse))
        {
            actionOnFailure(failureResponse);
        }

        return this;
    }

    public static implicit operator Result<TSuccessResult, TFailureReason>(TSuccessResult successResult)
        =>  new (true, successResult, default);

    public static implicit operator Result<TSuccessResult, TFailureReason>(TFailureReason failureReason)
        => new (false, default, failureReason);

}
