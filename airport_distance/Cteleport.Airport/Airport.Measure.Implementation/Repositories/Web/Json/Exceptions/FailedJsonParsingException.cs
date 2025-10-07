namespace Airport.Measure.Implementation.Repositories.Web.Json.Exceptions;

/// <summary>
/// Exception thrown when JSON parsing fails.
/// </summary>
public class FailedJsonParsingException : JsonParserException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FailedJsonParsingException"/> class.
    /// </summary>
    public FailedJsonParsingException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FailedJsonParsingException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public FailedJsonParsingException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FailedJsonParsingException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public FailedJsonParsingException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}