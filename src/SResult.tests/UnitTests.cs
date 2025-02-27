namespace SResult.Tests;

public class UnitTests
{
    [Fact]
    public void SuccessCheck()
    {
        var result = Result.Good(1);
        Assert.True(result.IsGood());
    }

    [Fact]
    public void FailCheck()
    {
        var result = Result.Bad(0);
        Assert.True(!result.IsGood());
    }

    [Fact]
    public void OnSuccessResultCheck()
    {
        const int expected = 1;
        var result = Result.Good(expected);
        result
            .WhenGood((actual) => { Assert.Equal(expected, actual); })
            .WhenBad(() => { Assert.Fail(); });
    }

    [Fact]
    public void OnFailureReasonCheck()
    {
        const string expected = "Worthless";
        var result = Result.Bad(expected);
        result
            .WhenGood(() => { Assert.Fail(); })
            .WhenBad((actual) => { Assert.Equal(expected, actual); });
    }

    [Fact]
    public void OnSuccessAndOnFailureBothCannotBeInvokedForFailure()
    {
        const string expected = "Worthless";
        var result = Result.Bad(expected);
        int methodInvocationCounter = 0;

        result
            .WhenGood(() => methodInvocationCounter++)
            .WhenBad(() => methodInvocationCounter++);

        Assert.Equal(1, methodInvocationCounter);
    }

    [Fact]
    public void OnSuccessAndOnFailureBothCannotBeInvokedForSuccess()
    {
        const int expected = 1;
        var result = Result.Good<int, string>(expected);
        int methodInvocationCounter = 0;

        result
            .WhenGood(() => methodInvocationCounter++)
            .WhenBad(() => methodInvocationCounter++);

        Assert.Equal(1, methodInvocationCounter);
    }

    [Fact]
    public void FailureReasonWillBeNullForSuccess()
    {
        const int expected = 1;
        var result = Result.Good(expected);
        if (result.IsGood(out var validResult, out var failureReason))
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
        var result = Result.Bad(expected);
        if (result.IsGood(out var validResult, out var failureReason))
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
            var result = Result.Good<string, string>(null);
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
            var result = Result.Bad<string, string>(null);
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
            .WhenGood(() => { Assert.Fail(); })
            .WhenBad((actualFailure) => { Assert.Equal(expectedFailure, actualFailure); });
    }

    [Fact]
    public void AutomaticMappingToSuccessBasedOnReturnType()
    {
        const int expectedHttpCode = 200;
        var result = CallApi("http://somedomain.com");
        result
            .WhenGood((actualHttpCode) => { Assert.Equal(expectedHttpCode, actualHttpCode); })
            .WhenBad(() => { Assert.Fail(); });
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
            .WhenGood((s) => Assert.Equal(false, s))
            .WhenBad(() => Assert.Fail());
    }

    [Fact]
    public void MapBooleanToObjectAsFailureTest()
    {
        var result = MapBooleanToObjectAsFailure();
        result
            .WhenGood(() => Assert.Fail())
            .WhenBad((f) => Assert.Equal(false, f));
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
            var result = Result.Good(false);
            Action? action = null;
#pragma warning disable CS8604 // Possible null reference argument.
            _ = result.WhenGood(action);
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
            var result = Result.Good(false);
            Action<bool>? action = null;
#pragma warning disable CS8604 // Possible null reference argument.
            _ = result.WhenGood(action);
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
            var result = Result.Bad(false);
            Action? action = null;
#pragma warning disable CS8604 // Possible null reference argument.
            _ = result.WhenBad(action);
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
            var result = Result.Bad(false);
            Action<bool>? action = null;
#pragma warning disable CS8604 // Possible null reference argument.
            _ = result.WhenBad(action);
#pragma warning restore CS8604 // Possible null reference argument.
            Assert.Fail();
        }
        catch (ArgumentNullException)
        {
            Assert.True(true);
        }
    }
}