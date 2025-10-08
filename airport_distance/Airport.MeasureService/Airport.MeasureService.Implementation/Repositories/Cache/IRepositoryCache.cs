using Airport.MeasureService.Core.Entities.Codes;
using Airport.MeasureService.Core.Entities.Locations;

namespace Airport.MeasureService.Implementation.Repositories.Cache;

/// <summary>
/// Defines a cache for storing and retrieving airport location data by IATA code.
/// </summary>
/// <remarks>
/// Implementations may use different caching strategies such as in-memory caching,
/// distributed caching (Redis), or other caching mechanisms.
/// </remarks>
public interface IRepositoryCache
{
    /// <summary>
    /// Retrieves the cached location for the specified IATA code.
    /// </summary>
    /// <param name="code">The IATA code to look up in the cache.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// the cached <see cref="LocationPoint"/> if found; otherwise, <c>null</c>.
    /// </returns>
    ValueTask<LocationPoint?> GetAsync(IataCode code);

    /// <summary>
    /// Stores a location in the cache associated with the specified IATA code.
    /// </summary>
    /// <param name="code">The IATA code to use as the cache key.</param>
    /// <param name="location">The location data to cache.</param>
    /// <returns>A task that represents the asynchronous put operation.</returns>
    Task PutAsync(IataCode code, LocationPoint location);
}