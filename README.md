# ConferenceBooking

ASP.NET Core Web API for managing conference halls, bookings, additional services, rental cost calculation, and booking analytics.

The project is designed as a backend API for a company that rents conference halls for business events.

## Project Overview

ConferenceBooking provides an API for:

- managing conference halls;
- managing additional services;
- searching for available halls;
- creating and managing bookings;
- checking booking conflicts;
- calculating booking costs based on booking duration and tariff zones;
- generating booking and utilization reports;
- centralized error handling and validation;
- logging application events;
- documenting the API with Swagger.

The main business goal is to allow clients to find a suitable conference hall, book it for a specific period, select additional services, and receive the calculated total booking cost.

## Technologies

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core 9
- SQL Server / LocalDB
- Swagger / OpenAPI
- xUnit
- Git

## Architecture

The project follows **Clean Architecture** principles with separation of responsibilities between layers.

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

Contains application/business orchestration logic:

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

The domain entities contain their own validation and business rules. For example, halls validate their name, capacity and hourly rate, while bookings validate the hall identifier and booking time range. 
#### ConferenceBooking.Infrastructure

Responsible for data access and infrastructure:

- Entity Framework Core
- `DbContext`
- database configurations
- repositories
- report repository
- database initialization / seed data

#### ConferenceBooking.Tests

Contains automated unit tests for the application's domain and application logic.

Current test suite:

**69 Unit Tests**

## Dependency Direction

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

## Main Features

### Conference Halls

The API supports:

- creating halls;
- retrieving halls;
- updating halls;
- deleting halls;
- retrieving available halls;
- assigning services to halls.

A hall contains:

- unique identifier;
- name;
- capacity;
- hourly rental rate;
- available services.

Hall capacity must be greater than zero and the hourly rate cannot be negative.

### Services

Services are additional options that can be selected for a booking.

Example services:

- Projector
- Wi-Fi
- Sound

Each service contains:

- unique identifier;
- name;
- price.

Service names cannot be empty and service prices cannot be negative.

### Bookings

A booking contains:

- hall identifier;
- start time;
- end time;
- selected services;
- total cost.

The API prevents overlapping bookings for the same hall.

A booking must have a valid time range where the start time is earlier than the end time.

### Hall Availability

The API can search for halls that:

- have sufficient capacity;
- are not already booked during the requested period.

The availability query checks for overlapping bookings before returning a hall as available.

## Booking Cost Calculation

Booking cost is calculated according to the booking period and tariff zones.

The hall cost is calculated by splitting the booking period into tariff segments.

### Tariff zones

| Time | Tariff |
|---|---:|
| 06:00–09:00 | -10% |
| 09:00–12:00 | Standard |
| 12:00–14:00 | +15% |
| 14:00–18:00 | Standard |
| 18:00–23:00 | -20% |

The final booking price consists of:

```text
Hall Cost + Additional Services Cost = Total Booking Cost
```

The hall cost is calculated according to the applicable tariff coefficient for each part of the booking period.

## Reports / Analytics

The API provides analytical reports for a selected period.

### Booking Summary

Returns:

- total number of bookings;
- total booked hours;
- total revenue;
- average booking cost.

### Hall Utilization

Returns statistics for each hall:

- hall ID;
- hall name;
- number of bookings;
- total booked hours;
- total revenue.

### Popular Services

Returns:

- service ID;
- service name;
- usage count.

The reports are available through the `ReportsController`.

Example:

```http
GET /api/reports/bookings?from=2026-01-01&to=2026-02-01
GET /api/reports/halls?from=2026-01-01&to=2026-02-01
GET /api/reports/services?from=2026-01-01&to=2026-02-01
```

The reporting period is validated: `from` must be earlier than `to`.

## Error Handling

The API uses centralized exception handling middleware.

Application and domain exceptions are converted into consistent HTTP error responses.

The API handles, among others:

- validation errors;
- not found errors;
- business rule violations;
- booking conflicts;
- invalid date ranges;
- unexpected server errors.

This prevents controllers from containing repetitive exception-handling logic.

## Validation

Validation is implemented at the appropriate application and domain levels.

Examples of protected business rules:

- hall name cannot be empty;
- hall capacity must be greater than zero;
- hall hourly rate cannot be negative;
- service name cannot be empty;
- service price cannot be negative;
- booking hall ID cannot be empty;
- booking start time must be earlier than end time;
- booking total cost cannot be negative;
- duplicate services cannot be added to a hall or booking.

## Logging

Application logging is implemented using `ILogger<T>`.

Logging is used for important application events and operational diagnostics, including:

- hall creation;
- hall updates;
- hall deletion;
- not-found situations;
- failed deletion attempts;
- HTTP request processing;
- unhandled exceptions.

## Security Hardening

The project includes a security hardening stage focused on protecting the API from invalid and potentially dangerous input.

Implemented protections include:

- validation of incoming data;
- validation of domain entities;
- protection against invalid identifiers;
- protection against invalid numeric values;
- validation of booking date ranges;
- prevention of conflicting bookings;
- centralized exception handling;
- consistent error responses;
- avoiding exposure of internal exception details through API responses.

## Unit Tests

The project contains automated unit tests using **xUnit**.

Current test suite:

**69 Unit Tests**

The tests cover domain rules and application logic, including validation and business scenarios.

The first group of tests covers the `Hall` domain entity, including:

- valid hall creation;
- invalid hall names;
- invalid capacity;
- invalid hourly rate;
- hall updates;
- service management.

The test suite is intended to protect business rules from regressions during further development.

## Swagger / OpenAPI

Swagger is used to document and test the API.

After starting the application, Swagger UI is available at:

```text
https://localhost:<port>/swagger
```

Swagger provides:

- available API endpoints;
- request models;
- response models;
- HTTP status codes;
- validation/error responses;
- API descriptions and XML documentation.

## Database

The project uses **Entity Framework Core** with **SQL Server**.

The database contains data for:

- halls;
- services;
- bookings;
- hall-service relationships;
- booking-service relationships.

The Infrastructure layer contains the EF Core `DbContext`, entity configurations and repositories.

### Local development database

For local development, SQL Server / LocalDB can be configured through the application's connection string.

Example configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ConferenceBookingDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

After configuring the connection string, apply the EF Core migrations to create/update the database.

## Running the Project

### Requirements

Install:

- .NET 9 SDK
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 or another compatible .NET IDE

### Start the API

Clone the repository and open the solution:

```bash
git clone <repository-url>
cd ConferenceBooking
```

Restore dependencies:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

Run the API:

```bash
dotnet run --project src/ConferenceBooking.Api
```

Then open Swagger:

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

## Main API Endpoints

### Halls

```http
GET    /api/halls
GET    /api/halls/{id}
POST   /api/halls
PUT    /api/halls/{id}
DELETE /api/halls/{id}
```

### Available Halls

```http
GET /api/halls/available
```

Example request parameters:

```text
startTime
endTime
capacity
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

## Example: Create a Hall

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

## Example: Create a Booking

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

The response contains the booking information, selected services and calculated `TotalCost`.

## Development Status

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
| Final Documentation | 🔜 |
| Final Smoke Check | 🔜 |

## Possible Future Improvements

Potential future extensions include:

- integration tests for the complete API;
- authentication and authorization;
- role-based access control;
- pagination and filtering for large collections;
- API versioning;
- rate limiting;
- more advanced analytics;
- export of reports to CSV/Excel/PDF;
- automated CI/CD pipeline;
- containerization with Docker;
- production monitoring and health checks.

## Project Goal

The project demonstrates the development of a scalable ASP.NET Core Web API using Clean Architecture, domain-driven business rules, Entity Framework Core, repository-based data access, centralized error handling, logging, automated testing, Swagger documentation, booking cost calculation and business analytics.