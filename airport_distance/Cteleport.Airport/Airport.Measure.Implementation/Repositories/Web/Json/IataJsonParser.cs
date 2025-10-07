using System.Text.Json;
using Airport.Measure.Domain.Entities.Locations;
using Airport.Measure.Implementation.Repositories.Web.Json.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Airport.Measure.Implementation.Repositories.Web.Json;

/// <summary>
/// Default implementation of <see cref="IJsonParser"/> for parsing IATA airport data from JSON responses.
/// </summary>
/// <remarks>
/// This parser expects JSON responses containing airport information with location coordinates.
/// It validates the response type and extracts longitude and latitude values.
/// </remarks>
public class IataJsonParser(ILogger<IataJsonParser>? logger = null) : IJsonParser
{
    private const string AIRPORT_TYPE = "airport";

    private readonly ILogger<IataJsonParser> _logger = logger ?? NullLogger<IataJsonParser>.Instance;

    /// <summary>
    /// Data transfer object for location coordinates.
    /// </summary>
    private class LocationDto
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
    }

    /// <summary>
    /// Data transfer object for airport location data from JSON response.
    /// </summary>
    private class LocationDataDto
    {
        public string? Type { get; set; }
        public string? Detail { get; set; }
        public LocationDto? Location { get; set; }
    }


    /// <inheritdoc />
    public LocationPoint GetLocationFromJson(string json)
    {
        _logger.LogTrace("Parse JSON: {Json}", json);

        // Validate input
        if (string.IsNullOrWhiteSpace(json))
            throw new InvalidJsonContentException("Invalid JSON content for IATA code");

        // Parse JSON to DTO
        var obj = JsonSerializer.Deserialize<LocationDataDto>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Check if airport was found and has location data
        if (obj?.Type == AIRPORT_TYPE)
        {
            if (obj.Location == null)
                throw new FailedJsonParsingException("Failed to parse JSON for IATA code. Result is null.");

            return new LocationPoint(obj.Location.Lon, obj.Location.Lat);
        }

        // No location found
        if (obj?.Type == null)
            throw new NoLocationFoundException(obj?.Detail ?? "No airport found");

        // Unexpected response type
        throw new UnexpectedJsonParserException("Unexpected exception when parsing JSON");
    }
}