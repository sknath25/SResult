namespace SResult.Tests;

public class ResultOfReasonUnitTests
{
    [Fact]
    public void ResultOfReasonShouldFailTest()
    {
        var result = Result.Fail<string>("Something is wrong");

        if (result.IsFail(out var reason))
        {
            Assert.Equal("Something is wrong", (Reason)reason);
        }
    }

    [Fact]
    public void ResultOfReasonShouldFailTest2()
    {
        var result = Result.Fail<string>("Something is wrong");

        if (result.IsFail(out var reason))
        {
            Assert.Equal("Something is wrong", (Reason)reason);
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
        var result = PassResultThroughMethod(70);

        if (result.IsSuccess(out var value))
        {
            Assert.Equal(70, value);
        }
    }

    [Fact]
    public void ResultOfReasonThroughMethodWithStringToReasonCastingShouldFail()
    {
        var result = PassResultThroughMethod(25);

        if (result.IsFail(out var reason))
        {
            Assert.Equal("Sumit is a hopeless", reason.Message);
        }
    }

    public static Result2<string> DidRajeshPass(bool isPass)
    {
        return isPass ? "Rajesh completed assignment" : Reason.Error("Rajesh is a scoundrel");
    }

    public static Result2<int> PassResultThroughMethod(int marks)
    {
        return marks > 50 ? marks : Reason.Error("Sumit is a hopeless");
    }
}
