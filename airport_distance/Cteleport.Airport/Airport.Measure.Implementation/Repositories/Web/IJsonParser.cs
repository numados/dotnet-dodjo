using Airport.Measure.Domain.Entities.Locations;

namespace Airport.Measure.Implementation.Repositories.Web;

/// <summary>
/// Specific JSON parser
/// </summary>
public interface IJsonParser
{
    /// <summary>
    /// Get <see cref="LocationPoint"/> from JSON
    /// </summary>
    /// <param name="json">JSON content</param>
    /// <returns><see cref="LocationPoint"/> instance</returns>
    public LocationPoint GetLocationFromJson(string json);
}