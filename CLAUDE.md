# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Run the API (HTTP on :5268, HTTPS on :7276)
dotnet run

# Build
dotnet build

# Run tests (when test project exists)
dotnet test

# EF Core migrations
dotnet ef migrations add <MigrationName>
dotnet ef database update
dotnet ef migrations remove
```

The `.http` file at the root (`gstok-api.http`) can be used to test endpoints directly from VS Code with the REST Client extension.

## Architecture

This is a .NET 10 ASP.NET Core REST API following a layered Clean Architecture pattern. The data flow is:

**Request → Controller → Service → Repository → DbContext (PostgreSQL)**

Each layer lives in its own top-level folder:

| Folder | Role |
|---|---|
| `Controllers/` | Route handlers; thin — delegate immediately to a Service |
| `Services/` | Business logic; injected into controllers via interface |
| `Repositories/` | Data access abstraction over EF Core |
| `Models/` | EF Core entity classes |
| `DTOs/` | Request/response shapes exposed by the API |
| `Database/` | `DbContext` subclass and EF configuration |
| `Migrations/` | Auto-generated EF Core migrations |
| `Mappings/` | AutoMapper profiles (Model ↔ DTO) |
| `Validators/` | FluentValidation or similar input validators |
| `Middleware/` | Custom request pipeline components (e.g., error handling) |
| `Exceptions/` | Typed exception classes for domain errors |

Root namespace is `gstok_api`.

## Key dependencies

- **Npgsql.EntityFrameworkCore.PostgreSQL** — production database provider
- **Microsoft.EntityFrameworkCore.InMemory** — used for development/testing when no Postgres is available
- **Microsoft.AspNetCore.OpenApi** — OpenAPI schema exposed at `/openapi/v1.json` (dev only)

## Database

Configure the PostgreSQL connection string in `appsettings.Development.json` under `ConnectionStrings:DefaultConnection`. The DbContext class goes in `Database/`. Register it in `Program.cs` with:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

## Conventions

- Register services/repositories in `Program.cs` using the DI container (scoped lifetime for repositories and services).
- Controllers should only call service methods — no direct DB access.
- Return DTOs from controller endpoints, never raw EF entities.
- Use `[ApiController]` and `[Route("api/[controller]")]` on all controllers.

### Entity property naming

All entity properties use a 2-letter semantic prefix (PascalCase). See [README.md](README.md) for the full prefix table. Key prefixes:

| Prefix | Meaning | C# type |
|--------|---------|---------|
| `Nm` | Name | `string` |
| `Vl` | Monetary value | `decimal` |
| `Qt` | Quantity | `decimal`/`int` |
| `Pc` | Percentage/rate | `decimal` |
| `Dt` | Date | `DateOnly` |
| `Ts` | Timestamp (UTC) | `DateTime` |
| `St` | Status | `string`/`enum` |
| `Tp` | Type/category | `string`/`enum` |
| `Fl`/`In` | Boolean flag | `bool` |
| `Cd` | Business code | `string` |
| `Nr` | Numeric measurement (dimensions, counts) | `int` |
| `Sq` | Sequence/ordering position | `int` |

Primary keys use `Id` without a prefix. Foreign keys use `<EntityName>Id`. Navigation properties have no prefix.

## Related project

The frontend companion is `gstok-web` (Vite + TypeScript + Material UI) located at `../gstok-web`.