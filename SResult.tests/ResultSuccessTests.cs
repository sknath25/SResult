namespace SResult.Tests;

public class ResultUnitTests2
{
    [Fact]
    public void MakeSimpleFailureTest()
    {
        var result = Result.Fail("Something is wrong");
        if (result.IsFail(out var reason))
        {
            Assert.Equal("Something is wrong", reason.Message);
        }
    }

    [Fact]
    public void MakeSimpleSuccessTest()
    {
        var result = Result.Success("Something is good");
        if (result.IsSuccess(out var value))
        {
            Assert.Equal("Something is good", value);
        }
    }

    [Fact]
    public void MakeSimpleSuccessFromValueTest()
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
    public void MakeSimpleFailureCastedValueTest()
    {
        var failure = (Failure)"Something is wrong";
        Assert.Equal("Something is wrong", failure.Message);
    }

    [Fact]
    public void OnSuccessTest()
    {
        var result = Result.Success(11);
        result
            .OnSuccess((actual) => { Assert.Equal(11, actual); })
            .OnFail(() => { Assert.Fail(); });
    }

    [Fact]
    public void OnFailureTest()
    {
        var result = Result.Fail("Something is wrong!");
        result
            .OnSuccess(() => { Assert.Fail(); })
            .OnFail((failure) => { Assert.Equal("Something is wrong!", failure.Message); });
    }

    [Fact]
    public void NullFailureForSuccessTest()
    {
        var result = Result.Success(21);
        if (result.IsSuccess(out var validResult, out var failureReason))
        {
            Assert.Null(failureReason);
            Assert.Equal(21, validResult);
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
        if (result.IsSuccess(out var validResult, out var failureReason))
        {
            Assert.Fail();
        }
        else
        {
            Assert.Equal(default, validResult);
            Assert.Equal("Something is wrong!", failureReason.Message);
        }
    }

    [Fact]
    public void SuccessValueWillBeNullForFail()
    {
        var result = Result.Fail("Something is wrong!");
        if (result.IsFail(out var value, out var failReason))
        {
            Assert.Null(value);
            Assert.Equal("Something is wrong!", failReason.Message);
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
        Assert.True(r.IsSuccess(out var value));
        Assert.Equal(101, value);
    }

    [Fact]
    public void TestParseIntegerFailure()
    {
        var r = TryParseToInteger("Suman");
        Assert.True(r.IsFail(out var failure));
        Assert.Equal("This is bullshit. Not any number!", failure.Message);
    }

    public static Result<int> TryParseToInteger(string value)
    {
        if (int.TryParse(value, out var val))
        {
            return val;
        }
        else
        {
            return "This is bullshit. Not any number!";
        }
    }
}
