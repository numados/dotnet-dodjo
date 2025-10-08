using Airport.MeasureService.Core.Entities.Locations;

namespace Airport.MeasureService.Api.Extensions;

public static class StringExtensions
{
    public static Direction ToDirection(this string value)
    {
        // WEST 
        if (string.Equals(value, "w", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(value, "west", StringComparison.OrdinalIgnoreCase))
        {
            return Direction.West;
        }
        
        // EAST 
        if (string.Equals(value, "e", StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(value, "east", StringComparison.OrdinalIgnoreCase))
        {
            return Direction.East;
        }
        
        // NOT SUPPORTED
        throw new ArgumentException($"Direction '{value}' is not supported");
    }
}