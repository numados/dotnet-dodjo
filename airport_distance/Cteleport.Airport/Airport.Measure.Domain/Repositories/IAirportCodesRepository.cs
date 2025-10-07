using Airport.Measure.Domain.Entities.Codes;
using Airport.Measure.Domain.Entities.Locations;

namespace Airport.Measure.Domain.Repositories;

/// <summary>
/// Defines a repository for retrieving airport location data by IATA code.
/// </summary>
/// <remarks>
/// Implementations of this interface may retrieve data from various sources such as
/// web APIs, databases, or cached storage.
/// </remarks>
public interface IAirportCodesRepository
{
     /// <summary>
     /// Retrieves the geographic location for the specified IATA airport code.
     /// </summary>
     /// <param name="code">The IATA code of the airport to locate.</param>
     /// <returns>
     /// A task that represents the asynchronous operation. The task result contains
     /// the <see cref="LocationPoint"/> if the airport is found; otherwise, <c>null</c>.
     /// </returns>
     Task<LocationPoint?> GetLocationAsync(IataCode code);
}