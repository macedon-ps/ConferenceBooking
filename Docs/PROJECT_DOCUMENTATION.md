# ConferenceBooking — Project Documentation

## 1. Project Purpose

**ConferenceBooking** is an ASP.NET Core Web API for managing conference halls, bookings and additional services.

The API solves the following business tasks:

- managing conference halls;
- managing additional services;
- searching for available halls;
- creating and managing bookings;
- preventing booking conflicts;
- calculating rental costs;
- generating business reports and analytics.

The main business goal is to allow clients to find a suitable conference hall, book it for a specified period and calculate the total rental cost depending on the booking time and selected services.

---

## 2. Architecture

The project uses **Clean Architecture**.

```text
ConferenceBooking.sln
│
├── src/
│   ├── ConferenceBooking.Api
│   ├── ConferenceBooking.Application
│   ├── ConferenceBooking.Domain
│   └── ConferenceBooking.Infrastructure
│
└── tests/
    └── ConferenceBooking.Tests
```

### Project layers

#### ConferenceBooking.Api

Responsible for the HTTP API layer:

- Controllers
- Middleware
- API models
- dependency injection configuration
- Swagger/OpenAPI configuration
- application startup

#### ConferenceBooking.Application

Contains application-level business orchestration:

- DTOs
- application interfaces
- application services
- booking cost calculation
- reports and analytics

#### ConferenceBooking.Domain

Contains the core domain model:

- entities
- domain interfaces
- domain exceptions
- domain models

Main domain entities:

- `Hall`
- `Service`
- `Booking`
- `HallService`
- `BookingService`

#### ConferenceBooking.Infrastructure

Responsible for data access and infrastructure:

- Entity Framework Core
- `DbContext`
- database configurations
- repositories
- report repository
- database initialization / seed data

#### ConferenceBooking.Tests

Contains automated unit tests for domain and application logic.

Current test suite:

**69 Unit Tests**

---

## 3. Dependency Direction

The main dependency direction is:

```text
Api
 ↓
Application
 ↓
Domain
 ↑
Infrastructure

Tests
 ├──→ Application
 └──→ Domain
```

The Domain layer remains independent from infrastructure and API concerns.

---

## 4. Data Access

The project uses **Entity Framework Core** with **SQL Server**.

Main repository abstractions include:

```text
IHallRepository
IServiceRepository
IBookingRepository
IReportRepository
IUnitOfWork
```

Repositories provide operations for halls, services, bookings and analytical data.

---

## 5. Domain Model

The main relationships are:

```text
Hall
 │
 ├── HallService ── Service
 │
 └── Booking
       │
       └── BookingService ── Service
```

A hall can have multiple additional services.

A booking belongs to a hall and can contain multiple selected services.

The many-to-many relationships are represented by explicit linking entities:

- `HallService`;
- `BookingService`.

---

## 6. Hall Management

The API supports:

- creating halls;
- retrieving halls;
- updating halls;
- deleting halls;
- searching for available halls;
- assigning services to halls.

A hall contains:

- unique identifier;
- name;
- capacity;
- hourly rental rate;
- available services.

Business validation includes:

- capacity must be greater than zero;
- hourly rate cannot be negative.

---

## 7. Service Management

Additional services are managed independently from halls.

Example services from the original requirements:

| Service | Price |
|---|---:|
| Projector | 500 |
| Wi-Fi | 300 |
| Sound | 700 |

Each service contains:

- unique identifier;
- name;
- price.

---

## 8. Booking Management

A booking contains:

- hall identifier;
- start time;
- end time;
- selected services;
- calculated total cost.

Before creating a booking, the application checks whether the hall already has a conflicting booking.

A booking must have a valid time range:

```text
startTime < endTime
```

The API prevents overlapping bookings for the same hall.

---

## 9. Hall Availability

The API supports searching for available halls by:

- requested time period;
- required capacity.

The availability query excludes halls with conflicting bookings.

This allows the client to find halls that can actually be booked for the requested period.

---

## 10. Booking Cost Calculation

Booking cost is calculated by a dedicated application component:

```text
IBookingCostCalculator
        ↓
BookingCostCalculator
```

The calculation separates the hall cost from the cost of additional services.

The result contains:

```text
HallCost
TotalCost
```

The booking period is divided into tariff segments and the appropriate coefficient is applied to each segment.

### Tariff Zones

| Period | Coefficient | Effect |
|---|---:|---:|
| 06:00–09:00 | 0.90 | −10% |
| 09:00–12:00 | 1.00 | Standard |
| 12:00–14:00 | 1.15 | +15% |
| 14:00–18:00 | 1.00 | Standard |
| 18:00–23:00 | 0.80 | −20% |

The final price consists of:

```text
Hall Cost + Additional Services Cost = Total Booking Cost
```

The calculation supports bookings crossing multiple tariff zones.

---

## 11. Validation

Validation protects the domain and application from invalid input.

Examples:

- hall name cannot be empty;
- hall capacity must be greater than zero;
- hall hourly rate cannot be negative;
- service name cannot be empty;
- service price cannot be negative;
- booking hall ID cannot be empty;
- booking start time must be earlier than end time;
- booking total cost cannot be negative;
- duplicate services cannot be added to a hall or booking.

Report periods are also validated:

```text
from < to
```

---

## 12. Exception Handling

The API uses centralized exception handling middleware.

Application and domain exceptions are processed centrally and converted into consistent HTTP error responses.

The API handles, among others:

- validation errors;
- not found errors;
- business rule violations;
- booking conflicts;
- invalid date ranges;
- unexpected server errors.

This keeps controllers focused on HTTP interaction instead of repetitive exception handling.

---

## 13. Error Response

The API uses a consistent error response model.

A typical response is:

```json
{
  "statusCode": 400,
  "message": "..."
}
```

This makes API errors predictable for clients and allows Swagger to describe error responses consistently.

---

## 14. Logging

Application logging is implemented using `ILogger<T>`.

Logging covers important application events and operational diagnostics, including:

- hall creation;
- hall updates;
- hall deletion;
- not-found situations;
- failed deletion attempts;
- HTTP request processing;
- unhandled exceptions.

The logging strategy covers both HTTP/infrastructure events and application business events.

---

## 15. Security Hardening

Security hardening focuses on protecting the API from invalid input and invalid business operations.

Implemented protections include:

- validation of incoming data;
- validation of domain entities;
- protection against invalid identifiers;
- protection against invalid numeric values;
- validation of booking date ranges;
- prevention of conflicting bookings;
- centralized exception handling;
- consistent API error responses;
- avoiding exposure of internal exception details through API responses.

---

## 16. Reports / Analytics

The project includes a dedicated reporting subsystem.

Architecture:

```text
ReportsController
       ↓
IReportApplicationService
       ↓
ReportApplicationService
       ↓
IReportRepository
       ↓
EF Core / SQL Server
```

### 16.1 Booking Summary

Returns:

- total number of bookings;
- total booked hours;
- total revenue;
- average booking cost.

Example:

```http
GET /api/reports/bookings?from=2026-01-01&to=2026-02-01
```

### 16.2 Hall Utilization

Returns:

- hall ID;
- hall name;
- booking count;
- total booked hours;
- total revenue.

Example:

```http
GET /api/reports/halls?from=2026-01-01&to=2026-02-01
```

### 16.3 Popular Services

Returns:

- service ID;
- service name;
- usage count.

Example:

```http
GET /api/reports/services?from=2026-01-01&to=2026-02-01
```

The reporting period is validated and `from` must be earlier than `to`.

---

## 17. Swagger / OpenAPI

Swagger is used for API documentation and manual API testing.

Swagger provides:

- available API endpoints;
- request models;
- response models;
- HTTP status codes;
- validation/error responses;
- XML documentation.

Swagger UI is available at:

```text
https://localhost:<port>/swagger
```

---

## 18. Unit Tests

The project contains automated unit tests using **xUnit**.

Current test suite:

**69 Unit Tests**

The tests cover important domain and application business rules.

The initial domain test group covers the `Hall` entity and validates scenarios such as:

- valid hall creation;
- invalid hall names;
- invalid capacity;
- invalid hourly rate;
- hall updates;
- service management.

The test suite protects business rules from regressions during further development.

---

## 19. Database

The project uses:

- SQL Server;
- Entity Framework Core;
- `ConferenceBookingDbContext`;
- repositories;
- database configurations;
- migrations and seed data.

### LocalDB

For local development, SQL Server LocalDB can be used through the application's connection string.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ConferenceBookingDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

After configuring the connection string, apply the EF Core migrations to create or update the database.

---

## 20. Running the Project

### Requirements

Install:

- .NET 9 SDK;
- SQL Server or SQL Server LocalDB;
- Visual Studio 2022 or another compatible .NET IDE.

### Clone and enter the repository

```bash
git clone <repository-url>
cd ConferenceBooking
```

### Restore dependencies

```bash
dotnet restore
```

### Build

```bash
dotnet build
```

### Run the API

```bash
dotnet run --project src/ConferenceBooking.Api
```

### Swagger

After starting the application:

```text
https://localhost:<port>/swagger
```

### Run Unit Tests

```bash
dotnet test
```

Expected result:

```text
69 tests passed
```

---

## 21. Main API Endpoints

### Halls

```http
GET    /api/halls
GET    /api/halls/{id}
POST   /api/halls
PUT    /api/halls/{id}
DELETE /api/halls/{id}
GET    /api/halls/available
```

### Services

```http
GET    /api/services
GET    /api/services/{id}
POST   /api/services
PUT    /api/services/{id}
DELETE /api/services/{id}
```

### Bookings

```http
GET    /api/bookings
GET    /api/bookings/{id}
GET    /api/bookings/by-hall/{hallId}
POST   /api/bookings
DELETE /api/bookings/{id}
```

### Reports

```http
GET /api/reports/bookings
GET /api/reports/halls
GET /api/reports/services
```

---

## 22. Example: Create a Hall

```http
POST /api/halls
Content-Type: application/json
```

```json
{
  "name": "Conference Hall A",
  "capacity": 50,
  "hourlyRate": 2000,
  "serviceIds": []
}
```

---

## 23. Example: Create a Booking

```http
POST /api/bookings
Content-Type: application/json
```

```json
{
  "hallId": "00000000-0000-0000-0000-000000000000",
  "startTime": "2026-08-10T10:00:00",
  "endTime": "2026-08-10T14:00:00",
  "serviceIds": []
}
```

The response contains booking information, selected services and the calculated `TotalCost`.

---

## 24. Current Project Status

| Feature | Status |
|---|:---:|
| Clean Architecture | ✅ |
| Domain Model | ✅ |
| EF Core / SQL Server | ✅ |
| CRUD Halls | ✅ |
| Services | ✅ |
| Bookings | ✅ |
| Availability Check | ✅ |
| Cost Calculation | ✅ |
| Tariff Zones | ✅ |
| Validation | ✅ |
| Exception Middleware | ✅ |
| Logging | ✅ |
| Dependency Injection | ✅ |
| Swagger | ✅ |
| Swagger Documentation | 🟡 |
| Unit Tests | ✅ 69 tests |
| Integration Tests | 🔜 |
| Security Hardening | ✅ |
| Reports / Analytics | ✅ |
| README | ✅ |
| Final Documentation | ✅ |
| Final Smoke Check | 🔜 |

---

## 25. Final Verification Plan

Before considering the project complete, perform the following smoke-check.

### Build

```bash
dotnet build
```

Expected:

```text
Build succeeded.
```

### Unit Tests

```bash
dotnet test
```

Expected:

```text
69 tests passed.
```

### Database

Verify:

- application starts successfully;
- database connection works;
- migrations are applied;
- required seed data exists.

### Swagger

Verify:

- Swagger opens;
- all controllers are displayed;
- request models are correct;
- response models are correct;
- documented error responses are displayed.

### Halls

Verify:

- create;
- get;
- update;
- delete;
- availability search;
- validation errors.

### Services

Verify:

- get services;
- create;
- update;
- delete;
- service assignment to halls.

### Bookings

Verify:

- create booking;
- calculate total cost;
- select services;
- detect booking conflicts;
- retrieve bookings;
- delete booking.

### Reports

Verify:

```text
GET /api/reports/bookings
GET /api/reports/halls
GET /api/reports/services
```

Also verify an invalid period:

```text
from >= to
```

returns:

```text
400 Bad Request
```

---

## 26. Possible Future Improvements

Potential future extensions include:

- integration tests for the complete API;
- authentication and authorization;
- role-based access control;
- pagination and filtering;
- API versioning;
- rate limiting;
- more advanced analytics;
- export of reports to CSV/Excel/PDF;
- automated CI/CD pipeline;
- containerization with Docker;
- production monitoring and health checks.

These improvements are not required for the current core implementation.

---

## 27. Conclusion

ConferenceBooking implements the core functionality required for conference hall rental management.

The project combines:

```text
Clean Architecture
       +
Domain Business Rules
       +
EF Core / SQL Server
       +
REST API
       +
Validation
       +
Exception Handling
       +
Logging
       +
Cost Calculation
       +
Reports / Analytics
       +
Unit Tests
       +
Swagger
```

The current implementation provides a scalable foundation for further development while keeping business logic separated from API and infrastructure concerns.

The final remaining project activity is a complete smoke-check of the running application, followed by addressing any issues discovered during verification.
