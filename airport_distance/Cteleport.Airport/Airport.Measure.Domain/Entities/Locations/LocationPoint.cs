using System.Text.Json.Serialization;

namespace Airport.Measure.Domain.Entities.Locations;

/// <summary>
/// Represents a geographic point defining an airport's location using longitude and latitude coordinates.
/// </summary>
/// <remarks>
/// This struct is immutable and provides value-based equality comparison.
/// Coordinates are expressed in decimal degrees.
/// </remarks>
public struct LocationPoint
{
    /// <summary>
    /// Gets the longitude coordinate in decimal degrees.
    /// </summary>
    /// <remarks>
    /// Valid range is -180 to +180 degrees, where negative values represent west and positive values represent east.
    /// </remarks>
    public double Longitude { get; }

    /// <summary>
    /// Gets the latitude coordinate in decimal degrees.
    /// </summary>
    /// <remarks>
    /// Valid range is -90 to +90 degrees, where negative values represent south and positive values represent north.
    /// </remarks>
    public double Latitude { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocationPoint"/> struct with the specified coordinates.
    /// </summary>
    /// <param name="longitude">The longitude coordinate in decimal degrees.</param>
    /// <param name="latitude">The latitude coordinate in decimal degrees.</param>
    [JsonConstructor]
    public LocationPoint(double longitude, double latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }

    #region Equality

    /// <summary>
    /// Provides equality comparison for <see cref="LocationPoint"/> based on longitude and latitude values.
    /// </summary>
    private sealed class LongitudeLatitudeEqualityComparer : IEqualityComparer<LocationPoint>
    {
        public bool Equals(LocationPoint x, LocationPoint y)
        {
            return x.Longitude.Equals(y.Longitude) && x.Latitude.Equals(y.Latitude);
        }

        public int GetHashCode(LocationPoint obj)
        {
            return HashCode.Combine(obj.Longitude, obj.Latitude);
        }
    }

    /// <summary>
    /// Gets an equality comparer that compares <see cref="LocationPoint"/> instances based on their longitude and latitude values.
    /// </summary>
    public static IEqualityComparer<LocationPoint> LongitudeLatitudeComparer { get; } = new LongitudeLatitudeEqualityComparer();

    #endregion
}