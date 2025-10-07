using Airport.Measure.Domain.Entities.Locations;

namespace Airport.Measure.Domain.Services;

/// <summary>
/// Defines a service for calculating the distance between two geographic points.
/// </summary>
/// <remarks>
/// Implementations may use different algorithms such as Haversine formula, Vincenty formula,
/// or other geodesic distance calculation methods.
/// </remarks>
public interface IDistanceCalculator
{
    /// <summary>
    /// Calculates the distance between two geographic location points.
    /// </summary>
    /// <param name="from">The starting location point.</param>
    /// <param name="to">The destination location point.</param>
    /// <returns>
    /// A <see cref="Distance"/> object representing the calculated distance in miles.
    /// </returns>
    Distance Calculate(LocationPoint from, LocationPoint to);
}