using Airport.Measure.Domain.Entities.Codes;
using Airport.Measure.Domain.Entities.Locations;
using Airport.Measure.Implementation.Repositories;
using Airport.Measure.Implementation.Repositories.Web;
using Moq;

namespace Airport.Measure.Implementation.Tests.Repositories;

[TestFixture]
public class WebIataCodeRepositoryTests
{
    [TestFixture]
    public class GetLocationTests
    {
        [Test]
        public void ShouldThrowExceptionIfCodeDidNotProvided()
        {
            // arrange
            var httpMock = new Mock<IHttpGet>();
            var jsonMock = new Mock<IJsonParser>();

            var repo = new WebIataCodeRepository(httpMock.Object, jsonMock.Object);

            // assert
            Assert.ThrowsAsync<ArgumentNullException>(() => repo.GetLocationAsync(null));
        }

        [Test]
        public async Task ShouldMakeRequestAndParseResult()
        {
            // arrange
            var httpMock = new Mock<IHttpGet>();
            httpMock
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync("{}");

            var expected = new LocationPoint(12.356, 34.78);
            var jsonMock = new Mock<IJsonParser>();
            jsonMock
                .Setup(x => x.GetLocationFromJson(It.IsAny<string>()))
                .Returns(expected);

            var repo = new WebIataCodeRepository(httpMock.Object, jsonMock.Object);

            // act
            var location = await repo.GetLocationAsync(new IataCode("AMS"));
            
            // assert
            Assert.That(location, Is.EqualTo(expected));
        }
    }
}