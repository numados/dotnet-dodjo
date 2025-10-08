using Airport.MeasureService.Core.Entities.Locations;

namespace Airport.MeasureService.Implementation.Repositories.Web;

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