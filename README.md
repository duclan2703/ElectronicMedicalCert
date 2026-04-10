Here's the improved `README.md` file, incorporating the new content while maintaining the existing structure and information:

# Elektronicke Posudky (ElectronicMedicalCert)

This repository implements the Elektronicke posudky API. It follows a clean, layered architecture with separation of concerns between Web, Application, Infrastructure, and Domain layers.

## Requirements

- .NET 8 SDK
- Visual Studio 2022 or VS Code
- SQL Server for production (LocalDB or Docker for development)

## Quick run (development)

1. Configure settings: open `appsettings.json` and adjust `ConnectionStrings:DefaultConnection` and `Auth:SigningKey` or set environment variables (recommended):
   - `ConnectionStrings__DefaultConnection`
   - `Jwt__Key` or `Auth__SigningKey`

2. Run the application from the IDE or CLI:

   dotnet run --project ElectronicMedicalCert

The API is hosted under path base `/elektronickePosudky`.

Swagger UI is enabled in Development at `/elektronickePosudky/swagger`.

## Running with Docker + SQL Server (example)

- Start SQL Server (Docker):

   docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Your_password123' -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest

- Update `appsettings.json` to point to `Server=host.docker.internal,1433;User Id=sa;Password=Your_password123;Database=ElectronicMedicalCert;TrustServerCertificate=True` or set `ConnectionStrings__DefaultConnection`.

- Run the app.

## Migrations & Database

- **Development/Testing**: By default, the app will auto-migrate when running in Development or Testing or when `Database:AutoMigrate` is set to true.
- **Production**: Do NOT rely on auto-migration. Apply migrations as part of your deployment pipeline using `dotnet ef database update` or generated SQL scripts. The app will detect pending migrations and fail fast in Production to avoid silent schema mismatches.

Configuration key:

- `Database:AutoMigrate` (bool) — when true, the app will apply pending migrations at startup (use with caution in non-dev environments).

## Configuration keys

Important keys expected by the app (can be set via `appsettings.json`, `appsettings.{Environment}.json`, or environment variables):

- `ConnectionStrings:DefaultConnection` — A valid SQL Server connection string.
- `Auth:SigningKey` (or `Jwt:Key`) — Symmetric signing key for JWTs (base64 or plain string). Required in non-testing environments.
- `Database:AutoMigrate` — Controls auto-applying EF Core migrations at startup.
- `Serilog` settings — Logging configuration.

**Security note**: Keep signing keys and production connection strings in a secret store or environment variables, not in source control.

## Architecture overview

- `ElectronicMedicalCert` (Web)
- Program startup, controllers, middleware, localization, authentication, Swagger.
- `ElectronicMedicalCert.Application`
- MediatR handlers/queries/commands, DTOs, validation (FluentValidation), pipeline behaviors.
- `ElectronicMedicalCert.Infrastructure`
- EF Core `ElectronicMedicalCertDbContext`, data access, DI setup.
- `ElectronicMedicalCert.Domain`
- Entity models and domain types.

### Cross-cutting concerns

- Serilog for structured logging and request logging.
- `CorrelationIdMiddleware` for request correlation.
- `ProblemDetailsMiddleware` centralizes exception -> RFC7807-like problem responses.
- `ModelStateValidationFilter` and a validation pipeline behavior ensure consistent validation and error shaping.

## Error handling

- Validation errors are returned as `application/problem+json` payloads with field-level details.
- Domain errors map to HTTP status codes using custom exceptions (NotFound, Conflict, Forbidden) handled by `ProblemDetailsMiddleware`.
- Unhandled exceptions are logged and translated to a 500 problem response with a correlation id.

## Assumptions

- Tests will set the environment name to `Testing` to bypass strict JWT validation.
- The signing key must be configured for non-testing environments.
- Production deployments will manage DB migrations explicitly unless `Database:AutoMigrate` is intentionally enabled.
- Localization resources are stored in `Resources` and used by the ProblemDetails and validation messages.

## Operational recommendations

- Rotate and store `Auth` signing keys in a secrets manager.
- Run migrations in CI/CD or as a controlled deployment step.
- Enable structured logging sink (files, Seq, or ELK) for production observability.
- Add health checks and metrics endpoints for orchestration platforms.

## Contributing

Follow the repository's coding standards and ensure new features include tests for middleware, validation, and data access.

---

This version maintains the original structure while enhancing clarity and coherence. Each section is clearly defined, and the flow of information is logical, making it easier for users to understand and navigate the README.