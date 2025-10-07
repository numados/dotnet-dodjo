# ✈️ Airport Distance Calculator

A modern .NET 8 REST API service that calculates the great-circle distance between airports using their IATA codes. This project demonstrates clean architecture principles, modern C# 12 features, and best practices for building microservices.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=c-sharp)
![License](https://img.shields.io/badge/license-MIT-blue.svg)

## 📋 Purpose

This is a **demonstration project** showcasing:

- **Modern C# 12 Features**: Primary constructors, collection expressions, file-scoped namespaces, record types
- **.NET 8 Capabilities**: Latest framework features and performance improvements
- **Clean Architecture**: Domain-driven design with clear separation of concerns (Domain, Implementation, WebApi layers)
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
┌─────────────────────────────────────┐
│   Airport.MeasureService.WebApi     │  ← Presentation Layer (Controllers, API)
├─────────────────────────────────────┤
│  Airport.Measure.Implementation     │  ← Implementation Layer (Services, Repositories)
├─────────────────────────────────────┤
│     Airport.Measure.Domain          │  ← Domain Layer (Entities, Interfaces)
└─────────────────────────────────────┘
```

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
- [Docker](https://www.docker.com/get-started) (optional, for containerized deployment)
- [Redis](https://redis.io/download) (optional, for distributed caching)

## 🚀 Getting Started

### Running Locally

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Cteleport.Airport
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
   cd Airport.MeasureService.WebApi
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
dotnet test Airport.Measure.Implementation.Tests/Airport.Measure.Implementation.Tests.csproj
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
Cteleport.Airport/
├── Airport.Measure.Domain/              # Domain layer
│   ├── Entities/                        # Domain entities (IataCode, LocationPoint, Distance)
│   ├── Exceptions/                      # Domain exceptions
│   ├── Repositories/                    # Repository interfaces
│   └── Services/                        # Service interfaces
├── Airport.Measure.Implementation/      # Implementation layer
│   ├── Exceptions/                      # Implementation-specific exceptions
│   ├── Repositories/                    # Repository implementations
│   │   ├── Cache/                       # Caching implementations (In-Memory, Redis)
│   │   └── Web/                         # Web-based repository
│   └── Services/                        # Service implementations
│       ├── DistanceCalculators/         # Haversine formula
│       └── Validators/                  # IATA code validator
├── Airport.Measure.Implementation.Tests/ # Unit tests
├── Airport.MeasureService.WebApi/       # Web API layer
│   ├── Controllers/                     # API controllers
│   ├── Extensions/                      # Service collection extensions
│   └── Properties/                      # Launch settings
├── docker-compose.yml                   # Docker Compose configuration
├── Dockerfile                           # Docker build configuration
└── README.md                            # This file
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

