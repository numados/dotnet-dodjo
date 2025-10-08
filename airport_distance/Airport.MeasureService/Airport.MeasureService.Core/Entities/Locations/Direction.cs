using System.Runtime.Serialization;

namespace Airport.MeasureService.Core.Entities.Locations;

/// <summary>
/// Represents the direction for distance calculation between two geographic points.
/// </summary>
/// <remarks>
/// This enum is used to specify whether to calculate distance traveling eastward or westward around the globe.
/// </remarks>
public enum Direction
{
    /// <summary>
    /// Represents the eastward direction.
    /// </summary>
    [EnumMember(Value = "East")]
    East = 0,

    /// <summary>
    /// Represents the westward direction.
    /// </summary>
    [EnumMember(Value = "West")]
    West = 1
}

