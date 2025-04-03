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
    [InlineData(ReasonType.Duplicate)]
    [InlineData(ReasonType.Inconsistent)]
    public void ReasonTypeShouldMapTypeProperly(ReasonType typeInput)
    {
        const string reasonMessage = "Some error";

        var (message, type, _) = typeInput switch 
        {            
            ReasonType.NotFound => Reason.NotFound(reasonMessage),
            ReasonType.Unavailable => Reason.Unavailable(reasonMessage),
            ReasonType.NoContent => Reason.NoContent(reasonMessage),
            ReasonType.Forbidden => Reason.Forbidden(reasonMessage),
            ReasonType.Unauthorized => Reason.Unauthorized(reasonMessage),
            ReasonType.Invalid => Reason.Invalid(reasonMessage),
            ReasonType.InvalidArgument => Reason.InvalidArgument(reasonMessage),
            ReasonType.Conflict => Reason.Conflict(reasonMessage),
            ReasonType.Duplicate => Reason.Duplicate(reasonMessage),
            ReasonType.Inconsistent => Reason.Inconsistent(reasonMessage),
            _ => Reason.Error(reasonMessage)
        };
                
        Assert.Equal(reasonMessage, message);
        Assert.Equal(typeInput, type);
    }

    [Fact]
    public void ReasonTypeShouldBeError()
    {
        const string reasonMessage = "Some error";
        var (message, type, _) = Reason.Error(new Exception(reasonMessage));
            
        Assert.Equal(reasonMessage, message);
        Assert.Equal(ReasonType.Error, type);
    }
}