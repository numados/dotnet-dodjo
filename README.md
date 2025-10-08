## Repository Overview

This repository hosts a small collection of .NET projects and learning demos. Each project is self-contained with its own solution, documentation, and (where applicable) tests and Docker support.

- Purpose: Provide a space to explore modern .NET patterns, clean architecture, testing practices, and service design.
- How to use: Browse the projects below and open the linked README or solution files for setup and usage details.

---

## Projects

### 1) Airport Distance Calculator
Path: [airport-distance/](airport-distance/)

- Overview: A modern .NET 8 REST API that calculates great‑circle distance between airports (by IATA code), demonstrating clean architecture and best practices.
- Solution: [Airport.MeasureService.sln](airport-distance/Airport.MeasureService/Airport.MeasureService.sln)
- Docs: [airport-distance/README.md](airport-distance/README.md) (full feature list, getting started, configuration, API docs, tests)

Components inside the solution:
- API: [Airport.MeasureService.Api](airport-distance/Airport.MeasureService/Airport.MeasureService.Api) — ASP.NET Core Web API (Swagger, versioning, DI)
- Domain (Core): [Airport.MeasureService.Core](airport-distance/Airport.MeasureService/Airport.MeasureService.Core) — domain entities, interfaces, exceptions
- Implementation: [Airport.MeasureService.Implementation](airport-distance/Airport.MeasureService/Airport.MeasureService.Implementation) — services, repositories, caching
- Tests: [Airport.MeasureService.Implementation.Tests](airport-distance/Airport.MeasureService/Airport.MeasureService.Implementation.Tests) — unit tests

Quick links:
- Open solution: [Airport.MeasureService.sln](airport-distance/Airport.MeasureService/Airport.MeasureService.sln)
- Project README: [airport-distance/README.md](airport-distance/README.md)
- Docker Compose: [docker-compose.yml](airport-distance/Airport.MeasureService/docker-compose.yml)

---

### 2) Plugin System Reference Implementation
Path: [plugin-system-reference/](plugin-system-reference/)

- Overview: A production-ready, extensible plugin system framework for .NET 8 demonstrating reflection-based plugin discovery, parallel execution, and comprehensive testing. Perfect for building validation systems, data processing pipelines, or any extensible application architecture.
- Solution: [PluginSystem.sln](plugin-system-reference/PluginSystem.sln)
- Docs: [plugin-system-reference/README.md](plugin-system-reference/README.md) (architecture, usage examples, creating plugins, testing)

Components inside the solution:
- Core: [PluginSystem.Core](plugin-system-reference/PluginSystem.Core) — plugin infrastructure (interfaces, loader, executor, manager)
- Sample Plugins: [PluginSystem.DocumentValidation](plugin-system-reference/PluginSystem.DocumentValidation) — 4 validation plugins demonstrating best practices
- Demo App: [PluginSystem.Host](plugin-system-reference/PluginSystem.Host) — console application showing plugin system in action
- Tests: [PluginSystem.Core.Tests](plugin-system-reference/PluginSystem.Core.Tests) — 73 comprehensive unit tests

Key Features:
- 🔍 Automatic plugin discovery via reflection
- ⚡ Parallel execution support for high performance
- 🎯 Category-based filtering and priority ordering
- 📝 Rich result types with severity levels
- 🧪 Fully tested with 73 passing tests

Quick links:
- Open solution: [PluginSystem.sln](plugin-system-reference/PluginSystem.sln)
- Project README: [plugin-system-reference/README.md](plugin-system-reference/README.md)

---

## Getting Started

- Each project folder contains its own instructions. Start with the project README for prerequisites and run steps.
- For the Airport Distance Calculator, see [airport-distance/README.md](airport-distance/README.md) for quick start (CLI, Rider/VS, Docker).
- For the Plugin System, see [plugin-system-reference/README.md](plugin-system-reference/README.md) for architecture overview, usage examples, and how to create your own plugins.

## Contributing

Improvements and suggestions are welcome. Feel free to open an issue or PR for any of the projects contained here.
