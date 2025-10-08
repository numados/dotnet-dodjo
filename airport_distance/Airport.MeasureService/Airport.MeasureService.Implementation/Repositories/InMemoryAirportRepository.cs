using Airport.MeasureService.Core.Entities.Codes;
using Airport.MeasureService.Core.Entities.Locations;
using Airport.MeasureService.Core.Repositories;

namespace Airport.MeasureService.Implementation.Repositories;

/// <summary>
/// In-memory implementation of <see cref="IAirportCodesRepository"/> for testing and development.
/// </summary>
/// <remarks>
/// This repository contains a predefined set of major airports with their coordinates.
/// Use this for development and testing when an external API is not available.
/// </remarks>
public class InMemoryAirportRepository : IAirportCodesRepository
{
    private static readonly Dictionary<string, LocationPoint> _airports = new()
    {
        // Europe
        ["AMS"] = new LocationPoint(4.76389, 52.30833),    // Amsterdam Schiphol
        ["LHR"] = new LocationPoint(-0.45436, 51.47002),   // London Heathrow
        ["CDG"] = new LocationPoint(2.54778, 49.00972),    // Paris Charles de Gaulle
        ["FRA"] = new LocationPoint(8.57056, 50.03333),    // Frankfurt
        ["MAD"] = new LocationPoint(-3.56778, 40.47194),   // Madrid
        ["BCN"] = new LocationPoint(2.07833, 41.29694),    // Barcelona
        ["FCO"] = new LocationPoint(12.25111, 41.80028),   // Rome Fiumicino
        ["MUC"] = new LocationPoint(11.78611, 48.35389),   // Munich
        ["ZRH"] = new LocationPoint(8.54917, 47.45806),    // Zurich
        ["VIE"] = new LocationPoint(16.56972, 48.11028),   // Vienna
        ["CPH"] = new LocationPoint(12.65611, 55.61806),   // Copenhagen
        ["ARN"] = new LocationPoint(17.91861, 59.65194),   // Stockholm Arlanda
        ["OSL"] = new LocationPoint(11.10028, 60.19389),   // Oslo
        ["HEL"] = new LocationPoint(24.96333, 60.31722),   // Helsinki
        ["DUB"] = new LocationPoint(-6.27, 53.42139),      // Dublin
        ["LIS"] = new LocationPoint(-9.13592, 38.77417),   // Lisbon
        ["ATH"] = new LocationPoint(23.94444, 37.93639),   // Athens
        ["IST"] = new LocationPoint(28.81583, 41.27583),   // Istanbul
        ["PRG"] = new LocationPoint(14.26, 50.10083),      // Prague
        ["WAW"] = new LocationPoint(20.96722, 52.16583),   // Warsaw
        
        // North America
        ["JFK"] = new LocationPoint(-73.77889, 40.63972),  // New York JFK
        ["LAX"] = new LocationPoint(-118.40806, 33.94250), // Los Angeles
        ["ORD"] = new LocationPoint(-87.90472, 41.97861),  // Chicago O'Hare
        ["DFW"] = new LocationPoint(-97.03806, 32.89694),  // Dallas/Fort Worth
        ["DEN"] = new LocationPoint(-104.67306, 39.85833), // Denver
        ["SFO"] = new LocationPoint(-122.37500, 37.61889), // San Francisco
        ["SEA"] = new LocationPoint(-122.30944, 47.44889), // Seattle
        ["LAS"] = new LocationPoint(-115.15222, 36.08000), // Las Vegas
        ["MIA"] = new LocationPoint(-80.29056, 25.79325),  // Miami
        ["ATL"] = new LocationPoint(-84.42806, 33.64044),  // Atlanta
        ["BOS"] = new LocationPoint(-71.00528, 42.36444),  // Boston
        ["IAD"] = new LocationPoint(-77.45583, 38.94472),  // Washington Dulles
        ["PHX"] = new LocationPoint(-112.01167, 33.43417), // Phoenix
        ["IAH"] = new LocationPoint(-95.34139, 29.98444),  // Houston
        ["MSP"] = new LocationPoint(-93.22167, 44.88194),  // Minneapolis
        ["DTW"] = new LocationPoint(-83.35333, 42.21222),  // Detroit
        ["PHL"] = new LocationPoint(-75.24111, 39.87194),  // Philadelphia
        ["LGA"] = new LocationPoint(-73.87250, 40.77722),  // New York LaGuardia
        ["YYZ"] = new LocationPoint(-79.63056, 43.67722),  // Toronto
        ["YVR"] = new LocationPoint(-123.18389, 49.19389), // Vancouver
        ["YUL"] = new LocationPoint(-73.74083, 45.47056),  // Montreal
        
        // Asia
        ["HND"] = new LocationPoint(139.78111, 35.55306),  // Tokyo Haneda
        ["NRT"] = new LocationPoint(140.38694, 35.76472),  // Tokyo Narita
        ["PEK"] = new LocationPoint(116.58444, 40.08028),  // Beijing Capital
        ["PVG"] = new LocationPoint(121.80583, 31.14333),  // Shanghai Pudong
        ["HKG"] = new LocationPoint(113.91444, 22.30889),  // Hong Kong
        ["SIN"] = new LocationPoint(103.99444, 1.35917),   // Singapore
        ["ICN"] = new LocationPoint(126.45083, 37.46917),  // Seoul Incheon
        ["BKK"] = new LocationPoint(100.74722, 13.68111),  // Bangkok
        ["KUL"] = new LocationPoint(101.70972, 2.74556),   // Kuala Lumpur
        ["DEL"] = new LocationPoint(77.10306, 28.55583),   // Delhi
        ["BOM"] = new LocationPoint(72.86778, 19.08861),   // Mumbai
        ["DXB"] = new LocationPoint(55.36444, 25.25278),   // Dubai
        ["DOH"] = new LocationPoint(51.60806, 25.27306),   // Doha
        
        // Oceania
        ["SYD"] = new LocationPoint(151.17722, -33.94611), // Sydney
        ["MEL"] = new LocationPoint(144.84333, -37.67333), // Melbourne
        ["BNE"] = new LocationPoint(153.11722, -27.38417), // Brisbane
        ["AKL"] = new LocationPoint(174.78500, -37.00806), // Auckland
        
        // South America
        ["GRU"] = new LocationPoint(-46.47306, -23.43556), // São Paulo
        ["GIG"] = new LocationPoint(-43.24306, -22.81000), // Rio de Janeiro
        ["EZE"] = new LocationPoint(-58.53583, -34.82222), // Buenos Aires
        ["SCL"] = new LocationPoint(-70.78583, -33.39306), // Santiago
        ["BOG"] = new LocationPoint(-74.14639, 4.70167),   // Bogotá
        ["LIM"] = new LocationPoint(-77.11444, -12.02194), // Lima
        
        // Africa
        ["JNB"] = new LocationPoint(28.24611, -26.13917),  // Johannesburg
        ["CPT"] = new LocationPoint(18.60194, -33.96472),  // Cape Town
        ["CAI"] = new LocationPoint(31.40556, 30.12194),   // Cairo
        ["NBO"] = new LocationPoint(36.92778, -1.31917),   // Nairobi
        
        // Additional UK airports
        ["LGW"] = new LocationPoint(-0.19028, 51.14806),   // London Gatwick
        ["MAN"] = new LocationPoint(-2.27500, 53.35389),   // Manchester
        ["EDI"] = new LocationPoint(-3.37222, 55.95000),   // Edinburgh
        ["BHX"] = new LocationPoint(-1.74806, 52.45389),   // Birmingham
        ["GLA"] = new LocationPoint(-4.43306, 55.87194),   // Glasgow
        
        // LON is often used as a generic code for London airports
        ["LON"] = new LocationPoint(-0.45436, 51.47002),   // London (using Heathrow coordinates)
    };

    /// <inheritdoc />
    public Task<LocationPoint?> GetLocationAsync(IataCode code)
    {
        if (_airports.TryGetValue(code.Value, out var location))
        {
            return Task.FromResult<LocationPoint?>(location);
        }

        // Return null if airport not found (following the interface contract)
        return Task.FromResult<LocationPoint?>(null);
    }
}

