namespace SResult;

public enum ReasonType
{
    Error,
    NotFound,
    Unavailable,
    NoContent,
    Forbidden,
    Unauthorized,
    Invalid,
    InvalidArgument,    
    Conflict    
}

public record Reason(string Message, ReasonType Type = ReasonType.Error)
{
    public static Reason Error(string Message) => new(Message);
    public static Reason Error(Exception ex) => new(ex.Message);
    public static Reason NotFound(string Message) => new(Message, ReasonType.NotFound);
    public static Reason InvalidArgument(string Message) => new(Message, ReasonType.InvalidArgument);
    public static Reason Forbidden(string Message) => new(Message, ReasonType.Forbidden);
    public static Reason Unauthorized(string Message) => new(Message, ReasonType.Unauthorized);
    public static Reason Invalid(string Message) => new(Message, ReasonType.Invalid);
    public static Reason NoContent(string Message) => new(Message, ReasonType.NoContent);
    public static Reason Conflict(string Message) => new(Message, ReasonType.Conflict);
    public static Reason Unavailable(string Message) => new(Message, ReasonType.Unavailable);
    
    public static implicit operator Reason(string Message) => Error(Message);
}
