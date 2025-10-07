using Airport.Measure.Domain.Entities.Codes;
using Airport.Measure.Domain.Entities.Locations;
using Airport.Measure.Implementation.Repositories.Cache;

namespace Airport.Measure.Implementation.Tests.Repositories;

[TestFixture]
public class InMemoryRepositoryCacheTests
{
    [Test]
    public async Task ShouldReturnNullIfNoValueInCache()
    {
        // arrange 
        var cache = new InMemoryRepositoryCache();
        
        // act
        var location = await cache.GetAsync(new IataCode("AMS"));
        
        // assert
        Assert.That(location, Is.Null);
    }
    
    [Test]
    public async Task ShouldReturnValueFromCache()
    {
        // arrange 
        var cache = new InMemoryRepositoryCache();

        var expected = new LocationPoint(12.34, 5.78);
        var code = new IataCode("AMS");
        
        // act
        await cache.PutAsync(code, expected);
        var location = await cache.GetAsync(new IataCode("AMS"));
        
        // assert 
        Assert.That(location, Is.EqualTo(expected));
    }
}