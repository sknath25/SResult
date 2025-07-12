namespace SResult.Tests;

public class ResultUnitTests2
{
    [Fact]
    public void MakeSimpleFailureTest()
    {
        var result = Result.Fail<int>("Something is wrong");
        if (!result.IsSuccess)
        {
            Assert.Equal("Something is wrong", result.Failure.Message);
        }
    }

    [Fact]
    public void MakeSimpleSuccessTest()
    {
        var result = Result.Success("Something is good");
        if (result.IsSuccess)
        {
            Assert.Equal("Something is good", result.Value);
        }
    }

    [Fact]
    public void MakeSimpleSuccessFromValueTest()
    {
        var result = Result.Success(1);
        if (result.IsSuccess)
        {
            Assert.Equal(1, result.Value);
            return;
        }

        Assert.Fail();
    }

    [Fact]
    public void MakeFailureAsDefaultTest()
    {
        var failure = Failure.Unauthorized("Something is wrong");
        Assert.Equal("Something is wrong", failure.Message);
        Assert.True(failure.Level.IsWarning());
    }

    [Fact]
    public void MakeFailureAsWarningTest()
    {
        var failure = Failure.Unauthorized("Something is wrong").AsWarning();
        Assert.Equal("Something is wrong", failure.Message);
        Assert.True(failure.Level.IsWarning());
    }

    [Fact]
    public void MakeFailureAsFatalTest()
    {
        var failure = Failure.Unauthorized("Something is wrong").AsFatal();
        Assert.Equal("Something is wrong", failure.Message);
        Assert.True(failure.Level.IsFatal());
    }

    [Fact]
    public void OnSuccessTest()
    {
        var result = Result.Success(11);
        result
            .OnSuccess(() => { Assert.True(true); })
            .OnFail(() => { Assert.Fail(); });
    }

    [Fact]
    public void OnSuccessTestWithValue()
    {
        var result = Result.Success(11);
        result
            .OnSuccess((actual) => { Assert.Equal(11, actual); })
            .OnFail(() => { Assert.Fail(); });
    }

    [Fact]
    public void OnSuccessShouldThrowExceptionForNullActionTest()
    {
        try
        {
#pragma warning disable CS8600, CS8604
            Action action = null;
            Result.Success(11).OnSuccess(action);
#pragma warning restore CS8600, CS8604
            Assert.Fail();
        }
        catch (ArgumentNullException)
        {
            Assert.True(true);
        }
    }

    [Fact]
    public void OnSuccessShouldThrowExceptionForNullGenericActionTest()
    {
        try
        {
#pragma warning disable CS8600, CS8604
            Action<int> action = null;
            Result.Success(11).OnSuccess(action);
#pragma warning restore CS8600, CS8604
            Assert.Fail();
        }
        catch (ArgumentNullException)
        {
            Assert.True(true);
        }
    }

    [Fact]
    public void OnFailureShouldThrowExceptionForNullActionTest()
    {
        try
        {
#pragma warning disable CS8600, CS8604
            Action action = null;
            Result.Fail<int>("Massive problem here!").OnFail(action);
#pragma warning restore CS8600, CS8604
            Assert.Fail();
        }
        catch (ArgumentNullException)
        {
            Assert.True(true);
        }
    }

    [Fact]
    public void OnFailureShouldThrowExceptionForNullGenericActionTest()
    {
        try
        {
#pragma warning disable CS8600, CS8604
            Action<IFailure> action = null;
            Result.Fail<int>("Massive problem here!").OnFail(action);
#pragma warning restore CS8600, CS8604
            Assert.Fail();
        }
        catch (ArgumentNullException)
        {
            Assert.True(true);
        }
    }

    [Fact]
    public void OnFailureTest()
    {
        var result = Result.Fail<int>("Something is wrong!");
        result
            .OnSuccess(() => { Assert.Fail(); })
            .OnFail(() => { Assert.True(true); });
    }

    [Fact]
    public void OnFailureTestWithFailure()
    {
        var result = Result.Fail<int>("Something is wrong!");
        result
            .OnSuccess(() => { Assert.Fail(); })
            .OnFail((failure) => { Assert.Equal("Something is wrong!", failure.Message); });
    }

    [Fact]
    public void NullFailureForSuccessTest()
    {
        var result = Result.Success(21);
        if (result.IsSuccess)
        {
            Assert.Null(result.Failure);
            Assert.Equal(21, result.Value);
        }
        else
        {
            Assert.Fail();
        }
    }

    [Fact]
    public void NullValueForFailureTest()
    {
        var result = Result.Fail<int>("Something is wrong!");
        if (result.IsSuccess)
        {
            Assert.Fail();
        }
        else
        {
            Assert.Equal(default, result.Value);
            Assert.Equal("Something is wrong!", result.Failure.Message);
        }
    }

    [Fact]
    public void SuccessValueWillBeNullForFail()
    {
        var result = Result.Fail<string>("Something is wrong!");
        if (result.IsFailed)
        {
            Assert.Null(result.Value);
            Assert.Equal("Something is wrong!", result.Failure.Message);
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

    [Fact]
    public void TestParseIntegerSuccess()
    {
        var r = TryParseToInteger("101");
        Assert.True(r.IsSuccess);
        Assert.Equal(101, r.Value);
    }

    [Fact]
    public void TestParseIntegerFailure()
    {
        var r = TryParseToInteger("Suman");
        Assert.False(r.IsSuccess);
        Assert.Equal("This is bullshit. Not any number!", r.Failure.Message);
    }

    public static Result<int> TryParseToInteger(string value)
    {
        if (int.TryParse(value, out var val))
        {
            return val;
        }
        else
        {
            return Result.Fail<int>("This is bullshit. Not any number!");
        }
    }
}
