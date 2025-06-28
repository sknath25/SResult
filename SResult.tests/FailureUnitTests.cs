namespace SResult.Tests;

public class FailureUnitTests
{
    [Theory]
    [InlineData(FailureType.Error)]
    [InlineData(FailureType.NotFound)]
    [InlineData(FailureType.Unavailable)]
    [InlineData(FailureType.NoContent)]
    [InlineData(FailureType.Forbidden)]
    [InlineData(FailureType.Unauthorized)]
    [InlineData(FailureType.Invalid)]
    [InlineData(FailureType.InvalidArgument)]
    [InlineData(FailureType.Conflict)]
    [InlineData(FailureType.Duplicate)]
    [InlineData(FailureType.Inconsistent)]
    public void ReasonTypeShouldMapTypeProperly(FailureType typeInput)
    {
        const string reasonMessage = "Some error";

        var (message, type, _) = typeInput switch 
        {            
            FailureType.NotFound => Failure.NotFound(reasonMessage),
            FailureType.Unavailable => Failure.Unavailable(reasonMessage),
            FailureType.NoContent => Failure.NoContent(reasonMessage),
            FailureType.Forbidden => Failure.Forbidden(reasonMessage),
            FailureType.Unauthorized => Failure.Unauthorized(reasonMessage),
            FailureType.Invalid => Failure.Invalid(reasonMessage),
            FailureType.InvalidArgument => Failure.InvalidArgument(reasonMessage),
            FailureType.Conflict => Failure.Conflict(reasonMessage),
            FailureType.Duplicate => Failure.Duplicate(reasonMessage),
            FailureType.Inconsistent => Failure.Inconsistent(reasonMessage),
            _ => Failure.Error(reasonMessage)
        };
                
        Assert.Equal(reasonMessage, message);
        Assert.Equal(typeInput, type);
    }

    [Fact]
    public void ReasonTypeShouldBeError()
    {
        const string reasonMessage = "Some error";
        var (message, type, _) = Failure.Error(new Exception(reasonMessage));
            
        Assert.Equal(reasonMessage, message);
        Assert.Equal(FailureType.Error, type);
    }
}