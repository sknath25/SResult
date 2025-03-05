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
    Conflict,
    Duplicate,
    Inconsistent
}

public record Reason(string Message, ReasonType Type = ReasonType.Error, params object[] Values)
{
    public static Reason Error(string Message, params object[] values) => new(Message, ReasonType.Error, values);
    public static Reason Error(Exception ex) => new(ex.Message);
    public static Reason NotFound(string Message, params object[] values) => new(Message, ReasonType.NotFound, values);
    public static Reason InvalidArgument(string Message, params object[] values) => new(Message, ReasonType.InvalidArgument, values);
    public static Reason Forbidden(string Message, params object[] values) => new(Message, ReasonType.Forbidden, values);
    public static Reason Unauthorized(string Message, params object[] values) => new(Message, ReasonType.Unauthorized, values);
    public static Reason Invalid(string Message, params object[] values) => new(Message, ReasonType.Invalid, values);
    public static Reason NoContent(string Message, params object[] values) => new(Message, ReasonType.NoContent, values);
    public static Reason Conflict(string Message, params object[] values) => new(Message, ReasonType.Conflict, values);
    public static Reason Unavailable(string Message, params object[] values) => new(Message, ReasonType.Unavailable, values);
    public static Reason Duplicate(string Message, params object[] values) => new(Message, ReasonType.Duplicate, values);
    public static Reason Inconsistent(string Message, params object[] values) => new(Message, ReasonType.Inconsistent, values);

    public static implicit operator Reason(string Message) => Error(Message);
}
