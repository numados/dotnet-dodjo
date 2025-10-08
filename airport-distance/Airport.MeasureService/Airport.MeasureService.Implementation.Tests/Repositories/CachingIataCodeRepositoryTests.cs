using Airport.MeasureService.Core.Entities.Codes;
using Airport.MeasureService.Core.Entities.Locations;
using Airport.MeasureService.Core.Repositories;
using Airport.MeasureService.Implementation.Repositories;
using Airport.MeasureService.Implementation.Repositories.Cache;
using Moq;

namespace Airport.MeasureService.Implementation.Tests.Repositories;

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