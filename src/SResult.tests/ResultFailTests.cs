namespace SResult.Tests;

public class Result2FailTests
{
    [Fact]
    public void FailCheck()
    {
        var result = Result.Fail<int>("");
        Assert.True(!result.IsSuccess());
    }

    [Fact]
    public void OnFailureReasonCheck()
    {
        const string expected = "Worthless";
        var result = Result.Fail<string>(expected);
        result
            .OnSuccess(() => { Assert.Fail(); })
            .OnFail((actual) => { Assert.Equal(expected, actual.Message); });
    }

    [Fact]
    public void OnSuccessAndOnFailureBothCannotBeInvokedForFailure()
    {
        const string expected = "Worthless";
        var result = Result.Fail<string>(expected);
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
        var result = Result.Fail<string>(expected);
        if (result.IsSuccess(out var value, out var failure))
        {
            Assert.Fail();
        }
        else
        {
            Assert.Equal(default, value);
            Assert.Equal(expected, failure.Message);
        }
    }

    [Fact]
    public void SuccessValueWillBeNullForFail()
    {
        const string expected = "Wrong";
        var result = Result.Fail<string>(expected);
        if (result.IsFail(out var value, out var reason))
        {
            Assert.Null(value);
            Assert.Equal(expected, reason.Message);
        }
        else
        {
            Assert.Fail();
        }
    }

    [Fact]
    public void FailureHasToHaveAReasonAndCannotBeNull()
    {
        try
        {
#pragma warning disable CS8625 // To test, passing null forcefully make an exception.
            var result = Result.Fail<string>(null);
#pragma warning restore CS8625 
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
            .OnFail((actualFailure) => { Assert.Equal(expectedFailure, actualFailure.Message); });
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

    private static Result2<int> CallApi(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return (Reason)"Url cannot be blank";
        }

        return 200;
    }

    [Fact]
    public void OnSuccessActionShouldNotBeNull()
    {
        try
        {
            var result = Result.Success(false);
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
            var result = Result.Success(false);
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
            var result = Result.Fail<bool>("");
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
            var result = Result.Fail<bool>("");
            Action<IReason>? action = null;
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
