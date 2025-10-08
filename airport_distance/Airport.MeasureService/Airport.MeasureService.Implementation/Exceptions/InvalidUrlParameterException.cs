namespace Airport.MeasureService.Implementation.Exceptions;

/// <summary>
/// Exception thrown when a URL parameter is invalid or malformed.
/// </summary>
public class InvalidUrlParameterException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidUrlParameterException"/> class.
    /// </summary>
    public InvalidUrlParameterException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidUrlParameterException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidUrlParameterException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidUrlParameterException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public InvalidUrlParameterException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}