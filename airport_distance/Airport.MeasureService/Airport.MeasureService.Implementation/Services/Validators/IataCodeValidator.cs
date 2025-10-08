using System.Text.RegularExpressions;
using Airport.MeasureService.Core.Services;

namespace Airport.MeasureService.Implementation.Services.Validators;

/// <summary>
/// Provides validation for IATA airport codes using regular expression pattern matching.
/// </summary>
/// <remarks>
/// This validator checks that the code consists of exactly three uppercase letters (A-Z).
/// </remarks>
public class IataCodeValidator : IIataCodeValidator
{
    /// <summary>
    /// Regular expression pattern for validating IATA codes (three uppercase letters).
    /// </summary>
    private const string PATTERN = @"^[A-Z]{3}$";

    /// <inheritdoc />
    /// <remarks>
    /// The validation is case-sensitive and requires exactly three uppercase letters.
    /// Codes with lowercase letters, numbers, or special characters will be rejected.
    /// </remarks>
    public bool IsValidIataCode(string code)
        => Regex.IsMatch(code, PATTERN);
}