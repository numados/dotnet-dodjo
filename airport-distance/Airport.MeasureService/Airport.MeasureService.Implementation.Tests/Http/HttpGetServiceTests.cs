using Airport.MeasureService.Implementation.Repositories.Web.Http;

namespace Airport.MeasureService.Implementation.Tests.Http;

[TestFixture]
public class HttpGetServiceTests
{
    [Test]
    [Ignore("Make real request. Should be in integration tests")]
    public async Task ShouldGetResponseTest()
    {
        // arrange
        var baseUrl = "https://example.com/airports";

        var http = new HttpGetService(baseUrl);

        // act
        var response = await http.GetAsync("AMS");
        
        // assert 
        Assert.That(response, Is.Not.Null);
    }
}