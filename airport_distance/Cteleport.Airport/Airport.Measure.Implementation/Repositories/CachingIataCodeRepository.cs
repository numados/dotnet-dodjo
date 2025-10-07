using Airport.Measure.Domain.Entities.Codes;
using Airport.Measure.Domain.Entities.Locations;
using Airport.Measure.Domain.Exceptions;
using Airport.Measure.Domain.Repositories;
using Airport.Measure.Implementation.Repositories.Cache;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Airport.Measure.Implementation.Repositories;

/// <summary>
/// Decorator implementation of <see cref="IAirportCodesRepository"/> that adds caching capabilities.
/// </summary>
/// <remarks>
/// This repository implements the decorator pattern to add a caching layer on top of another repository.
/// It first checks the cache for requested data, and only queries the underlying repository if the data is not cached.
/// Successfully retrieved data is automatically stored in the cache for future requests.
/// </remarks>
public class CachingIataCodeRepository(
    IAirportCodesRepository codesRepository,
    IRepositoryCache cache,
    ILogger<CachingIataCodeRepository>? logger = null) : IAirportCodesRepository
{
    private readonly IAirportCodesRepository _codesRepository = codesRepository ?? throw new ArgumentNullException(nameof(codesRepository));
    private readonly IRepositoryCache _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    private readonly ILogger<CachingIataCodeRepository> _logger = logger ?? NullLogger<CachingIataCodeRepository>.Instance;

    /// <inheritdoc />
    /// <exception cref="InvalidIataCode">Thrown when <paramref name="code"/> is null.</exception>
    /// <remarks>
    /// This method implements a cache-aside pattern:
    /// 1. First checks if the location is in the cache
    /// 2. If found in cache, returns the cached value
    /// 3. If not in cache, retrieves from the underlying repository
    /// 4. Stores the retrieved value in cache for future requests
    /// </remarks>
    public async Task<LocationPoint?> GetLocationAsync(IataCode code)
    {
        _logger.LogDebug("Get Location for '{IataCode}' from Caching Repository", code?.Value);

        // Validate input
        if (code == null)
            throw new InvalidIataCode("IATA code is not specified");

        // Try to get from cache first
        var location = await _cache.GetAsync(code);
        if (location != null)
        {
            _logger.LogDebug(
                "Cache contains Location for '{IataCode}'. Data: '{Latitude}', '{Longitude}'",
                code.Value,
                location.Value.Latitude,
                location.Value.Longitude);

            return location.Value;
        }

        // Cache miss - retrieve from underlying repository
        location = await _codesRepository.GetLocationAsync(code);

        // Store in cache for future requests
        if (location != null)
        {
            _logger.LogDebug(
                "Update record in Cache for '{IataCode}'. Data: '{Latitude}', '{Longitude}'",
                code.Value,
                location.Value.Latitude,
                location.Value.Longitude);

            await _cache.PutAsync(code, location.Value);
        }

        return location;
    }
}