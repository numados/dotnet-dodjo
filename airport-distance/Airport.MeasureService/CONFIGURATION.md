# Configuration Guide

This document explains how to configure the Airport Distance Calculator API for different environments and use cases.

## Quick Start (Default Configuration)

The application is **pre-configured** to work out-of-the-box with:
- ✅ **In-Memory Airport Repository** (no external API required)
- ✅ **In-Memory Cache** (no Redis required)
- ✅ **80+ major airports** worldwide

**Just run the application and it works!**

## Repository Configuration

The application supports two repository implementations:

### Option 1: In-Memory Repository (Default - Recommended for Development)

**Status**: ✅ **Currently Active**

**Features**:
- No external dependencies
- Fast and reliable
- Contains 80+ major airports worldwide
- Perfect for development and testing

**Airports Included**:
- **Europe**: AMS, LHR, LON, CDG, FRA, MAD, BCN, FCO, MUC, ZRH, VIE, CPH, ARN, OSL, HEL, DUB, LIS, ATH, IST, PRG, WAW, LGW, MAN, EDI, BHX, GLA
- **North America**: JFK, LAX, ORD, DFW, DEN, SFO, SEA, LAS, MIA, ATL, BOS, IAD, PHX, IAH, MSP, DTW, PHL, LGA, YYZ, YVR, YUL
- **Asia**: HND, NRT, PEK, PVG, HKG, SIN, ICN, BKK, KUL, DEL, BOM, DXB, DOH
- **Oceania**: SYD, MEL, BNE, AKL
- **South America**: GRU, GIG, EZE, SCL, BOG, LIM
- **Africa**: JNB, CPT, CAI, NBO

**Configuration** (in `Program.cs`):
```csharp
// Currently active
builder.Services.AddInMemoryIataCodeRepository();
```

**Pros**:
- ✅ No setup required
- ✅ No external API calls
- ✅ Fast and predictable
- ✅ Works offline

**Cons**:
- ❌ Limited to predefined airports
- ❌ No dynamic airport data updates

---

### Option 2: Web Repository (For Production with External API)

**Status**: ⚠️ **Currently Disabled**

**Features**:
- Fetches airport data from external API
- Dynamic and up-to-date
- Supports any IATA code

**Configuration** (in `Program.cs`):
```csharp
// Comment out in-memory repository
// builder.Services.AddInMemoryIataCodeRepository();

// Uncomment web repository
builder.Services.AddWebIataCodeRepository(
    builder.Configuration["Repository:Url"] 
    ?? throw new InvalidOperationException("Repository:Url configuration is missing")
);
```

**Required Settings** (in `appsettings.json`):
```json
{
  "Repository": {
    "Url": "https://your-airport-api.com/airports"
  }
}
```

**Expected API Response Format**:
```json
{
  "type": "airport",
  "location": {
    "lon": 4.76389,
    "lat": 52.30833
  }
}
```

**Pros**:
- ✅ Dynamic airport data
- ✅ Supports all IATA codes
- ✅ Always up-to-date

**Cons**:
- ❌ Requires external API
- ❌ Network dependency
- ❌ Potential latency

---

## Cache Configuration

The application supports two caching implementations:

### Option 1: In-Memory Cache (Default - Recommended for Development)

**Status**: ✅ **Currently Active**

**Features**:
- Simple and fast
- No external dependencies
- Automatic memory management
- Per-instance cache (not shared across instances)

**Configuration** (in `Program.cs`):
```csharp
// Currently active
builder.Services.AddInMemoryCacheForIataCodeRepository();
```

**Pros**:
- ✅ No setup required
- ✅ Fast access
- ✅ Simple configuration

**Cons**:
- ❌ Not shared across instances
- ❌ Lost on application restart
- ❌ Memory usage grows with cache size

---

### Option 2: Redis Cache (For Production/Distributed Systems)

**Status**: ⚠️ **Currently Disabled**

**Features**:
- Distributed cache (shared across instances)
- Persistent across restarts
- Scalable for production

**Configuration** (in `Program.cs`):
```csharp
// Comment out in-memory cache
// builder.Services.AddInMemoryCacheForIataCodeRepository();

// Uncomment Redis cache
builder.Services.AddRedisCacheForIataCodeRepository(
    builder.Configuration.GetConnectionString("Redis") 
    ?? throw new InvalidOperationException("Redis connection string is missing")
);
```

**Required Settings** (in `appsettings.json`):
```json
{
  "ConnectionStrings": {
    "Redis": "localhost:6379"
  }
}
```

**Start Redis** (Docker):
```bash
docker run -d -p 6379:6379 --name redis redis:latest
```

**Start Redis** (macOS with Homebrew):
```bash
brew services start redis
```

**Verify Redis**:
```bash
redis-cli ping
# Should return: PONG
```

**Pros**:
- ✅ Shared across instances
- ✅ Persistent
- ✅ Production-ready

**Cons**:
- ❌ Requires Redis server
- ❌ Additional infrastructure
- ❌ Network latency

---

## Configuration Combinations

### Development (Current Default)
```csharp
builder.Services.AddInMemoryIataCodeRepository();
builder.Services.AddInMemoryCacheForIataCodeRepository();
```
**Best for**: Local development, testing, demos

---

### Production with External API
```csharp
builder.Services.AddWebIataCodeRepository(builder.Configuration["Repository:Url"]!);
builder.Services.AddRedisCacheForIataCodeRepository(builder.Configuration.GetConnectionString("Redis")!);
```
**Best for**: Production deployment with external airport data API

---

### Hybrid (In-Memory Data + Redis Cache)
```csharp
builder.Services.AddInMemoryIataCodeRepository();
builder.Services.AddRedisCacheForIataCodeRepository(builder.Configuration.GetConnectionString("Redis")!);
```
**Best for**: Production with predefined airports and distributed caching

---

## Environment-Specific Configuration

### appsettings.Development.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  }
}
```

### appsettings.Production.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Repository": {
    "Url": "https://production-airport-api.com/airports"
  },
  "ConnectionStrings": {
    "Redis": "production-redis-host:6379"
  }
}
```

---

## Testing the Configuration

### Test In-Memory Repository
```bash
# Start the application
dotnet run --project Airport.MeasureService.Api

# Test with AMS to LON
curl "http://localhost:5135/api/v1/distance?from=AMS&to=LON"

# Expected response:
# {"distance":226.36,"unit":"miles"}
```

### Test with Swagger UI
1. Navigate to: http://localhost:5135/swagger
2. Expand `GET /api/v1/distance`
3. Click "Try it out"
4. Enter:
   - `from`: AMS
   - `to`: LON
5. Click "Execute"
6. Verify response: `{"distance":226.36,"unit":"miles"}`

---

## Troubleshooting

### Error: "Airport with IATA code 'XXX' not found"
**Cause**: Using in-memory repository with an airport not in the predefined list.

**Solution**:
1. Check available airports in `InMemoryAirportRepository.cs`
2. Or switch to web repository with external API
3. Or add the airport to the in-memory repository

### Error: "Repository:Url configuration is missing"
**Cause**: Web repository is enabled but URL is not configured.

**Solution**:
1. Add `Repository:Url` to `appsettings.json`
2. Or switch to in-memory repository

### Error: "Redis connection string is missing"
**Cause**: Redis cache is enabled but connection string is not configured.

**Solution**:
1. Add Redis connection string to `appsettings.json`
2. Ensure Redis is running
3. Or switch to in-memory cache

### Error: "JsonException: '<' is an invalid start of a value"
**Cause**: Web repository is receiving HTML instead of JSON from the external API.

**Solution**:
1. Verify the `Repository:Url` is correct
2. Test the API endpoint manually
3. Check API authentication/authorization
4. Or switch to in-memory repository

---

## Adding Custom Airports to In-Memory Repository

Edit `Airport.MeasureService.Implementation/Repositories/InMemoryAirportRepository.cs`:

```csharp
private static readonly Dictionary<string, LocationPoint> _airports = new()
{
    // Add your custom airport
    ["XXX"] = new LocationPoint(longitude, latitude),
    
    // Existing airports...
    ["AMS"] = new LocationPoint(4.76389, 52.30833),
    // ...
};
```

---

## Summary

**Current Configuration** (Out-of-the-box):
- ✅ In-Memory Repository (80+ airports)
- ✅ In-Memory Cache
- ✅ No external dependencies
- ✅ Ready to run immediately

**To Switch to Production**:
1. Configure external airport API URL
2. Set up Redis server
3. Update `Program.cs` to use web repository and Redis cache
4. Update `appsettings.Production.json` with production values

---

**For more information, see**:
- `RIDER_SETUP.md` - Running the application in JetBrains Rider
- `README.md` - General project documentation
- `Program.cs` - Service configuration

