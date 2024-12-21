namespace SResult.Tests;

public class UnitTests
{
    [Fact]
    public void SuccessCheck()
    {
        var result = Result.CreateSuccessResult(1);
        Assert.True(result.IsSuccess());
    }

    [Fact]
    public void FailCheck()
    {
        var result = Result.CreateFailureReason(0);
        Assert.True(!result.IsSuccess());
    }

    [Fact]
    public void OnSuccessResultCheck()
    {
        const int expected = 1;
        var result = Result.CreateSuccessResult(expected);
        result
            .OnSuccess((actual) => { Assert.Equal(expected, actual); })
            .OnFailure(() => { Assert.Fail(); });
    }

    [Fact]
    public void OnFailureReasonCheck()
    {
        const string expected = "Worthless";
        var result = Result.CreateFailureReason(expected);
        result
            .OnSuccess(() => { Assert.Fail(); })
            .OnFailure((actual) => { Assert.Equal(expected, actual); });
    }

    [Fact]
    public void OnSuccessAndOnFailureBothCannotBeInvokedForFailure()
    {
        const string expected = "Worthless";
        var result = Result.CreateFailureReason(expected);
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
        var result = Result.CreateSuccessResult<int, string>(expected);
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
        var result = Result.CreateSuccessResult(expected);
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
        var result = Result.CreateFailureReason(expected);
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
            var result = Result.CreateSuccessResult<string, string>(null);
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
            var result = Result.CreateFailureReason<string, string>(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.Fail();
        }
        catch (ArgumentNullException)
        {
            Assert.True(true);
        }
    }

    [Fact]
    public void AutomaticMappingToFailureBasedOnReturnType()
    {
        const string expectedFailure = "Url cannot be blank";
        var result = CallApi(string.Empty);
        result
            .OnSuccess(() => { Assert.Fail(); })
            .OnFailure((actualFailure) => { Assert.Equal(expectedFailure, actualFailure); });
    }

    [Fact]
    public void AutomaticMappingToSuccessBasedOnReturnType()
    {
        const int expectedHttpCode = 200;
        var result = CallApi("http://somedomain.com");
        result
            .OnSuccess((actualHttpCode) => { Assert.Equal(expectedHttpCode, actualHttpCode); })
            .OnFailure(() => { Assert.Fail(); });
    }

    private static Result<int, string> CallApi(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return "Url cannot be blank";
        }

        return 200;
    }

    [Fact]
    public void MapBooleanToObjectAsSuccessTest()
    {
        var result = MapBooleanToObjectAsSuccess();
        result
            .OnSuccess((s) => Assert.Equal(false, s))
            .OnFailure(() => Assert.Fail());
    }

    [Fact]
    public void MapBooleanToObjectAsFailureTest()
    {
        var result = MapBooleanToObjectAsFailure();
        result
            .OnSuccess(() => Assert.Fail())
            .OnFailure((f) => Assert.Equal(false, f));
    }

    private static Result<object, string> MapBooleanToObjectAsSuccess()
    {
        return false;
    }

    private static Result<string, object> MapBooleanToObjectAsFailure()
    {
        return false;
    }
}