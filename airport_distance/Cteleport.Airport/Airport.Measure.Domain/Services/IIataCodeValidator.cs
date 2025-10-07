namespace Airport.Measure.Domain.Services;

/// <summary>
/// Defines a service for validating IATA airport codes.
/// </summary>
/// <remarks>
/// IATA codes are three-letter geocodes designating airports worldwide.
/// Valid codes consist of exactly three uppercase letters (A-Z).
/// </remarks>
public interface IIataCodeValidator
{
    /// <summary>
    /// Validates whether a given string conforms to the IATA airport code format.
    /// </summary>
    /// <param name="code">The string to validate.</param>
    /// <returns>
    /// <c>true</c> if the string is a valid IATA airport code (three uppercase letters); otherwise, <c>false</c>.
    /// </returns>
    bool IsValidIataCode(string code);
}