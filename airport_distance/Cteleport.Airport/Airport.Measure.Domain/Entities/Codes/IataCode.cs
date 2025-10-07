using Airport.Measure.Domain.Exceptions;

namespace Airport.Measure.Domain.Entities.Codes;

/// <summary>
/// Represents an IATA (International Air Transport Association) airport code.
/// </summary>
/// <remarks>
/// IATA codes are three-letter geocodes designating airports worldwide.
/// The code is automatically normalized to uppercase and trimmed of whitespace.
/// </remarks>
public record IataCode
{
    private const string PATTERN = @"^[A-Z]{3}$";

    /// <summary>
    /// Gets the normalized IATA code value (three uppercase letters).
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IataCode"/> record with the specified code value.
    /// </summary>
    /// <param name="value">The IATA code string to normalize and validate.</param>
    /// <exception cref="InvalidIataCode">Thrown when the provided value is null, empty, or whitespace.</exception>
    public IataCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidIataCode("IATA code cannot be empty");

        Value = Normalize(value);
    }

    /// <summary>
    /// Normalizes the IATA code by converting it to uppercase and trimming whitespace.
    /// </summary>
    /// <param name="code">The code to normalize.</param>
    /// <returns>The normalized IATA code.</returns>
    private string Normalize(string code)
        => code.ToUpper().Trim();
}