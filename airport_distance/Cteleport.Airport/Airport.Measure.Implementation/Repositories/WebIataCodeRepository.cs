using Airport.Measure.Domain.Entities.Codes;
using Airport.Measure.Domain.Entities.Locations;
using Airport.Measure.Domain.Repositories;
using Airport.Measure.Implementation.Exceptions;
using Airport.Measure.Implementation.Repositories.Web;
using Airport.Measure.Implementation.Repositories.Web.Json.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Airport.Measure.Implementation.Repositories;

/// <summary>
/// Repository implementation that retrieves airport location data from a web-based data source.
/// </summary>
/// <remarks>
/// This repository fetches airport information via HTTP requests and parses JSON responses
/// to extract geographic coordinates for IATA airport codes.
/// </remarks>
public class WebIataCodeRepository(
    IHttpGet http,
    IJsonParser json,
    ILogger<WebIataCodeRepository>? logger = null) : IAirportCodesRepository
{
    private readonly IHttpGet _http = http ?? throw new ArgumentNullException(nameof(http), "HTTP getter must be provided.");
    private readonly IJsonParser _json = json ?? throw new ArgumentNullException(nameof(json), "JSON parser must be provided.");
    private readonly ILogger<WebIataCodeRepository> _logger = logger ?? NullLogger<WebIataCodeRepository>.Instance;

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="code"/> is null.</exception>
    /// <exception cref="FailedToGetLocationForIataCodeException">
    /// Thrown when the JSON response cannot be parsed or the location data is invalid.
    /// </exception>
    public async Task<LocationPoint?> GetLocationAsync(IataCode code)
    {
        _logger.LogDebug("Get locations from Web repository for '{IataCode}'", code?.Value);

        // Validate input
        if (code == null)
            throw new ArgumentNullException(nameof(code));

        // Make HTTP request to retrieve airport data
        var response = await _http.GetAsync(code.Value);
        if (string.IsNullOrWhiteSpace(response))
        {
            _logger.LogWarning("Response from Web for '{IataCode}' is empty", code.Value);
            return null;
        }

        // Parse JSON response to extract location
        try
        {
            _logger.LogDebug("Response for '{IataCode}': {Response}", code.Value, response);
            return _json.GetLocationFromJson(response);
        }
        catch (JsonParserException ex)
        {
            _logger.LogError("Error for JSON parsing: {Exception}", ex.ToString());
            throw new FailedToGetLocationForIataCodeException($"{code.Value}: {ex.Message}", ex);
        }
    }
}