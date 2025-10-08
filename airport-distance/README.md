# ✈️ Airport Distance Calculator

A modern .NET 8 REST API service that calculates the great-circle distance between airports using their IATA codes. This project demonstrates clean architecture principles, modern C# 12 features, and best practices for building microservices.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=c-sharp)
![License](https://img.shields.io/badge/license-MIT-blue.svg)

## 📋 Purpose

This is a **demonstration project** showcasing:

- **Modern C# 12 Features**: Primary constructors, collection expressions, file-scoped namespaces, record types
- **.NET 8 Capabilities**: Latest framework features and performance improvements
- **Clean Architecture**: Domain-driven design with clear separation of concerns (Domain, Implementation, API layers)
- **Microservice Patterns**: Caching strategies, dependency injection, API versioning
- **Best Practices**: Comprehensive XML documentation, unit testing, Docker support, Swagger/OpenAPI integration

## ✨ Features

- 🌍 **Distance Calculation**: Calculate great-circle distance between any two airports using the Haversine formula
- 🔍 **IATA Code Validation**: Automatic validation of airport codes
- 💾 **Dual Caching Strategy**: 
  - In-memory caching for development
  - Redis distributed caching for production
- 📚 **API Versioning**: URL-based versioning for API evolution
- 📖 **Interactive API Documentation**: Swagger UI for easy API exploration
- 🐳 **Docker Support**: Containerized deployment with Docker Compose
- ✅ **Comprehensive Testing**: Unit tests with NUnit and Moq
- 📝 **Full XML Documentation**: Complete API documentation for IntelliSense

## 🏗️ Architecture Overview

The solution follows **Clean Architecture** (Onion Architecture) principles with three main layers:

```
┌──────────────────────────────────────────┐
│    Airport.MeasureService.Api            │  ← API Layer (Controllers, HTTP)
├──────────────────────────────────────────┤
│  Airport.MeasureService.Implementation   │  ← Implementation Layer (Services, Repositories)
├──────────────────────────────────────────┤
│    Airport.MeasureService.Core           │  ← Domain Layer (Domain Entities, Interfaces)
└──────────────────────────────────────────┘
```

**Layer Responsibilities:**

- **Domain Layer** (`Airport.MeasureService.Core`): Contains domain entities, interfaces, and domain exceptions. This is the innermost layer with no dependencies on other projects.
- **Implementation Layer** (`Airport.MeasureService.Implementation`): Contains concrete implementations of Domain interfaces, services, repositories, and caching strategies. Depends only on the Domain layer.
- **API Layer** (`Airport.MeasureService.Api`): Contains ASP.NET Core Web API controllers, handles HTTP requests/responses, and provides access to business logic. Depends on both Domain and Implementation layers.

**Key Design Patterns:**
- Repository Pattern
- Decorator Pattern (Caching)
- Dependency Injection
- Cache-Aside Pattern

## 🛠️ Technology Stack

- **Framework**: .NET 8.0
- **Language**: C# 12
- **API**: ASP.NET Core Web API
- **API Versioning**: Asp.Versioning 8.1.0
- **Caching**: 
  - In-Memory (Microsoft.Extensions.Caching.Memory)
  - Redis (StackExchange.Redis 8.0.0)
- **Documentation**: Swagger/Swashbuckle 6.8.1
- **Testing**: NUnit 4.2.2, Moq 4.20.72
- **Dependency Injection**: Scrutor 5.0.1 (for decorators)
- **Containerization**: Docker, Docker Compose

## 📦 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (8.0.0 or later)
- [JetBrains Rider](https://www.jetbrains.com/rider/) or [Visual Studio 2022](https://visualstudio.microsoft.com/) (optional, for IDE support)
- [Docker](https://www.docker.com/get-started) (optional, for containerized deployment)
- [Redis](https://redis.io/download) (optional, for distributed caching - **not required by default**)

> **✨ Quick Start**: The application works **out-of-the-box** with an in-memory airport repository (80+ airports) and in-memory caching. No external dependencies or configuration required!

## 🚀 Getting Started

### Option 1: Running in JetBrains Rider (Recommended)

1. Open the solution in Rider: `File → Open → Airport.MeasureService.sln`
2. Select run configuration: `Airport.MeasureService.Api`
3. Click Run (▶️) or press `Shift+F10`
4. Browser opens automatically to Swagger UI

📖 **For detailed Rider setup, see [RIDER_SETUP.md](RIDER_SETUP.md)**

---

### Option 2: Running from Command Line

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Airport.MeasureService
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run the API**
   ```bash
   cd Airport.MeasureService.Api
   dotnet run
   ```

5. **Access Swagger UI**
   
   Open your browser and navigate to: `https://localhost:7056/swagger`

### Running with Docker Compose

1. **Build and start services**
   ```bash
   docker-compose up --build
   ```

2. **Access the API**
   
   API: `http://localhost:8081`
   
   Swagger UI: `http://localhost:8081/swagger`

3. **Stop services**
   ```bash
   docker-compose down
   ```

## ⚙️ Configuration

The application uses **configuration-based setup** - no code changes required!

### Current Default (Out-of-the-Box)
- ✅ **Static Airport Data Source** (80+ major airports worldwide)
- ✅ **In-Memory Cache Layer** (no Redis required)
- ✅ **No external dependencies**
- ✅ **Configuration via appsettings.json**

When you start the application, you'll see:
```
✓ Using Static Airport Data Source (80+ predefined airports)
✓ Using In-Memory Cache Layer
```

### Available Airports
AMS, LHR, LON, CDG, FRA, MAD, BCN, FCO, MUC, ZRH, VIE, CPH, ARN, OSL, HEL, DUB, LIS, ATH, IST, PRG, WAW, JFK, LAX, ORD, DFW, DEN, SFO, SEA, LAS, MIA, ATL, BOS, IAD, PHX, IAH, MSP, DTW, PHL, LGA, YYZ, YVR, YUL, HND, NRT, PEK, PVG, HKG, SIN, ICN, BKK, KUL, DEL, BOM, DXB, DOH, SYD, MEL, BNE, AKL, GRU, GIG, EZE, SCL, BOG, LIM, JNB, CPT, CAI, NBO, LGW, MAN, EDI, BHX, GLA, and more...

### Switching to Production Configuration

Edit `appsettings.Production.json` (no code changes needed):

```json
{
  "AirportService": {
    "DataSource": {
      "Type": "Web",
      "WebApiUrl": "https://your-api.com/airports"
    },
    "Cache": {
      "Type": "Redis",
      "RedisConnection": "your-redis:6379"
    }
  }
}
```

Then run with:
```bash
export ASPNETCORE_ENVIRONMENT=Production
dotnet run --project Airport.MeasureService.Api
```

📖 **For detailed configuration options, see [CONFIGURATION.md](CONFIGURATION.md)**

---

## 📖 API Documentation

### Calculate Distance

**Endpoint**: `GET /api/v1/distance`

**Query Parameters**:
- `from` (required): Origin airport IATA code (3 letters, e.g., "AMS")
- `to` (required): Destination airport IATA code (3 letters, e.g., "JFK")

**Example Request**:
```bash
curl -X GET "https://localhost:7056/api/v1/distance?from=AMS&to=JFK"
```

**Example Response**:
```json
{
  "from": {
    "value": "AMS"
  },
  "to": {
    "value": "JFK"
  },
  "distance": {
    "value": 3639.17,
    "unit": "miles"
  }
}
```

### Interactive Documentation

Visit `/swagger` when the application is running to explore the API interactively with Swagger UI.

## 🧪 Running Tests

### Run all tests
```bash
dotnet test
```

### Run tests with detailed output
```bash
dotnet test --verbosity normal
```

### Run tests in a specific project
```bash
dotnet test Airport.MeasureService.Implementation.Tests/Airport.MeasureService.Implementation.Tests.csproj
```

**Test Coverage**: 14 unit tests covering:
- Distance calculation (Haversine formula)
- IATA code validation
- Repository caching
- JSON parsing
- HTTP service

## 🐳 Docker Support

### Dockerfile

The project includes a multi-stage Dockerfile for optimized production builds:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# ... build steps ...
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
# ... runtime configuration ...
```

### Docker Compose

The `docker-compose.yml` orchestrates the API and Redis services:

```yaml
services:
  api:
    build: .
    ports:
      - "8081:80"
  redis:
    image: redis:latest
    ports:
      - "6379:6379"
```

## 📁 Project Structure

```
Airport.MeasureService/
├── Airport.MeasureService.Core/                    # Domain layer
│   ├── Entities/                                   # Domain entities
│   │   ├── Codes/                                  # IATA code entity
│   │   └── Locations/                              # Location-related entities (LocationPoint, Distance, Direction)
│   ├── Exceptions/                                 # Domain exceptions
│   ├── Repositories/                               # Repository interfaces
│   └── Services/                                   # Service interfaces
├── Airport.MeasureService.Implementation/          # Implementation layer
│   ├── Exceptions/                                 # Implementation-specific exceptions
│   ├── Repositories/                               # Repository implementations
│   │   ├── Cache/                                  # Caching implementations (In-Memory, Redis)
│   │   └── Web/                                    # Web-based repository
│   │       ├── Http/                               # HTTP client abstraction
│   │       └── Json/                               # JSON parsing
│   └── Services/                                   # Service implementations
│       ├── DistanceCalculators/                    # Haversine formula implementation
│       └── Validators/                             # IATA code validator
├── Airport.MeasureService.Implementation.Tests/    # Unit tests
│   ├── DistanceCalculators/                        # Distance calculation tests
│   ├── Http/                                       # HTTP service tests
│   ├── Parsers/                                    # JSON parser tests
│   ├── Repositories/                               # Repository tests
│   └── Validators/                                 # Validator tests
├── Airport.MeasureService.Api/                     # API layer (Presentation)
│   ├── Controllers/                                # API controllers
│   ├── Extensions/                                 # Service collection extensions
│   └── Properties/                                 # Launch settings
├── docker-compose.yml                              # Docker Compose configuration
├── Dockerfile                                      # Docker build configuration (in Api project)
└── README.md                                       # This file
```

## 🤝 Contributing

This is a demonstration project, but suggestions and improvements are welcome! Feel free to open an issue or submit a pull request.

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🙏 Acknowledgments

- Haversine formula for great-circle distance calculation
- Clean Architecture principles by Robert C. Martin
- .NET community for excellent tools and libraries

---

**Built with ❤️ using .NET 8 and C# 12**

