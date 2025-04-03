using System.Diagnostics;

namespace SResult.Tests;

public class ResultUnitTests2
{

    [Fact]
    public void SuccessWithParameterCheck()
    {
        var result = Result.Success(1);
        Assert.True(result.IsSuccess());
    }

    [Fact]
    public void SuccessWithParameterCheck2()
    {
        var result = Result.Success(1);
        if (result.IsSuccess(out var value))
        {
            Assert.Equal(1, value);
            return;
        }

        Assert.Fail();
    }

    [Fact]
    public void SuccessWithStringParameterCheck2()
    {
        var result = Result.Success("Suman");
        if (result.IsSuccess(out var value))
        {
            Assert.Equal("Suman", value);
            return;
        }

        Assert.Fail();
    }

    [Fact]
    public void SuccessWithTypeCheck()
    {
        var result = Result.Success(1);
        Assert.True(result.IsSuccess());
    }

    [Fact]
    public void SuccessWithTypeCheck4()
    {
        var result = Result.Success(1);
        if (result.IsSuccess(out var value))
        {
            Assert.Equal(1, value);
            return;
        }

        Assert.Fail();
    }

    [Fact]
    public void SuccessWithTypeCheck2()
    {
        var result = Result.Success(1);
        Assert.True(result.IsSuccess());
    }

    [Fact]
    public void FailCheck()
    {
        var result = Result.Fail<int>((Reason)"");
        Assert.True(!result.IsSuccess());
    }

    [Fact]
    public void OnSuccessResultCheck()
    {
        const int expected = 1;
        var result = Result.Success(expected);
        result
            .OnSuccess((actual) => { Assert.Equal(expected, actual); })
            .OnFail(() => { Assert.Fail(); });
    }

    [Fact]
    public void OnFailureReasonCheck()
    {
        Reason reason = "Worthless";
        var result = Result.Fail<string>(reason);
        result
            .OnSuccess(() => { Assert.Fail(); })
            .OnFail((fail) => { Assert.Equal(reason, fail); });
    }

    [Fact]
    public void OnSuccessAndOnFailureBothCannotBeInvokedForFailure()
    {
        Reason reason = "Worthless";
        var result = Result.Fail<string>(reason);
        int methodInvocationCounter = 0;

        result
            .OnSuccess(() => methodInvocationCounter++)
            .OnFail(() => methodInvocationCounter++);

        Assert.Equal(1, methodInvocationCounter);
    }

    [Fact]
    public void OnSuccessAndOnFailureBothCannotBeInvokedForSuccess()
    {
        const int expected = 1;
        var result = Result.Success(expected);
        int methodInvocationCounter = 0;

        result
            .OnSuccess(() => methodInvocationCounter++)
            .OnFail(() => methodInvocationCounter++);

        Assert.Equal(1, methodInvocationCounter);
    }

    [Fact]
    public void FailureReasonWillBeNullForSuccess()
    {
        const int expected = 1;
        var result = Result.Success(expected);
        if (result.IsSuccess(out var validResult, out var failureReason))
        {
            Assert.Null(failureReason);
            Assert.Equal(expected, validResult);
        }
        else
        {
            Assert.Fail();
        }
    }

    [Fact]
    public void SuccessResultWillBeNullForFailure()
    {
        Reason reason = "Worthless";
        var result = Result.Fail<string>(reason);
        if (result.IsSuccess(out var validResult, out var failureReason))
        {
            Assert.Fail();
        }
        else
        {
            Assert.Equal(default, validResult);
            Assert.Equal(reason, failureReason);
        }
    }

    [Fact]
    public void SuccessValueWillBeNullForFail()
    {
        Reason reason = "Worthless";
        var result = Result.Fail<string>(reason);
        if (result.IsFail(out var value, out var failReason))
        {
            Assert.Null(value);
            Assert.Equal(reason, failReason);
        }
        else
        {
            Assert.Fail();
        }
    }

    [Fact]
    public void SuccessHasToHaveAValueAndCannotBeNull()
    {
        try
        {
#pragma warning disable CS8625 // To test, passing null forcefully make an exception.
            var result = Result.Success<string>(null);
#pragma warning restore CS8625 
            Assert.Fail();
        }
        catch (ArgumentNullException)
        {
            Assert.True(true);
        }
    }    
}
