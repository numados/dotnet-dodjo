using System.ComponentModel.DataAnnotations;
using Airport.MeasureService.Core.Entities.Codes;
using Airport.MeasureService.Core.Repositories;
using Airport.MeasureService.Core.Services;
using Airport.MeasureService.Implementation.Exceptions;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Airport.MeasureService.Api.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/distance")]
public class DistanceCalculatorController: ControllerBase
{
    #region Private 
    
    private readonly IIataCodeValidator _iataCodeValidator;
    private readonly IAirportCodesRepository _repository;
    private readonly IDistanceCalculator _calculator;
    private readonly ILogger<DistanceCalculatorController> _logger;

    #endregion
    
    #region .ctor

    public DistanceCalculatorController(
        IIataCodeValidator iataCodeValidator,
        IAirportCodesRepository repository,
        IDistanceCalculator calculator,
        ILogger<DistanceCalculatorController> logger)
    {
        _iataCodeValidator = iataCodeValidator;
        _repository = repository;
        _calculator = calculator;
        _logger = logger;
    }
    
    #endregion
    
    #region Private

    private BadRequestObjectResult? ValidateIataCode(string code, string name) =>
        !_iataCodeValidator.IsValidIataCode(code) 
            ? BadRequest($"Invalid '{name}' parameter. It should be a valid 3-letter IATA code.") 
            : null;

    private async Task<ActionResult<double>> CalculateDistanceInMilesAsync(IataCode from, IataCode to)
    {
        try
        {
            var f = await _repository.GetLocationAsync(from);
            var t = await _repository.GetLocationAsync(to);

            // calculate distance
            var distance = _calculator.Calculate(f.Value, t.Value);

            // return result
            _logger.LogInformation("Calculated distance in miles: {Distance}", distance.Miles);
            return Ok(new { DistanceInMiles = distance.Miles });
        }
        catch (FailedToGetLocationForIataCodeException ex)
        {
            _logger.LogWarning(
                "Failed to calculate distance between '{FromIataCode}' and '{ToIataCode}': {Msg}",
                from.Value,
                to.Value,
                ex.Message);
            
            return BadRequest(ex.Message);
        }
    }

    #endregion

    /// <summary>
    /// Calculate the distance between two airports using their IATA codes.
    /// </summary>
    /// <param name="from">The IATA code of the departure airport.</param>
    /// <param name="to">The IATA code of the destination airport.</param>
    /// <returns>The distance in miles between the two airports.</returns>
    [HttpGet("calculate"), MapToApiVersion(1.0)]
    [SwaggerOperation(
        Summary = "Calculate distance between two airports.",
        Description = "Calculates the distance between two airports using their 3-letter IATA codes.",
        OperationId = "CalculateDistanceBetweenAirports",
        Tags = new[] { "Airport Distance" }
    )]
    public async Task<ActionResult<double>> CalculateDistanceBetweenAirports(
        [FromQuery] [Required] string from, 
        [FromQuery] [Required] string to)
    {
        _logger.LogInformation("Calculate distance between '{FromIataCode}' and '{ToIataCode}'", from, to);

        try
        {
            // parse input parameters 
            var airportFrom = new IataCode(from);
            var airportTo = new IataCode(to);

            // validate codes 
            var validationResult =
                ValidateIataCode(airportFrom.Value, nameof(from)) ??
                ValidateIataCode(airportTo.Value, nameof(to));
        
            if (validationResult != null)
                return validationResult;
        
            // calculate
            return await CalculateDistanceInMilesAsync(airportFrom, airportTo);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError("Exception of parsing parameters: {Ex}", ex.ToString());
            return BadRequest($"Ivalid input parameters: {ex.Message}");
        }
        catch(Exception ex)
        {
            _logger.LogCritical(ex.ToString());
            return StatusCode(500, "Internal Server Error");
        }
    }
}