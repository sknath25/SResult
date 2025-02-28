namespace SResult.Tests;

public class ReasonUnitTests
{
    [Theory]
    [InlineData(ReasonType.Error)]
    [InlineData(ReasonType.NotFound)]
    [InlineData(ReasonType.Unavailable)]
    [InlineData(ReasonType.NoContent)]
    [InlineData(ReasonType.Forbidden)]
    [InlineData(ReasonType.Unauthorized)]
    [InlineData(ReasonType.Invalid)]
    [InlineData(ReasonType.InvalidArgument)]
    [InlineData(ReasonType.Conflict)]
    public void ReasonTypeShouldMapTypeProperly(ReasonType typeInput)
    {

        var (message, type) = typeInput switch 
        {            
            ReasonType.NotFound => Reason.NotFound("Some error"),
            ReasonType.Unavailable => Reason.Unavailable("Some error"),
            ReasonType.NoContent => Reason.NoContent("Some error"),
            ReasonType.Forbidden => Reason.Forbidden("Some error"),
            ReasonType.Unauthorized => Reason.Unauthorized("Some error"),
            ReasonType.Invalid => Reason.Invalid("Some error"),
            ReasonType.InvalidArgument => Reason.InvalidArgument("Some error"),
            ReasonType.Conflict => Reason.Conflict("Some error"),
            _ => Reason.Error("Some error")
        };
                
        Assert.Equal("Some error", message);
        Assert.Equal(typeInput, type);
    }

    [Fact]
    public void ReasonTypeShouldBeError()
    {

        var (message, type) = Reason.Error(new Exception("Some error"));
            
        Assert.Equal("Some error", message);
        Assert.Equal(ReasonType.Error, type);
    }
}