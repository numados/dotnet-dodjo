using System.Text.Json;
using Airport.MeasureService.Core.Entities.Codes;
using Airport.MeasureService.Core.Entities.Locations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Airport.MeasureService.Implementation.Repositories.Cache;

/// <summary>
/// Redis-based implementation of <see cref="IRepositoryCache"/> using distributed caching.
/// </summary>
/// <remarks>
/// This cache stores airport location data in Redis, making it suitable for distributed applications
/// and production scenarios where data persistence across application restarts is desired.
/// Data is serialized to JSON before storage and deserialized upon retrieval.
/// </remarks>
public class RedisRepositoryCache(
    IDistributedCache cache,
    ILogger<RedisRepositoryCache>? logger = null) : IRepositoryCache
{
    private readonly IDistributedCache _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    private readonly ILogger<RedisRepositoryCache> _logger = logger ?? NullLogger<RedisRepositoryCache>.Instance;

    /// <inheritdoc />
    /// <remarks>
    /// Retrieves data from Redis and deserializes it from JSON. Returns null if the key is not found
    /// or if an error occurs during retrieval or deserialization.
    /// </remarks>
    public async ValueTask<LocationPoint?> GetAsync(IataCode code)
    {
        _logger.LogTrace("Get '{IataCode}' from Redis Cache", code?.Value);

        // Validate input
        if (code == null)
            return null;

        // Retrieve from Redis
        try
        {
            var key = code.Value;
            var jsonContentOfLocation = await _cache.GetStringAsync(key);
            if (string.IsNullOrWhiteSpace(jsonContentOfLocation))
            {
                _logger.LogInformation("Cache does not contain info for '{IataCode}'", key);
                return null;
            }
            _logger.LogTrace("JSON from Redis cache: {Json}", jsonContentOfLocation);

            // Deserialize JSON to LocationPoint
            var location = JsonSerializer.Deserialize<LocationPoint>(jsonContentOfLocation);
            _logger.LogTrace(
                "Redis Cache contains data for '{IataCode}': {Longitude} and {Latitude}",
                key,
                location.Longitude,
                location.Latitude);

            return location;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while reading data from Redis cache");
            return null;
        }
    }

    /// <inheritdoc />
    /// <remarks>
    /// Serializes the location to JSON and stores it in Redis. Errors during storage are logged but not thrown.
    /// Cache expiration is currently not configured and should be set based on application requirements.
    /// </remarks>
    public async Task PutAsync(IataCode code, LocationPoint location)
    {
        _logger.LogTrace("Put '{IataCode}' to Redis Cache", code?.Value);

        if (code == null)
        {
            _logger.LogWarning("Cannot write empty code as key to Redis");
            return;
        }

        try
        {
            var key = code.Value;
            var json = JsonSerializer.Serialize(location);
            var options = new DistributedCacheEntryOptions
            {
                // TODO: Configure cache expiration based on application requirements
                // Example: AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            };

            await _cache.SetStringAsync(key, json, options);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while storing data in Redis cache");
        }
    }
}