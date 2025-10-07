using Airport.Measure.Domain.Entities.Locations;
using Airport.Measure.Implementation.Repositories.Web.Json;
using Airport.Measure.Implementation.Repositories.Web.Json.Exceptions;

namespace Airport.Measure.Implementation.Tests.Parsers;

[TestFixture]
public class IataJsonParserTests
{
    [Test]
    public void ShouldParseJsonToLocationPointInstance()
    {
        // arrange
        var json =
            "{\"iata\":\"AMS\",\"name\":\"Amsterdam\",\"city\":\"Amsterdam\",\"city_iata\":\"AMS\",\"country\":\"Netherlands\",\"country_iata\":\"NL\",\"location\":{\"lon\":4.763385,\"lat\":52.309069},\"rating\":3,\"hubs\":7,\"timezone_region_name\":\"Europe/Amsterdam\",\"type\":\"airport\"}";

        var expected = new LocationPoint(4.763385, 52.309069);

        var parser = new IataJsonParser();

        // act
        var location = parser.GetLocationFromJson(json);

        // assert
        Assert.That(location, Is.EqualTo(expected));
    }

    [Test]
    public void ShouldThrowExceptionWhenAirportNotFound()
    {
        // arrange
        var json =
            "{\"detail\":\"Airport not found\"}";

        var parser = new IataJsonParser();

        // assert
        Assert.Throws<NoLocationFoundException>(() => parser.GetLocationFromJson(json));
    }
}