namespace SResult.Tests;

public class ResultOfReasonUnitTests
{
    [Fact]
    public void ResultOfReasonShouldFailTest()
    {
        var result = Result.Fail<string>("Something is wrong");

        if (result.IsFail(out var reason))
        {
            Assert.Equal("Something is wrong", reason);
        }
    }

    [Fact]
    public void ResultOfReasonShouldFailTest2()
    {
        var result = Result.Fail("Something is wrong");

        if (result.IsFail(out var reason))
        {
            Assert.Equal("Something is wrong", reason);
        }
    }

    [Fact]
    public void ResultOfReasonShouldSuccessTest()
    {
        var result = Result.Success("Something is good");

        if (result.IsSuccess(out var value))
        {
            Assert.Equal("Something is good", value);
        }
    }

    [Fact]
    public void ResultOfReasonThroughMethodShouldSuccess()
    {
        var result = DidRajeshPass(true);

        if (result.IsSuccess(out var value))
        {
            Assert.Equal("Rajesh completed assignment", value);
        }
    }

    [Fact]
    public void ResultOfReasonThroughMethodShouldFail()
    {
        var result = DidRajeshPass(false);

        if (result.IsFail(out var reason))
        {
            Assert.Equal("Rajesh is a scoundrel", reason.Message);
        }
    }

    [Fact]
    public void ResultOfReasonThroughMethodWithStringToReasonCastingShouldSuccess()
    {
        var result = DidSumitPass(70);

        if (result.IsSuccess(out var value))
        {
            Assert.Equal(70, value);
        }
    }

    [Fact]
    public void ResultOfReasonThroughMethodWithStringToReasonCastingShouldFail()
    {
        var result = DidSumitPass(25);

        if (result.IsFail(out var reason))
        {
            Assert.Equal("Sumit is a hopeless", reason.Message);
        }
    }

    public static Result<string> DidRajeshPass(bool isPass)
    {
        return isPass ? "Rajesh completed assignment" : Reason.Error("Rajesh is a scoundrel");
    }

    public static Result<int> DidSumitPass(int marks)
    {
        return marks > 50 ? marks : Reason.Error("Sumit is a hopeless");
    }

    private static Result<int, string> DummyHttpGet(string url)
    {
        if (string.IsNullOrEmpty(url))
            return Result.Fail<int, string>("Url cannot be blank");

        return Result.Success<int, string>(200);
    }
}
