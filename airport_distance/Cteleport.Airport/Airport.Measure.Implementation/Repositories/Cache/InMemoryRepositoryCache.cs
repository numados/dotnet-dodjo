using Airport.Measure.Domain.Entities.Codes;
using Airport.Measure.Domain.Entities.Locations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Airport.Measure.Implementation.Repositories.Cache;

/// <summary>
/// In-memory implementation of <see cref="IRepositoryCache"/> using a dictionary for fast lookups.
/// </summary>
/// <remarks>
/// This cache stores airport location data in application memory. Data is lost when the application restarts.
/// This implementation is suitable for single-instance applications or development/testing scenarios.
/// For distributed or production scenarios, consider using <see cref="RedisRepositoryCache"/> instead.
/// </remarks>
public class InMemoryRepositoryCache(ILogger<InMemoryRepositoryCache>? logger = null) : IRepositoryCache
{
    private readonly IDictionary<string, LocationPoint> _dict = new Dictionary<string, LocationPoint>();
    private readonly ILogger<InMemoryRepositoryCache> _logger = logger ?? NullLogger<InMemoryRepositoryCache>.Instance;

    /// <inheritdoc />
    /// <remarks>
    /// This operation is thread-safe for reads but not for concurrent reads and writes.
    /// </remarks>
    public ValueTask<LocationPoint?> GetAsync(IataCode code)
    {
        _logger.LogTrace("Get '{IataCode}' from in-memory cache", code?.Value);

        // Check if code exists in cache
        if (code == null || !_dict.ContainsKey(code.Value))
            return ValueTask.FromResult<LocationPoint?>(null);

        // Return cached value
        return ValueTask.FromResult<LocationPoint?>(_dict[code.Value]);
    }

    /// <inheritdoc />
    /// <remarks>
    /// This operation is not thread-safe. For concurrent access, consider using a thread-safe collection.
    /// </remarks>
    public Task PutAsync(IataCode code, LocationPoint location)
    {
        _logger.LogTrace("Put '{IataCode}' into in-memory cache", code?.Value);

        if (code != null)
        {
            _dict[code.Value] = location;
        }

        return Task.CompletedTask;
    }
}