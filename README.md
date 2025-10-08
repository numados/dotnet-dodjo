## Repository Overview

This repository hosts a small collection of .NET projects and learning demos. Each project is self-contained with its own solution, documentation, and (where applicable) tests and Docker support.

- Purpose: Provide a space to explore modern .NET patterns, clean architecture, testing practices, and service design.
- How to use: Browse the projects below and open the linked README or solution files for setup and usage details.

---

## Projects

### 1) Airport Distance Calculator
Path: [airport_distance/](airport_distance/)

- Overview: A modern .NET 8 REST API that calculates great‑circle distance between airports (by IATA code), demonstrating clean architecture and best practices.
- Solution: [Airport.MeasureService.sln](airport_distance/Airport.MeasureService/Airport.MeasureService.sln)
- Docs: [airport_distance/README.md](airport_distance/README.md) (full feature list, getting started, configuration, API docs, tests)

Components inside the solution:
- API: [Airport.MeasureService.Api](airport_distance/Airport.MeasureService/Airport.MeasureService.Api) — ASP.NET Core Web API (Swagger, versioning, DI)
- Domain (Core): [Airport.MeasureService.Core](airport_distance/Airport.MeasureService/Airport.MeasureService.Core) — domain entities, interfaces, exceptions
- Implementation: [Airport.MeasureService.Implementation](airport_distance/Airport.MeasureService/Airport.MeasureService.Implementation) — services, repositories, caching
- Tests: [Airport.MeasureService.Implementation.Tests](airport_distance/Airport.MeasureService/Airport.MeasureService.Implementation.Tests) — unit tests

Quick links:
- Open solution: [Airport.MeasureService.sln](airport_distance/Airport.MeasureService/Airport.MeasureService.sln)
- Project README: [airport_distance/README.md](airport_distance/README.md)
- Docker Compose: [docker-compose.yml](airport_distance/Airport.MeasureService/docker-compose.yml)

---

## Getting Started

- Each project folder contains its own instructions. Start with the project README for prerequisites and run steps.
- For the Airport Distance Calculator, see [airport_distance/README.md](airport_distance/README.md) for quick start (CLI, Rider/VS, Docker).

## Contributing

Improvements and suggestions are welcome. Feel free to open an issue or PR for any of the projects contained here.
