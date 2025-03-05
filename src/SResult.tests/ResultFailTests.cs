using System.Diagnostics;

namespace SResult.Tests;

public class ResultFailTests
{
   

    [Fact]
    public void FailCheck()
    {
        var result = Result.Fail<int, int>(0);
        Assert.True(!result.IsSuccess());
    }

    [Fact]
    public void OnFailureReasonCheck()
    {
        const string expected = "Worthless";
        var result = Result.Fail<string, string>(expected);
        result
            .OnSuccess(() => { Assert.Fail(); })
            .OnFail((actual) => { Assert.Equal(expected, actual); });
    }

    [Fact]
    public void OnSuccessAndOnFailureBothCannotBeInvokedForFailure()
    {
        const string expected = "Worthless";
        var result = Result.Fail<string, string>(expected);
        int methodInvocationCounter = 0;

        result
            .OnSuccess(() => methodInvocationCounter++)
            .OnFail(() => methodInvocationCounter++);

        Assert.Equal(1, methodInvocationCounter);
    }

    [Fact]
    public void SuccessResultWillBeNullForFailure()
    {
        const string expected = "Worthless";
        var result = Result.Fail<string, string>(expected);
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
    public void SuccessValueWillBeNullForFail()
    {
        const string expected = "Wrong";
        var result = Result.Fail<string, string>(expected);
        if (result.IsFail(out var value, out var reason))
        {
            Assert.Null(value);
            Assert.Equal(expected, reason);
        }
        else
        {
            Assert.Fail();
        }
    }

    [Fact]
    public void FailureHasToHaveAReason()
    {
        try
        {
            var result = Result.Fail<string, string>(null);
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
            .OnFail((actualFailure) => { Assert.Equal(expectedFailure, actualFailure); });
    }

    [Fact]
    public void AutomaticMappingToSuccessBasedOnReturnType()
    {
        const int expectedHttpCode = 200;
        var result = CallApi("http://somedomain.com");
        result
            .OnSuccess((actualHttpCode) => { Assert.Equal(expectedHttpCode, actualHttpCode); })
            .OnFail(() => { Assert.Fail(); });
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
            .OnFail(() => Assert.Fail());
    }

    [Fact]
    public void MapBooleanToObjectAsFailureTest()
    {
        var result = MapBooleanToObjectAsFailure();
        result
            .OnSuccess(() => Assert.Fail())
            .OnFail((f) => Assert.Equal(false, f));
    }

    private static Result<object, string> MapBooleanToObjectAsSuccess()
    {
        return false;
    }

    private static Result<string, object> MapBooleanToObjectAsFailure()
    {
        return false;
    }

    [Fact]
    public void OnSuccessActionShouldNotBeNull()
    {
        try
        {
            var result = Result.Success<object, bool>(false);
            Action? action = null;
#pragma warning disable CS8604 // Possible null reference argument.
            _ = result.OnSuccess(action);
#pragma warning restore CS8604 // Possible null reference argument.
            Assert.Fail();
        }
        catch (ArgumentNullException)
        {
            Assert.True(true);
        }
    }

    [Fact]
    public void OnSuccessParamActionShouldNotBeNull()
    {
        try
        {
            var result = Result.Success<bool, bool>(false);
            Action<bool>? action = null;
#pragma warning disable CS8604 // Possible null reference argument.
            _ = result.OnSuccess(action);
#pragma warning restore CS8604 // Possible null reference argument.
            Assert.Fail();
        }
        catch (ArgumentNullException)
        {
            Assert.True(true);
        }
    }

    [Fact]
    public void OnFailureActionShouldNotBeNull()
    {
        try
        {
            var result = Result.Fail<object, bool>(false);
            Action? action = null;
#pragma warning disable CS8604 // Possible null reference argument.
            _ = result.OnFail(action);
#pragma warning restore CS8604 // Possible null reference argument.
            Assert.Fail();
        }
        catch (ArgumentNullException)
        {
            Assert.True(true);
        }
    }

    [Fact]
    public void OnFailureParamActionShouldNotBeNull()
    {
        try
        {
            var result = Result.Fail<object, bool>(false);
            Action<bool>? action = null;
#pragma warning disable CS8604 // Possible null reference argument.
            _ = result.OnFail(action);
#pragma warning restore CS8604 // Possible null reference argument.
            Assert.Fail();
        }
        catch (ArgumentNullException)
        {
            Assert.True(true);
        }
    }
}
