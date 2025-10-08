using Airport.MeasureService.Api.Extensions;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

// ADD SERVICES
builder.Services.AddIataCodeOperationsServices();

// Configure Airport Data Source (based on appsettings.json)
var dataSourceType = builder.Configuration["AirportService:DataSource:Type"];
switch (dataSourceType?.ToLowerInvariant())
{
    case "static":
        builder.Services.AddStaticAirportDataSource();
        builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Information);
        Console.WriteLine("✓ Using Static Airport Data Source (80+ predefined airports)");
        break;

    case "web":
        var webApiUrl = builder.Configuration["AirportService:DataSource:WebApiUrl"]
            ?? throw new InvalidOperationException("AirportService:DataSource:WebApiUrl is required when DataSource:Type is 'Web'");
        builder.Services.AddWebAirportDataSource(webApiUrl);
        Console.WriteLine($"✓ Using Web Airport Data Source: {webApiUrl}");
        break;

    default:
        throw new InvalidOperationException(
            $"Invalid AirportService:DataSource:Type '{dataSourceType}'. Valid values are 'Static' or 'Web'.");
}

// Configure Caching Layer (based on appsettings.json)
var cacheType = builder.Configuration["AirportService:Cache:Type"];
switch (cacheType?.ToLowerInvariant())
{
    case "inmemory":
        builder.Services.AddInMemoryCacheLayer();
        Console.WriteLine("✓ Using In-Memory Cache Layer");
        break;

    case "redis":
        var redisConnection = builder.Configuration["AirportService:Cache:RedisConnection"]
            ?? throw new InvalidOperationException("AirportService:Cache:RedisConnection is required when Cache:Type is 'Redis'");
        builder.Services.AddRedisCacheLayer(redisConnection);
        Console.WriteLine($"✓ Using Redis Cache Layer: {redisConnection}");
        break;

    default:
        throw new InvalidOperationException(
            $"Invalid AirportService:Cache:Type '{cacheType}'. Valid values are 'InMemory' or 'Redis'.");
}


// HELPERS
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

    // Include XML comments for better API documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Airport Distance Calculator API",
        Version = "v1",
        Description = "A REST API service that calculates the great-circle distance between airports using their IATA codes.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Airport Distance Calculator",
            Url = new Uri("https://github.com/yourusername/airport-distance-calculator")
        }
    });
});
//

// API versioning
builder.Services.AddApiVersioning(options =>
    {
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddMvc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Airport Distance Calculator API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Airport Distance Calculator API";
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();