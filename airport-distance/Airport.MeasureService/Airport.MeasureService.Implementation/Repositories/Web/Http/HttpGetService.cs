using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Airport.MeasureService.Implementation.Repositories.Web.Http;

/// <summary>
/// HTTP Get service
/// </summary>
public class HttpGetService: IHttpGet
{
    #region .ctor

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="baseUrl">Base URL</param>
    public HttpGetService(string baseUrl): this(baseUrl, NullLogger<HttpGetService>.Instance) {}
    
    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="baseUrl">Base URL</param>
    /// <param name="logger">Logger instance for logging HTTP operations</param>
    public HttpGetService(
        string baseUrl,
        ILogger<HttpGetService> logger)
    {
        // validate
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new AggregateException("Base URL for IATA repository cannot be empty");

        // init
        if (!baseUrl.EndsWith("/"))
            baseUrl += "/";
            
        baseUri = new Uri(baseUrl);
        _logger = logger;

        _logger.LogDebug("Base URL: {URL}", baseUrl);
    }
    
    #endregion
    
    #region Private

    private readonly HttpClient _http = new HttpClient();
    private readonly Uri baseUri;
    private readonly ILogger<HttpGetService> _logger;

    #endregion
    
    #region IHttpGet implementation

    /// <inheritdoc />
    public async Task<string?> GetAsync(string code)
    {
        // build url 
        var uri = new Uri(baseUri, code);
        
        // make a request 
        var response = await _http.GetAsync(uri);

        //
        return await response.Content.ReadAsStringAsync();
    }
    
    #endregion
}