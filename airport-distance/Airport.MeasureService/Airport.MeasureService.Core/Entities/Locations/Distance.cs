namespace Airport.MeasureService.Core.Entities.Locations;

/// <summary>
/// Represents a distance measurement in miles between two geographic points.
/// </summary>
/// <remarks>
/// The distance is stored as an integer value, with fractional miles truncated.
/// </remarks>
public struct Distance
{
    /// <summary>
    /// Gets the distance in miles (rounded down to the nearest whole number).
    /// </summary>
    public int Miles { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Distance"/> struct with the specified distance in miles.
    /// </summary>
    /// <param name="miles">The distance in miles. Fractional values are truncated to an integer.</param>
    public Distance(double miles)
    {
        Miles = (int)miles;
    }
}