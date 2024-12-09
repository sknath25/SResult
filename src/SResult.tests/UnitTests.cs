using NuGet.Frameworks;

namespace SResult.Tests;

public class UnitTests
{
    [Fact]
    public void OnSuccessResultCheck()
    {
        const int expected = 1;
        var result = Result<int, string>.CreateSuccessResult(expected);
        result
            .OnSuccess((actual) => { Assert.Equal(expected, actual); })
            .OnFailure(() => { Assert.Fail(); });

    }

    [Fact]
    public void OnFailureReasonCheck()
    {
        const string expected = "Worthless";
        var result = Result<int, string>.CreateFailureResult(expected);
        result
            .OnSuccess(() => { Assert.Fail(); })
            .OnFailure((actual) => { Assert.Equal(expected, actual); });
    }

    [Fact]
    public void OnSuccessAndOnFailureBothCannotBeInvokedForFailure()
    {
        const string expected = "Worthless";
        var result = Result<int, string>.CreateFailureResult(expected);
        int methodInvocationCounter = 0;

        result
            .OnSuccess(() => methodInvocationCounter++)
            .OnFailure(() => methodInvocationCounter++);

        Assert.Equal(1, methodInvocationCounter);
    }

    [Fact]
    public void OnSuccessAndOnFailureBothCannotBeInvokedForSuccess()
    {
        const int expected = 1;
        var result = Result<int, string>.CreateSuccessResult(expected);
        int methodInvocationCounter = 0;

        result
            .OnSuccess(() => methodInvocationCounter++)
            .OnFailure(() => methodInvocationCounter++);

        Assert.Equal(1, methodInvocationCounter);
    }

    [Fact]
    public void FailureReasonWillBeNullForSuccess()
    {
        const int expected = 1;
        var result = Result<int, string>.CreateSuccessResult(expected);
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
        const string expected = "Worthless";
        var result = Result<int, string>.CreateFailureResult(expected);
        if (result.IsSuccess(out var validResult, out var failureReason))
        {
            Assert.Fail();
        }
        else
        {
            Assert.Equal(default, validResult);
            Assert.Equal(expected, failureReason);
        }
    }

    [Fact]
    public void SuccessHasToHaveAResult()
    {
        try
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var result = Result<string, string>.CreateSuccessResult(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.Fail();
        }
        catch (ArgumentNullException)
        {
            Assert.True(true);
        }
    }

    [Fact]
    public void FailureHasToHaveAReason()
    {
        try
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var result = Result<string, string>.CreateFailureResult(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.Fail();
        }
        catch (ArgumentNullException)
        {
            Assert.True(true);
        }
    }
}