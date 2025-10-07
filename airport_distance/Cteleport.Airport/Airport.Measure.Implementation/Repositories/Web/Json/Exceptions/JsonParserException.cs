namespace Airport.Measure.Implementation.Repositories.Web.Json.Exceptions;

/// <summary>
/// Base exception for JSON parsing errors.
/// </summary>
public class JsonParserException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonParserException"/> class.
    /// </summary>
    public JsonParserException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonParserException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public JsonParserException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonParserException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public JsonParserException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}