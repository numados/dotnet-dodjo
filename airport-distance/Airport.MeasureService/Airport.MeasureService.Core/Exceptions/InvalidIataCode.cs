namespace Airport.MeasureService.Core.Exceptions;

/// <summary>
/// Exception thrown when an invalid IATA airport code is encountered.
/// </summary>
/// <remarks>
/// This exception is thrown when a provided IATA code does not meet the required format
/// (three uppercase letters) or is null, empty, or whitespace.
/// </remarks>
public class InvalidIataCode : ArgumentException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidIataCode"/> class.
    /// </summary>
    public InvalidIataCode()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidIataCode"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidIataCode(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidIataCode"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public InvalidIataCode(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidIataCode"/> class with a specified error message
    /// and the name of the parameter that causes this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="paramName">The name of the parameter that caused the current exception.</param>
    public InvalidIataCode(string? message, string? paramName) : base(message, paramName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidIataCode"/> class with a specified error message,
    /// the parameter name, and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="paramName">The name of the parameter that caused the current exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public InvalidIataCode(string? message, string? paramName, Exception? innerException) : base(message, paramName, innerException)
    {
    }
}