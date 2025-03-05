using System.Diagnostics;

namespace SResult.Tests;

public class ResultUnitTests
{
    [Fact]
    public void SuccessWithNoParameterCheck()
    {
        var result = Result.Success();
        Assert.True(result.IsSuccess());
    }

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
        if(result.IsSuccess(out var value))
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
        var result = Result.Success<int>(1);
        Assert.True(result.IsSuccess());
    }

    [Fact]
    public void SuccessWithTypeCheck4()
    {
        var result = Result.Success<int>(1);
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
        var result = Result.Success<int, string>(1);
        Assert.True(result.IsSuccess());
    }

    [Fact]
    public void FailCheck()
    {
        var result = Result.Fail<int, int>(0);
        Assert.True(!result.IsSuccess());
    }

    [Fact]
    public void OnSuccessResultCheck()
    {
        const int expected = 1;
        var result = Result.Success<int, string>(expected);
        result
            .OnSuccess((actual) => { Assert.Equal(expected, actual); })
            .OnFail(() => { Assert.Fail(); });
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
    public void OnSuccessAndOnFailureBothCannotBeInvokedForSuccess()
    {
        const int expected = 1;
        var result = Result.Success<int, string>(expected);
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
        var result = Result.Success<int, string>(expected);
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
    public void SuccessHasToHaveAResult()
    {
        try
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var result = Result.Success<string, string>(null);
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
#pragma warning disable CS8625 // Cannot convert nu/ll literal to non-nullable reference type.
            var result = Result.Fail<string, string>(null);
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
