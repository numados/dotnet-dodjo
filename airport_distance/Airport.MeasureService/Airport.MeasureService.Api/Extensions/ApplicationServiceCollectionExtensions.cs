using Airport.MeasureService.Core.Repositories;
using Airport.MeasureService.Core.Services;
using Airport.MeasureService.Implementation.Repositories;
using Airport.MeasureService.Implementation.Repositories.Cache;
using Airport.MeasureService.Implementation.Repositories.Web;
using Airport.MeasureService.Implementation.Repositories.Web.Http;
using Airport.MeasureService.Implementation.Repositories.Web.Json;
using Airport.MeasureService.Implementation.Services.DistanceCalculators;
using Airport.MeasureService.Implementation.Services.Validators;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;

namespace Airport.MeasureService.Api.Extensions;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to configure airport measurement services.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    /// <summary>
    /// Adds a web-based airport data source that fetches airport information from an external API.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/></param>
    /// <param name="url">Base URL of the external airport API</param>
    /// <exception cref="ArgumentException">Thrown when URL is empty or null</exception>
    /// <remarks>
    /// This is the DATA SOURCE layer - it provides the actual airport location data.
    /// Use this in production when you have access to a live airport data API.
    /// </remarks>
    public static IServiceCollection AddWebAirportDataSource(this IServiceCollection services, string url)
    {
       services.AddSingleton<IJsonParser, IataJsonParser>();
       services.AddSingleton<IHttpGet>(_ =>
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Web Repository base URL is not provided. Please, update configuration.");

            return new HttpGetService(url);
        });

       services.AddSingleton<IAirportCodesRepository, WebIataCodeRepository>();
       return services;
    }

    /// <summary>
    /// Adds a static in-memory airport data source with 80+ predefined major airports.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/></param>
    /// <remarks>
    /// This is the DATA SOURCE layer - it provides the actual airport location data.
    /// Use this for development, testing, or when an external API is not available.
    /// Contains major airports from Europe, North America, Asia, Oceania, South America, and Africa.
    /// </remarks>
    public static IServiceCollection AddStaticAirportDataSource(this IServiceCollection services)
    {
        services.AddSingleton<IAirportCodesRepository, InMemoryAirportRepository>();
        return services;
    }

    /// <summary>
    /// Adds an in-memory caching layer that decorates the airport data source.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/></param>
    /// <remarks>
    /// This is the CACHING layer - it wraps around any data source to cache results.
    /// Cache is stored in application memory and is lost on restart.
    /// Suitable for development and single-instance deployments.
    /// </remarks>
    public static IServiceCollection AddInMemoryCacheLayer(this IServiceCollection services)
    {
        services.AddSingleton<IRepositoryCache, InMemoryRepositoryCache>();
        services.Decorate<IAirportCodesRepository, CachingIataCodeRepository>();

        return services;
    }

    /// <summary>
    /// Adds a Redis-based distributed caching layer that decorates the airport data source.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/></param>
    /// <param name="connectionString">Redis connection string</param>
    /// <remarks>
    /// This is the CACHING layer - it wraps around any data source to cache results.
    /// Cache is stored in Redis and is shared across multiple instances.
    /// Suitable for production and multi-instance deployments.
    /// Requires a running Redis server.
    /// </remarks>
    public static IServiceCollection AddRedisCacheLayer(this IServiceCollection services, string connectionString)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = "AirportDistanceCalculator_";
        });

        services.AddSingleton<IRepositoryCache, RedisRepositoryCache>();
        services.Decorate<IAirportCodesRepository, CachingIataCodeRepository>();

        return services;
    }

    /// <summary>
    /// Add IATA code operations
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/></param>
    public static IServiceCollection AddIataCodeOperationsServices(this IServiceCollection services)
    {
        services.AddSingleton<IIataCodeValidator, IataCodeValidator>();
        services.AddSingleton<IDistanceCalculator, HaversineFormula>();

        return services;
    }
}