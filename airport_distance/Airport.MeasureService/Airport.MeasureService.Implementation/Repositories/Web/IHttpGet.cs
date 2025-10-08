namespace Airport.MeasureService.Implementation.Repositories.Web;

/// <summary>
/// HTTP GET
/// </summary>
public interface IHttpGet
{
    /// <summary>
    /// Fetch data from provided URL
    /// </summary>
    /// <param name="code">IATA code</param>
    /// <returns>Response from GET operation</returns>
    public Task<string?> GetAsync(string code);
}