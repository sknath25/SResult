namespace SResult;

public interface IFailure
{
    string Message { get; }
    FailureLevel Level { get; }
}

public enum FailureType
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

public enum FailureLevel
{
    /// <summary>
    /// Fatal means the failure is catastrophic and an abort is advised.
    /// </summary>
    Fatal,
    /// <summary>
    /// Warning means not significant enough to abort but proceed with caution.
    /// </summary>
    Warning,
}

public record Failure(string Message, FailureType Type = FailureType.Error, params object[] Values) : IFailure
{
    public FailureLevel Level { get; private set; } = FailureLevel.Warning;

    public Failure AsFatal()
    {
        Level = FailureLevel.Fatal;
        return this;
    }

    public Failure AsWarning()
    {
        Level = FailureLevel.Warning;
        return this;
    }

    public static Failure Error(string Message, params object[] values) => new(Message, FailureType.Error, values);
    public static Failure Error(Exception ex) => new(ex.Message);
    public static Failure NotFound(string Message, params object[] values) => new(Message, FailureType.NotFound, values);
    public static Failure InvalidArgument(string Message, params object[] values) => new(Message, FailureType.InvalidArgument, values);
    public static Failure Forbidden(string Message, params object[] values) => new(Message, FailureType.Forbidden, values);
    public static Failure Unauthorized(string Message, params object[] values) => new(Message, FailureType.Unauthorized, values);
    public static Failure Invalid(string Message, params object[] values) => new(Message, FailureType.Invalid, values);
    public static Failure NoContent(string Message, params object[] values) => new(Message, FailureType.NoContent, values);
    public static Failure Conflict(string Message, params object[] values) => new(Message, FailureType.Conflict, values);
    public static Failure Unavailable(string Message, params object[] values) => new(Message, FailureType.Unavailable, values);
    public static Failure Duplicate(string Message, params object[] values) => new(Message, FailureType.Duplicate, values);
    public static Failure Inconsistent(string Message, params object[] values) => new(Message, FailureType.Inconsistent, values);
    //public static implicit operator Failure(string Message) => Error(Message);
    //public static implicit operator string(Failure failure) => failure.Message;
}

public static class ReasonLevelExtensions
{
    public static bool IsFatal(this FailureLevel source) => source == FailureLevel.Fatal;
    public static bool IsWarning(this FailureLevel source) => source == FailureLevel.Warning;
}
