# Dating App

A full-stack dating application built with .NET 8 Web API and Angular 12. This application provides user authentication, profile management, and user listing capabilities.

![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)
![Angular Version](https://img.shields.io/badge/Angular-12.2-red)
![License](https://img.shields.io/badge/license-MIT-green)

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)

## 🌟 Overview

The Dating App is a modern web application that demonstrates a complete full-stack solution using .NET 8 for the backend API and Angular 12 for the frontend client. It implements secure authentication using JWT tokens and provides a RESTful API for user management.

## ✨ Features

### Current Features

- **User Authentication**
  - Secure user registration with password hashing (HMAC-SHA512)
  - JWT-based authentication
  - Token-based authorization

- **User Management**
  - View list of all registered users
  - Get individual user details
  - Secure password storage

- **API Features**
  - RESTful API design
  - Swagger/OpenAPI documentation
  - CORS enabled for cross-origin requests
  - SQLite database for easy deployment

### Planned Features

- User profiles with photos
- Matching algorithm
- Real-time messaging
- Like/Unlike functionality
- Advanced filtering and search

## 🛠 Technology Stack

### Backend (API)

- **.NET 8.0** - Modern, cross-platform framework
- **Entity Framework Core 8.0** - ORM for database operations
- **SQLite** - Lightweight database for development
- **JWT Authentication** - Secure token-based authentication
- **Swagger/OpenAPI** - API documentation and testing

### Frontend (Client)

- **Angular 12.2** - Progressive web application framework
- **TypeScript 4.3** - Type-safe JavaScript
- **Bootstrap 4.5** - Responsive UI framework
- **RxJS 6.6** - Reactive programming
- **ngx-bootstrap** - Bootstrap components for Angular

## 🏗 Architecture

The application follows a clean architecture pattern with separation of concerns:

```
DatingApp/
├── API/                    # Backend .NET Web API
│   ├── Controllers/        # API endpoints
│   ├── Data/              # Database context and migrations
│   ├── DTOs/              # Data Transfer Objects
│   ├── Entities/          # Domain models
│   ├── Extensions/        # Service extensions
│   ├── Interfaces/        # Service interfaces
│   └── Services/          # Business logic services
│
└── client/                # Frontend Angular application
    └── src/
        ├── app/           # Application components
        ├── assets/        # Static assets
        └── environments/  # Environment configurations
```

### Design Patterns Used

- **Repository Pattern** - Data access abstraction
- **Dependency Injection** - Loose coupling and testability
- **DTO Pattern** - Secure data transfer
- **Service Layer** - Business logic separation
- **Extension Methods** - Clean service configuration

## 🚀 Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (version 8.0 or higher)
- [Node.js](https://nodejs.org/) (version 14.x or higher)
- [npm](https://www.npmjs.com/) (version 6.x or higher)
- [Git](https://git-scm.com/)
- [Visual Studio Code](https://code.visualstudio.com/) (recommended) or any preferred IDE

### Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/HimanshuVinod/DatingApp.git
   cd DatingApp
   ```

2. **Setup the Backend API**

   ```bash
   # Navigate to API directory
   cd API

   # Restore NuGet packages
   dotnet restore

   # Build the project
   dotnet build

   # Install EF Core tools globally (if not already installed)
   dotnet tool install --global dotnet-ef

   # Apply database migrations
   dotnet ef database update

   # Run the API
   dotnet run
   ```

   The API will start on:
   - HTTP: `http://localhost:5000`
   - HTTPS: `https://localhost:5001`
   - Swagger UI: `https://localhost:5001/swagger`

3. **Setup the Frontend Client**

   ```bash
   # Navigate to client directory (from root)
   cd client

   # Install npm packages
   npm install

   # Start the development server
   npm start
   ```

   The Angular app will start on `http://localhost:4200`

### Quick Start (Both Services)

To run both the API and client simultaneously, open two terminal windows:

**Terminal 1 (API):**
```bash
cd API
dotnet run
```

**Terminal 2 (Client):**
```bash
cd client
npm start
```

Then navigate to `http://localhost:4200` in your browser.

## 📁 Project Structure

### API Structure

```
API/
├── Controllers/
│   ├── AccountController.cs      # Authentication endpoints
│   ├── UsersController.cs        # User management endpoints
│   └── BaseApiController.cs      # Base controller configuration
├── Data/
│   ├── DataContext.cs            # EF Core database context
│   └── Migrations/               # Database migrations
├── DTOs/
│   ├── LoginDto.cs               # Login request model
│   ├── RegisterDTOs.cs           # Registration request model
│   └── UserDto.cs                # User response model
├── Entities/
│   └── AppUser.cs                # User entity model
├── Extensions/
│   ├── ApplicationServiceExtension.cs    # App services configuration
│   └── IdentityServiceExtension.cs       # Auth services configuration
├── Interfaces/
│   └── ITokenService.cs          # Token service interface
├── Services/
│   └── TokenService.cs           # JWT token generation service
├── Program.cs                    # Application entry point
├── Startup.cs                    # Service and middleware configuration
└── appsettings.Development.json  # Development configuration
```

### Client Structure

```
client/
├── src/
│   ├── app/
│   │   ├── app.component.ts      # Root component
│   │   ├── app.component.html    # Root template
│   │   ├── app.module.ts         # Root module
│   │   └── app-routing.module.ts # Routing configuration
│   ├── assets/                   # Static assets
│   ├── environments/             # Environment configs
│   └── index.html               # Main HTML file
├── angular.json                 # Angular CLI configuration
├── package.json                 # npm dependencies
└── tsconfig.json               # TypeScript configuration
```

## 📚 API Documentation

### Base URL

- Development: `https://localhost:5001/api`

### Authentication Endpoints

#### Register a New User

```http
POST /api/account/register
```

**Request Body:**
```json
{
  "username": "string",
  "password": "string"
}
```

**Response:**
```json
{
  "username": "string",
  "token": "string"
}
```

#### Login

```http
POST /api/account/login
```

**Request Body:**
```json
{
  "username": "string",
  "password": "string"
}
```

**Response:**
```json
{
  "username": "string",
  "token": "string"
}
```

### User Endpoints

#### Get All Users

```http
GET /api/users
```

**Response:**
```json
[
  {
    "id": 1,
    "userName": "string"
  }
]
```

#### Get User by ID

```http
GET /api/users/{id}
```

**Headers:**
```
Authorization: Bearer {token}
```

**Response:**
```json
{
  "id": 1,
  "userName": "string"
}
```

### Swagger Documentation

Interactive API documentation is available at:
- `https://localhost:5001/swagger`

## 🔒 Security

The application implements several security best practices:

- **Password Hashing**: Uses HMAC-SHA512 with unique salt for each user
- **JWT Tokens**: Stateless authentication with 7-day expiration
- **HTTPS**: Enforced in development and production
- **CORS**: Configured to allow cross-origin requests
- **Authorization**: Protected endpoints require valid JWT tokens

## 🗄 Database

The application uses SQLite for development purposes:

- **Database File**: `datingapp.db` (created automatically)
- **Migrations**: Entity Framework Core migrations for version control
- **Connection String**: Configured in `appsettings.Development.json`

### Database Schema

**User Table:**
- `Id` (int, Primary Key)
- `UserName` (string)
- `PasswordHash` (byte[])
- `PasswordSalt` (byte[])

## 🧪 Testing

### Testing the API

1. **Using Swagger UI**
   - Navigate to `https://localhost:5001/swagger`
   - Try out the endpoints directly from the browser

2. **Using curl**
   ```bash
   # Register a user
   curl -k -X POST https://localhost:5001/api/account/register \
     -H "Content-Type: application/json" \
     -d '{"username":"testuser","password":"Test123!"}'

   # Login
   curl -k -X POST https://localhost:5001/api/account/login \
     -H "Content-Type: application/json" \
     -d '{"username":"testuser","password":"Test123!"}'

   # Get users
   curl -k https://localhost:5001/api/users
   ```

### Testing the Client

The Angular application can be tested by:
1. Running `npm start` and navigating to `http://localhost:4200`
2. Running unit tests with `npm test`

## 🛠 Development

### Building for Production

**API:**
```bash
cd API
dotnet publish -c Release
```

**Client:**
```bash
cd client
npm run build
```

The production files will be in `client/dist/client/`.

### Database Migrations

To create a new migration:
```bash
cd API
dotnet ef migrations add MigrationName
dotnet ef database update
```

## ⚙️ Configuration

### API Configuration

Edit `API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data source=datingapp.db"
  },
  "TokenKey": "your-secret-key-here-at-least-64-characters-long",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Client Configuration

Edit `client/src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api'
};
```

## 🐛 Troubleshooting

### Common Issues

1. **API doesn't start - "Framework not found"**
   - Ensure .NET 8 SDK is installed: `dotnet --version`
   - Install from: https://dotnet.microsoft.com/download/dotnet/8.0

2. **Angular build fails with OpenSSL error**
   - The project is already configured to use legacy OpenSSL provider
   - If issues persist, use Node.js version 16.x

3. **Database errors**
   - Delete `datingapp.db` and run `dotnet ef database update`

4. **CORS errors**
   - Ensure the API is running before starting the client
   - Check that CORS is configured in `Startup.cs`

## 📖 Additional Documentation

- [SETUP.md](./docs/SETUP.md) - Detailed setup instructions
- [ARCHITECTURE.md](./docs/ARCHITECTURE.md) - System architecture details
- [API_DOCUMENTATION.md](./docs/API_DOCUMENTATION.md) - Complete API reference

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👥 Authors

- **Himanshu Vinod** - [GitHub](https://github.com/HimanshuVinod)

## 🙏 Acknowledgments

- Built with .NET and Angular
- Inspired by modern dating applications
- Uses industry-standard security practices

## 📞 Support

For support, please open an issue in the GitHub repository.

---

**Happy Coding! 💻**
