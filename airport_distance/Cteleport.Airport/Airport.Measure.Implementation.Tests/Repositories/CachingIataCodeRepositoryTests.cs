using Airport.Measure.Domain.Entities.Codes;
using Airport.Measure.Domain.Entities.Locations;
using Airport.Measure.Domain.Repositories;
using Airport.Measure.Implementation.Repositories;
using Airport.Measure.Implementation.Repositories.Cache;
using Moq;

namespace Airport.Measure.Implementation.Tests.Repositories;

[TestFixture]
public class CachingIataCodeRepositoryTests
{
    [Test]
    public async Task ShouldUseCacheTest()
    {
        // arrange 
        var code = new IataCode("AMS");
        var expected = new LocationPoint(1.1, 2.2);
        
        var repo = new Mock<IAirportCodesRepository>();
        repo
            .Setup(x => x.GetLocationAsync(code))
            .ReturnsAsync(expected);
        
        var cache = new Mock<IRepositoryCache>();

        var cachingRepo = new CachingIataCodeRepository(repo.Object, cache.Object);

        // act
        var location = await cachingRepo.GetLocationAsync(code);
        
        // assert
        Assert.That(location, Is.EqualTo(expected));
        cache.Verify(x => x.GetAsync(code), Times.Once);
        cache.Verify(x => x.PutAsync(code, expected), Times.Once);
    }
}