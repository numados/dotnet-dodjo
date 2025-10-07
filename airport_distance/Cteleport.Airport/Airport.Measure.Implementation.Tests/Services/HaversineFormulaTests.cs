using Airport.Measure.Domain.Entities.Locations;
using Airport.Measure.Implementation.Services.DistanceCalculators;

namespace Airport.Measure.Implementation.Tests.Services;

[TestFixture]
public class HaversineFormulaTests
{
    [Test]
    public void ShouldCalculateDistanceBetweenTwoPoints()
    {
        // arrange 
        var dmeLocation = new LocationPoint(37.899494, 55.414566);
        var amsLocation = new LocationPoint(4.763385, 52.309069);
        
        // act 
        var distance = new HaversineFormula().Calculate(dmeLocation, amsLocation);

        // assert 
        Assert.That(distance.Miles, Is.EqualTo(1355));
    }
}