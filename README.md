# Project & Task Management API

A production-quality REST API for managing Projects and Tasks, built with **.NET 9**, **Clean Architecture**, **CQRS + MediatR**, and **ASP.NET Core Identity + JWT Authentication**.

This was built as a technical assessment for a Backend .NET Developer position. Most of bonus requirements were implemented: CQRS/MediatR, Generic Response Wrapper, and URL-segment API Versioning.

---

## Table of Contents

- [Tech Stack](#tech-stack)
- [Architecture Overview](#architecture-overview)
- [Project Structure](#project-structure)
- [Setup & Run Instructions](#setup--run-instructions)
- [Authentication & Authorization Model](#authentication--authorization-model)
- [API Versioning](#api-versioning)
- [API Endpoints](#api-endpoints)
- [Architecture Decisions & Trade-offs](#architecture-decisions--trade-offs)
- [Known Gaps & Future Improvements](#known-gaps--future-improvements)
- [Testing](#testing)

---

## Tech Stack

| Concern | Technology |
|---|---|
| Framework | .NET 9, ASP.NET Core Web API |
| Data Access | Entity Framework Core, SQL Server |
| CQRS / Mediator | MediatR |
| Validation | FluentValidation (via MediatR pipeline behavior) |
| Auth | ASP.NET Core Identity, JWT Bearer + Refresh Tokens |
| API Versioning | Asp.Versioning (URL segment: `/api/v1/...`) |
| API Docs | Swagger / OpenAPI |

---

## Architecture Overview

The solution follows **Clean Architecture** with a strict dependency direction:

```
API ──────► Application ──────► Domain
 │                                  ▲
 ▼                                  │
Infrastructure ────────────────────┘
 │
 ▼
Persistence ────────────────────────┘
```

- **Domain** has zero external dependencies — plain entities, enums, and constants only.
- **Application** contains all business/use-case logic, organized as **CQRS commands and queries using Vertical Slice Architecture** (each feature's command/query/handler/validator live together, not split across type-based folders).
- **Infrastructure** and **Persistence** are separated deliberately: Persistence handles EF Core/data access; Infrastructure handles cross-cutting external concerns (JWT generation, current-user resolution). They have different reasons to change, so they're different projects.
- **API** is the composition root — controllers are thin, translating HTTP requests into MediatR commands/queries and nothing more.

### Why Vertical Slices over type-based layering

Organizing `Application/Features/{FeatureName}/{Commands|Queries}/{UseCase}/` keeps everything needed for one use case — command, handler, validator, response — in a single folder. Implementing or debugging a feature means working in one place, not hunting across parallel `Commands/`, `Handlers/`, `Validators/` trees. It also scales naturally if a feature ever needs to be extracted into its own service.

---

## Project Structure

```
ProjectManagement.sln
├── ProjectManagement.Domain/          # Entities, enums, constants — no dependencies
├── ProjectManagement.Application/     # CQRS use cases, validation, interfaces, DTOs
├── ProjectManagement.Infrastructure/  # JWT generation, current-user service
├── ProjectManagement.Persistence/     # EF Core DbContext, repositories, migrations
├── ProjectManagement.API/             # Controllers, middleware, composition root
└── ProjectManagement.UnitTests/       # Handler & validator tests (xUnit/Moq/FluentAssertions)
```

> Note: projects sit at the solution root (no `src/` subfolder), per the actual scaffolded layout.

---

## Setup & Run Instructions

### Prerequisites

- .NET 9 SDK
- SQL Server (LocalDB is sufficient) or a SQL Server connection string you control
- (Optional) Postman, or just use Swagger UI

### 1. Clone and restore

```bash
git clone <repo-url>
cd ProjectManagement
dotnet restore
```

### 2. Configure the database connection

Edit `ProjectManagement.API/appsettings.json` (or override via User Secrets / environment variables) with your SQL Server connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProjectManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 3. Configure the JWT secret (required — the app will not run without this)

The JWT signing key is **not committed to source control**. Set it via User Secrets:

```bash
cd ProjectManagement.API
dotnet user-secrets init
dotnet user-secrets set "JwtSettings:SecretKey" "<a-random-32-character-or-longer-string>"
cd ..
```

> In production, this would be sourced from Azure Key Vault or environment variables, not User Secrets — User Secrets is a local-development-only mechanism.

### 4. Apply migrations

```bash
dotnet ef database update --project ProjectManagement.Persistence --startup-project ProjectManagement.API
```

### 5. Run the API

```bash
dotnet run --project ProjectManagement.API
```

On first startup, the application seeds the `Admin` and `Member` roles automatically. Swagger UI will be available at `https://localhost:<port>/swagger`.

### 6. Create your first Admin user (manual step)

Registration always assigns the `Member` role by default (see [Authentication & Authorization Model](#authentication--authorization-model) for why). To test Admin behavior, register normally via `/api/v1/auth/register`, then manually promote that user to `Admin` in the database (`AspNetUserRoles` table), or use the optional admin-seeding script if included.

---

## Authentication & Authorization Model

- **JWT access tokens** (15-minute expiry) + **rotating refresh tokens** (7-day expiry, stored in DB).
- On every refresh, the old token is revoked and chained to its replacement (`ReplacedByToken`), which limits the blast radius of a stolen refresh token — reuse of a revoked token is detectable.
- Two roles: **Admin** and **Member**.
  - **Member**: can only create/view/update/delete their own Projects and Tasks.
  - **Admin**: can view and manage all users' Projects and Tasks.
- Role-based authorization is **not just `[Authorize(Roles=...)]`** — ownership is enforced inside command/query handlers (e.g., a Member attempting to access another user's Project receives a `403 Forbidden`, not just a role check at the endpoint).
- `RegisterCommand` intentionally has **no `Role` field** — every self-registered user becomes `Member`. This prevents privilege escalation via the registration endpoint.
- Login returns an identical error message for "user not found" and "wrong password" to prevent email enumeration.

---

## API Versioning

All endpoints are versioned via URL segment: `/api/v1/...`. This was chosen for discoverability and ease of testing (visible directly in the URL, no extra headers needed in Swagger/Postman). Versioning infrastructure is in place from day one so future breaking changes don't disrupt existing clients, even though only `v1` currently exists.

---

## API Endpoints

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| POST | `/api/v1/auth/register` | None | Register a new user (always `Member` role) |
| POST | `/api/v1/auth/login` | None | Login, returns access + refresh token |
| POST | `/api/v1/auth/refresh-token` | None | Exchange a valid refresh token for a new pair |
| POST | `/api/v1/projects` | JWT | Create a project |
| GET | `/api/v1/projects` | JWT | List projects (own, or all if Admin) — paginated |
| GET | `/api/v1/projects/{id}` | JWT | Get a project by id (ownership enforced) |
| PUT | `/api/v1/projects/{id}` | JWT | Update a project (ownership enforced) |
| DELETE | `/api/v1/projects/{id}` | JWT | Delete a project (ownership enforced) |
| POST | `/api/v1/tasks` | JWT | Create a task under a project |
| GET | `/api/v1/tasks/by-project/{projectId}` | JWT | List tasks for a project — paginated |
| PATCH | `/api/v1/tasks/{taskId}/status` | JWT | Update a task's status |
| DELETE | `/api/v1/tasks/{taskId}` | JWT | Delete a task |

All responses are wrapped in a consistent envelope:

```json
{
  "succeeded": true,
  "message": "Project created.",
  "data": { ... },
  "errors": null
}
```

---

## Architecture Decisions & Trade-offs

This section documents deliberate decisions and their reasoning, so the "why" behind the code is explicit rather than left for a reviewer to infer.

**Simple POCO domain entities, not rich domain models.**
The domain here (Projects/Tasks with status and priority) has minimal intrinsic business logic — no complex state machines or cross-entity invariants. Business logic lives in command/query handlers rather than entity methods. If the domain grew more complex business rules tied to entity lifecycle, richer domain models would be the right next step.

**Generic Repository + Unit of Work alongside EF Core.**
EF Core's `DbContext` already functions as a Unit of Work, and `DbSet<T>` already provides repository-like querying — wrapping it again is a known anti-pattern when done carelessly ("repository over repository"). The justification here is decoupling: Application has zero reference to `Microsoft.EntityFrameworkCore`, keeping it testable via mocks and free of any persistence-technology dependency. Repositories are kept intentionally narrow and specific (e.g., `GetAllByUserIdAsync`), not a single bloated generic interface with unused methods. `IQueryable` never crosses the repository boundary into Application.

**`UserManager`/`RoleManager` injected directly into Auth handlers, bypassing the Unit of Work.**
ASP.NET Core Identity's managers are already a well-tested abstraction over user CRUD. Wrapping them in a custom repository would be redundant pass-through with no benefit. This is a deliberate, documented exception: Identity-managed entities go through Identity's own APIs; the project's own entities (RefreshToken, Project, Task) go through the custom Repository + UoW.

**`ApplicationUser : IdentityUser` lives in the Domain layer.**
Strictly, this means Domain has a dependency on the `Microsoft.AspNetCore.Identity` package, which is a minor deviation from "Domain has zero external dependencies." The alternative — maintaining a separate plain `User` entity in Domain and mapping it to/from `ApplicationUser` in Persistence — adds real mapping complexity for no practical benefit, since `ApplicationUser` doesn't participate in core business logic beyond being referenced by `UserId` foreign keys. This is a conscious, pragmatic trade-off, not an oversight.

**Unit tests only — no integration tests.**
Given the assessment's time constraint, integration tests were deprioritized in favor of completing all CQRS/Auth/Versioning/RBAC bonus requirements. Unit tests cover command/query handler logic and FluentValidation rules (mocking repository/UnitOfWork interfaces via Moq). This means EF Core query correctness, middleware behavior, and full request/response pipeline integration are **not** covered by automated tests — see [Known Gaps](#known-gaps--future-improvements).

**Offset-based pagination (`Skip`/`Take`), not keyset pagination.**
Offset pagination is simpler and sufficient at this data scale. Keyset pagination solves a performance problem (deep-page `OFFSET` cost) that doesn't materialize until datasets are orders of magnitude larger than what this system would realistically hold. `PageSize` is capped server-side to prevent a client from requesting an unbounded result set.

**Manual DTO mapping instead of AutoMapper.**
With only two simple, 1:1 entity-to-DTO mappings (`Project`→`ProjectDto`, `ProjectTask`→`TaskDto`), explicit `new ProjectDto(...)` construction is more transparent and fails at compile time if a property is renamed, rather than relying on convention-based runtime mapping for a marginal reduction in line count. AutoMapper was evaluated and deliberately not introduced for this reason.

**Custom `ApiResponse<T>` wrapper instead of `ProblemDetails`.**
ASP.NET Core's built-in `ProblemDetails` is the framework-idiomatic way to shape error responses. A custom wrapper was chosen instead so that **success and error responses share one consistent envelope shape**, which is simpler for API clients to parse than mixing `ProblemDetails` for errors with a bespoke shape for success.

---

## Known Gaps & Future Improvements

- **No integration tests.** Unit tests cover handler/validator logic via mocks; they do not exercise the real database, middleware pipeline, or full HTTP request/response cycle. Given more time, an integration test project using `WebApplicationFactory` + a test database (or Testcontainers) would close this gap.
- **No automated Admin-seeding.** The first Admin user must be promoted manually in the database; there's no self-service or seeded default Admin account.
- **JWT secret management is local-only.** User Secrets is appropriate for development; production deployment would require Azure Key Vault or environment-variable-based secret injection.
- **No rate limiting** on auth endpoints (login/register), which would be a reasonable production hardening step against brute-force attempts.

---

## Testing

Run all unit tests:

```bash
dotnet test
```

Tests cover:
- Command/query handler logic (happy path + failure path) via Moq-mocked `IUnitOfWork`/repositories
- FluentValidation validator rules
- The ownership-enforcement guard (`AuthorizationHelper`), explicitly verifying a non-owner Member is blocked from accessing another user's resources