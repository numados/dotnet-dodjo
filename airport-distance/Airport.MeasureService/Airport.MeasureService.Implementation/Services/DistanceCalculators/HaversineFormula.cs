using Airport.MeasureService.Core.Entities.Locations;
using Airport.MeasureService.Core.Services;

namespace Airport.MeasureService.Implementation.Services.DistanceCalculators;

/// <summary>
/// Implements the Haversine formula to calculate the great-circle distance between two points on a sphere.
/// </summary>
/// <remarks>
/// The Haversine formula determines the shortest distance over the earth's surface,
/// giving an "as-the-crow-flies" distance between the points (ignoring any hills, valleys, or obstacles).
/// For more information, see: https://en.wikipedia.org/wiki/Haversine_formula
/// </remarks>
public class HaversineFormula : IDistanceCalculator
{
    /// <summary>
    /// The Earth's radius in miles, used for distance calculations.
    /// </summary>
    private const double EARTH_RADIUS_IN_MILES = 3963.19;

    /// <inheritdoc />
    /// <remarks>
    /// This implementation uses the Haversine formula which provides good accuracy for most purposes.
    /// The formula assumes the Earth is a perfect sphere, which introduces a small error (typically less than 0.5%).
    /// </remarks>
    public Distance Calculate(LocationPoint from, LocationPoint to)
    {
        // Convert latitude and longitude from degrees to radians
        var radLat1 = Math.PI * from.Latitude / 180.0;
        var radLon1 = Math.PI * from.Longitude / 180.0;
        var radLat2 = Math.PI * to.Latitude / 180.0;
        var radLon2 = Math.PI * to.Longitude / 180.0;

        // Calculate differences in coordinates
        var deltaLat = radLat2 - radLat1;
        var deltaLon = radLon2 - radLon1;

        // Apply Haversine formula
        // a = sin²(Δlat/2) + cos(lat1) * cos(lat2) * sin²(Δlon/2)
        var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                Math.Cos(radLat1) * Math.Cos(radLat2) *
                Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

        // c = 2 * atan2(√a, √(1−a))
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        // Calculate distance: d = R * c
        var miles = EARTH_RADIUS_IN_MILES * c;

        return new Distance(miles);
    }
}