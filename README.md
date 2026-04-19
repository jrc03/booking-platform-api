# Booking Platform API

This repository contains the back-end infrastructure for the Booking Platform, a comprehensive API driving a property rental and reservation system. It provides secure, robust data management and real-time capabilities.

The corresponding front-end React web application can be found here: [Booking Platform Web Interface](https://github.com/jrc03/booking-platform-web).

## Technology Stack

- **Framework**: C# .NET (ASP.NET Core Web API)
- **Architecture**: Domain-Driven Design (Onion Architecture)
- **Data Access**: Entity Framework Core
- **Message Dispatch**: MediatR (CQRS Pattern)
- **Real-Time Communication**: Microsoft SignalR (WebSockets)
- **Authentication**: JWT Bearer Tokens
- **API Documentation**: Scalar OpenAPI

## Features

### Architecture & Security
- **Strict Onion Architecture**: Completely decoupled layers (Domain, Application, Infrastructure, WebAPI) ensuring highly testable code where dependencies elegantly flow inward.
- **CQRS Pattern**: Enforced segregation of Commands (writes) and Queries (reads) utilizing MediatR for highly traceable internal event tracking.
- **Role-Based Authorization**: JWT authentication natively filtering endpoint access based on "Host" and "Guest" permission models.

### Core Domains
- **Properties Registry**: Secure endpoints to retrieve, create, and manage rental properties. Includes host-exclusive data mutation.
- **Booking Engine**: State-managed booking lifecycles enforcing logical date constraints, validation, and completion sequences.
- **Review System**: Relational mapping allowing Guests to post 1-5 star ratings directly attached to completed Bookings.
- **Host Availability Control**: Dynamic logic processing arbitrary blocked dates overlaying standard booked reservations.

### Real-Time Infrastructure (WebSockets)
- **SignalR Telemetry**: Real-time push notification system maintaining persistent WebSocket connections.
- **Decoupled Publishing**: A strictly typed `INotificationRealtimePublisher` interface implemented inside infrastructure, completely isolating WebAPI logic away from internal Application events.

## Getting Started

### Prerequisites
- .NET 8.0 SDK (or higher)
- A compatible SQL host engine (configured via connection strings)

### Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd booking-platform-api
   ```

2. Restore Dependencies:
   ```bash
   dotnet restore
   ```

3. Database Migrations:
   Ensure your database connection string is properly set in `appsettings.json` or your user secrets, then apply the Entity Framework migrations.
   ```bash
   dotnet ef database update
   ```

4. Start the Application:
   ```bash
   dotnet run --project WebAPI
   ```

## API Documentation
When running the development environment, navigate to `/scalar` or the root to view the visually interactive OpenAPI specification dynamically mapped directly from the C# Controllers. This provides a playground to trigger endpoints and generate HTTP client code snippets natively!
