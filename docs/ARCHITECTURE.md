# Architecture Documentation - Dating App

## Table of Contents

- [Overview](#overview)
- [System Architecture](#system-architecture)
- [Backend Architecture](#backend-architecture)
- [Frontend Architecture](#frontend-architecture)
- [Data Flow](#data-flow)
- [Security Architecture](#security-architecture)
- [Database Design](#database-design)
- [API Design](#api-design)
- [Authentication Flow](#authentication-flow)
- [Design Patterns](#design-patterns)
- [Technology Choices](#technology-choices)

## Overview

The Dating App is built using a modern three-tier architecture pattern, separating the presentation layer (Angular), business logic layer (.NET API), and data layer (SQLite Database). This separation ensures maintainability, scalability, and testability.

### High-Level Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                        Client Layer                          │
│  ┌────────────────────────────────────────────────────┐     │
│  │         Angular 12 SPA (Port 4200)                 │     │
│  │  - Components  - Services  - HTTP Client           │     │
│  └────────────────────────────────────────────────────┘     │
└─────────────────────────────────────────────────────────────┘
                            │
                            │ HTTPS/REST
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                      API Layer (.NET 8)                      │
│  ┌────────────────────────────────────────────────────┐     │
│  │              Controllers                            │     │
│  │  - AccountController  - UsersController            │     │
│  └────────────────────────────────────────────────────┘     │
│  ┌────────────────────────────────────────────────────┐     │
│  │              Services & Business Logic              │     │
│  │  - TokenService  - Authentication                  │     │
│  └────────────────────────────────────────────────────┘     │
│  ┌────────────────────────────────────────────────────┐     │
│  │              Data Access Layer                      │     │
│  │  - DataContext  - Repositories                     │     │
│  └────────────────────────────────────────────────────┘     │
└─────────────────────────────────────────────────────────────┘
                            │
                            │ Entity Framework Core
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    Data Layer (SQLite)                       │
│  ┌────────────────────────────────────────────────────┐     │
│  │              Database (datingapp.db)                │     │
│  │  - User Table                                      │     │
│  └────────────────────────────────────────────────────┘     │
└─────────────────────────────────────────────────────────────┘
```

## System Architecture

### Architecture Style

**Type:** Client-Server Architecture with RESTful API

**Characteristics:**
- **Stateless Communication:** Each request contains all necessary information
- **Separation of Concerns:** Clear boundaries between layers
- **Single Page Application (SPA):** Rich client-side experience
- **API-First Design:** Backend exposes RESTful endpoints

### Component Interaction

```
User Browser
    │
    ├─→ HTTP Request (Angular)
    │       │
    │       └─→ API Request (HTTPS)
    │               │
    │               ├─→ Controller (Routing)
    │               │       │
    │               │       ├─→ Service Layer (Business Logic)
    │               │       │       │
    │               │       │       └─→ Data Context (EF Core)
    │               │       │               │
    │               │       │               └─→ Database
    │               │       │               
    │               │       ← ← ← ← ← ← ← ← ←
    │               │
    │               ← ← JSON Response
    │
    ← ← HTML/CSS/JS
```

## Backend Architecture

### Layered Architecture

#### 1. Presentation Layer (Controllers)

**Responsibility:** Handle HTTP requests and responses

**Components:**
- `AccountController` - Authentication endpoints
- `UsersController` - User management endpoints
- `BaseApiController` - Shared controller functionality

**Pattern:** Controller Pattern

```csharp
[ApiController]
[Route("api/[controller]")]
public class AccountController : BaseApiController
{
    // Handles /api/account/* routes
}
```

#### 2. Business Logic Layer (Services)

**Responsibility:** Implement business rules and logic

**Components:**
- `TokenService` - JWT token generation
- `ITokenService` - Service interface

**Pattern:** Service Layer Pattern

```csharp
public class TokenService : ITokenService
{
    public string CreateToken(AppUser user)
    {
        // Business logic for token creation
    }
}
```

#### 3. Data Access Layer

**Responsibility:** Database operations and data persistence

**Components:**
- `DataContext` - EF Core DbContext
- `AppUser` - Entity model

**Pattern:** Repository Pattern (via EF Core)

```csharp
public class DataContext : DbContext
{
    public DbSet<AppUser> User { get; set; }
}
```

### Dependency Injection

The application uses .NET's built-in DI container:

```csharp
// Extension method pattern for service registration
public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationService(
        this IServiceCollection services, 
        IConfiguration config)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddDbContext<DataContext>(...);
        return services;
    }
}
```

**Benefits:**
- Loose coupling
- Easier testing (mock dependencies)
- Better maintainability
- Lifecycle management

### Middleware Pipeline

```
Request
  │
  ├─→ HTTPS Redirection
  ├─→ Routing
  ├─→ CORS
  ├─→ Authentication
  ├─→ Authorization
  ├─→ Controller Execution
  │
Response
```

Configured in `Startup.cs`:

```csharp
public void Configure(IApplicationBuilder app)
{
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseCors(...);
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(...);
}
```

## Frontend Architecture

### Angular Component Architecture

```
AppComponent (Root)
    │
    ├─→ Displays user list
    └─→ Makes HTTP calls to API
```

**Current Structure:**
- **Root Component:** `AppComponent`
- **HTTP Service:** `HttpClient` (injected)
- **No Router Yet:** Future enhancement

### Planned Architecture (Future)

```
AppComponent
    │
    ├─→ NavComponent
    ├─→ RouterOutlet
    │       │
    │       ├─→ HomeComponent
    │       ├─→ RegisterComponent
    │       ├─→ MembersListComponent
    │       └─→ MemberDetailComponent
    │
    └─→ Services
            ├─→ AccountService
            ├─→ MemberService
            └─→ PresenceService
```

### Angular Modules

```typescript
AppModule (Root Module)
    │
    ├─→ BrowserModule
    ├─→ HttpClientModule
    ├─→ BrowserAnimationsModule
    └─→ AppRoutingModule
```

### Data Flow in Angular

```
Component
    │
    ├─→ Injects Service
    │       │
    │       ├─→ Makes HTTP Call
    │       │       │
    │       │       └─→ API Endpoint
    │       │
    │       └─→ Returns Observable
    │
    └─→ Subscribes to Observable
            │
            └─→ Updates UI
```

## Data Flow

### Registration Flow

```
1. User fills registration form (Angular)
   │
2. Angular sends POST /api/account/register
   │
3. AccountController.Register()
   │
4. Check if username exists (DataContext)
   │
5. Create password hash/salt (HMACSHA512)
   │
6. Save user to database
   │
7. Generate JWT token (TokenService)
   │
8. Return UserDto with token
   │
9. Angular receives response
   │
10. Store token (future: localStorage)
```

### Authentication Flow

```
1. User logs in
   │
2. POST /api/account/login
   │
3. Validate credentials
   │
4. Generate JWT token
   │
5. Return token to client
   │
6. Client stores token
   │
7. Subsequent requests include token in header:
   Authorization: Bearer {token}
   │
8. API validates token (JWT middleware)
   │
9. Extract user claims
   │
10. Allow/Deny access
```

## Security Architecture

### Authentication Mechanism

**Method:** JWT (JSON Web Token)

**Flow:**

```
Registration/Login
    │
    ├─→ Hash password (HMACSHA512 + Salt)
    ├─→ Store in database
    │
Login Success
    │
    └─→ Generate JWT Token
            │
            ├─→ Claims: { nameId: username }
            ├─→ Expires: 7 days
            ├─→ Signature: HMAC-SHA512
            │
            └─→ Return to client
```

### Password Security

**Algorithm:** HMAC-SHA512

```csharp
// Password Hashing
using var hmac = new HMACSHA512();
user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
user.PasswordSalt = hmac.Key;

// Password Verification
using var hmac = new HMACSHA512(user.PasswordSalt);
var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
// Compare byte arrays
```

**Security Features:**
- Unique salt per user
- SHA-512 hashing
- No plain text password storage
- Constant-time comparison

### JWT Token Structure

```json
{
  "header": {
    "alg": "HS512",
    "typ": "JWT"
  },
  "payload": {
    "nameid": "username",
    "nbf": 1776325302,
    "exp": 1776930102,
    "iat": 1776325302
  },
  "signature": "..."
}
```

### CORS Configuration

```csharp
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);
```

**Note:** In production, specify allowed origins instead of `AllowAnyOrigin()`.

## Database Design

### Entity Relationship Diagram (Current)

```
┌─────────────────────────┐
│        AppUser          │
├─────────────────────────┤
│ Id (PK)                 │
│ UserName                │
│ PasswordHash            │
│ PasswordSalt            │
└─────────────────────────┘
```

### Future Schema (Planned)

```
┌─────────────────────────┐       ┌─────────────────────────┐
│        AppUser          │       │         Photo           │
├─────────────────────────┤       ├─────────────────────────┤
│ Id (PK)                 │◄──┐   │ Id (PK)                 │
│ UserName                │   │   │ Url                     │
│ PasswordHash            │   │   │ AppUserId (FK)          │
│ PasswordSalt            │   │   │ IsMain                  │
│ DateOfBirth             │   └───┤                         │
│ KnownAs                 │       └─────────────────────────┘
│ Gender                  │
│ Introduction            │       ┌─────────────────────────┐
│ LookingFor              │       │         Like            │
│ Interests               │       ├─────────────────────────┤
│ City                    │   ┌───┤ SourceUserId (FK)       │
│ Country                 │◄──┘   │ LikedUserId (FK)        │
└─────────────────────────┘       └─────────────────────────┘
```

### Migrations

**Migration Files:**
1. `20220120074638_InitialCreate` - Creates User table
2. `20220122051811_UserPasswordAdded` - Adds password fields

**Migration Commands:**
```bash
# Create migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Rollback
dotnet ef database update PreviousMigration
```

## API Design

### RESTful Principles

1. **Resource-Based URLs**
   - `/api/users` - Collection
   - `/api/users/{id}` - Individual resource

2. **HTTP Methods**
   - GET - Retrieve data
   - POST - Create data
   - PUT - Update data (future)
   - DELETE - Delete data (future)

3. **Stateless**
   - No server-side sessions
   - JWT tokens for state

4. **Standard HTTP Status Codes**
   - 200 OK - Success
   - 201 Created - Resource created
   - 400 Bad Request - Validation error
   - 401 Unauthorized - Auth required
   - 404 Not Found - Resource not found
   - 500 Internal Server Error

### API Versioning

**Current:** No versioning (v1 implicit)

**Future:** URL-based versioning
```
/api/v1/users
/api/v2/users
```

### Response Format

**Success Response:**
```json
{
  "username": "testuser",
  "token": "eyJhbGciOiJIUz..."
}
```

**Error Response:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "errors": {
    "username": ["Username is required"]
  }
}
```

## Design Patterns

### 1. Repository Pattern

**Implementation:** Via Entity Framework Core

```csharp
// DataContext acts as a repository
public DbSet<AppUser> User { get; set; }

// Usage in controller
await _context.User.ToListAsync();
```

### 2. Dependency Injection Pattern

```csharp
public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    
    public AccountController(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }
}
```

### 3. DTO Pattern

```csharp
// Instead of returning entity directly
public class UserDto
{
    public string Username { get; set; }
    public string Token { get; set; }
}

// Controllers return DTOs, not entities
return new UserDto { ... };
```

**Benefits:**
- Security (don't expose internal structure)
- Flexibility (different views of same data)
- Versioning (maintain compatibility)

### 4. Extension Methods Pattern

```csharp
public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationService(
        this IServiceCollection services, 
        IConfiguration config)
    {
        // Service registration
        return services;
    }
}

// Usage
services.AddApplicationService(_config);
```

### 5. Options Pattern

```csharp
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  }
}

// Access via IConfiguration
config.GetConnectionString("DefaultConnection")
```

## Technology Choices

### Why .NET 8?

1. **Cross-Platform:** Runs on Windows, macOS, Linux
2. **Performance:** High throughput, low latency
3. **Modern:** Latest C# features
4. **Ecosystem:** Rich library ecosystem
5. **Support:** Long-term support (LTS)

### Why Angular 12?

1. **TypeScript:** Type safety and better tooling
2. **Component-Based:** Reusable, maintainable code
3. **RxJS:** Powerful reactive programming
4. **CLI:** Excellent developer experience
5. **Ecosystem:** Large community and libraries

### Why SQLite?

1. **Simplicity:** No server required
2. **Portability:** Single file database
3. **Development:** Easy setup for development
4. **Migration Path:** Can migrate to PostgreSQL/SQL Server for production

### Why JWT?

1. **Stateless:** No server-side session storage
2. **Scalable:** Works across multiple servers
3. **Standard:** Industry-standard (RFC 7519)
4. **Self-Contained:** Token includes user info
5. **Mobile-Friendly:** Works well with mobile apps

## Scalability Considerations

### Current Limitations

- SQLite (not suitable for high concurrency)
- No caching layer
- No load balancing
- Single server deployment

### Future Improvements

1. **Database**
   - Migrate to PostgreSQL/SQL Server
   - Add read replicas
   - Implement database pooling

2. **Caching**
   - Add Redis for session caching
   - Implement response caching
   - Use CDN for static assets

3. **Application**
   - Implement horizontal scaling
   - Add load balancer
   - Use message queues for async tasks

4. **Monitoring**
   - Application Insights
   - Logging infrastructure
   - Performance monitoring

## Conclusion

The Dating App architecture follows established patterns and best practices for building modern web applications. The separation of concerns, use of dependency injection, and RESTful API design create a maintainable and extensible codebase ready for future enhancements.

---

**For more information, see:**
- [README.md](../README.md) - Project overview
- [SETUP.md](./SETUP.md) - Setup instructions
- [API_DOCUMENTATION.md](./API_DOCUMENTATION.md) - API reference
