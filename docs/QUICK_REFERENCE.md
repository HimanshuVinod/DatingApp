# Quick Reference Guide - Dating App

This is a quick reference for developers working with the Dating App. For detailed documentation, see the full docs.

## 🚀 Quick Start

### Start the Application

```bash
# Terminal 1 - Start API
cd API
dotnet run

# Terminal 2 - Start Client
cd client
npm start
```

Then open: `http://localhost:4200`

## 📌 Common Commands

### API Commands

```bash
# Build
dotnet build

# Run
dotnet run

# Run with auto-reload
dotnet watch run

# Database migrations
dotnet ef migrations add MigrationName
dotnet ef database update

# Restore packages
dotnet restore
```

### Client Commands

```bash
# Install dependencies
npm install

# Start dev server
npm start

# Build for production
npm run build

# Run tests
npm test
```

## 🌐 URLs

| Service | URL | Description |
|---------|-----|-------------|
| Angular Client | http://localhost:4200 | Frontend application |
| API (HTTP) | http://localhost:5000 | Backend API |
| API (HTTPS) | https://localhost:5001 | Backend API (SSL) |
| Swagger UI | https://localhost:5001/swagger | API documentation |

## 🔑 API Endpoints

### Authentication

```bash
# Register
POST /api/account/register
{
  "username": "string",
  "password": "string"
}

# Login
POST /api/account/login
{
  "username": "string",
  "password": "string"
}
```

### Users

```bash
# Get all users (no auth)
GET /api/users

# Get user by ID (requires auth)
GET /api/users/{id}
Headers: Authorization: Bearer {token}
```

## 🧪 Testing

### Test with curl

```bash
# Register
curl -k -X POST https://localhost:5001/api/account/register \
  -H "Content-Type: application/json" \
  -d '{"username":"test","password":"Test123!"}'

# Login
curl -k -X POST https://localhost:5001/api/account/login \
  -H "Content-Type: application/json" \
  -d '{"username":"test","password":"Test123!"}'

# Get users
curl -k https://localhost:5001/api/users

# Get user by ID
curl -k https://localhost:5001/api/users/1 \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## 📂 Project Structure

```
DatingApp/
├── API/                        # .NET Backend
│   ├── Controllers/            # API endpoints
│   ├── Data/                   # Database context
│   ├── DTOs/                   # Data transfer objects
│   ├── Entities/               # Database models
│   ├── Extensions/             # Service extensions
│   ├── Interfaces/             # Service interfaces
│   ├── Services/               # Business logic
│   └── appsettings.Development.json
│
├── client/                     # Angular Frontend
│   ├── src/
│   │   ├── app/               # Angular app
│   │   ├── assets/            # Static files
│   │   └── environments/      # Environment configs
│   └── package.json
│
├── docs/                       # Documentation
│   ├── SETUP.md
│   ├── ARCHITECTURE.md
│   └── API_DOCUMENTATION.md
│
└── README.md
```

## 🔧 Configuration

### API Configuration

**File:** `API/appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data source=datingapp.db"
  },
  "TokenKey": "your-secret-key-min-64-chars"
}
```

### Client Configuration

**File:** `client/src/environments/environment.ts`

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api'
};
```

## 🗄️ Database

### Schema

```sql
User Table:
- Id (int, PK)
- UserName (string)
- PasswordHash (byte[])
- PasswordSalt (byte[])
```

### Reset Database

```bash
cd API
rm datingapp.db*
dotnet ef database update
```

## 🔐 Security

### Password Hashing
- Algorithm: HMAC-SHA512
- Unique salt per user
- Never stores plain text passwords

### JWT Tokens
- Algorithm: HS512
- Expiration: 7 days
- Minimum key length: 64 characters

## ❗ Common Issues

### Port in Use
```bash
# Find process on port 5001
lsof -ti:5001 | xargs kill -9
```

### Node.js OpenSSL Error
Already fixed in `package.json`:
```json
"start": "NODE_OPTIONS=--openssl-legacy-provider ng serve"
```

### Database Errors
```bash
dotnet ef database drop
dotnet ef database update
```

### CORS Errors
- Ensure API is running before client
- Check CORS settings in `Startup.cs`

## 📚 Full Documentation

- **[README.md](../README.md)** - Project overview and getting started
- **[SETUP.md](./SETUP.md)** - Detailed setup instructions
- **[ARCHITECTURE.md](./ARCHITECTURE.md)** - System architecture and design
- **[API_DOCUMENTATION.md](./API_DOCUMENTATION.md)** - Complete API reference

## 🆘 Getting Help

1. Check the full documentation above
2. Review GitHub Issues
3. Open a new issue with:
   - OS and versions
   - Complete error message
   - Steps to reproduce

## 💡 Pro Tips

### Development Workflow

1. **Start API first** - Client depends on it
2. **Use Swagger** - Test API before frontend integration
3. **Watch mode** - Use `dotnet watch run` for auto-reload
4. **Browser DevTools** - Monitor network requests
5. **SQLite Viewer** - VSCode extension to view database

### Code Quality

- Follow existing code style
- Add XML comments to public APIs
- Use meaningful variable names
- Keep controllers thin
- Put business logic in services

### Testing Strategy

1. Test API endpoints with Swagger
2. Test authentication flow
3. Test with curl/Postman
4. Test frontend integration
5. Test error scenarios

## 🎯 Next Steps

After setup:

1. ✅ Register a test user
2. ✅ Login and get JWT token
3. ✅ Test protected endpoints
4. ✅ Explore the codebase
5. ✅ Read architecture docs
6. 🚀 Start building features!

## 📦 Tech Stack Quick Reference

### Backend
- .NET 8.0
- Entity Framework Core 8.0
- SQLite
- JWT Authentication
- Swagger/OpenAPI

### Frontend
- Angular 12.2
- TypeScript 4.3
- Bootstrap 4.5
- RxJS 6.6

## 🔄 Common Workflows

### Adding a New Entity

1. Create entity in `Entities/`
2. Add DbSet to `DataContext`
3. Create migration: `dotnet ef migrations add AddEntity`
4. Update database: `dotnet ef database update`

### Adding a New Endpoint

1. Create/Update controller in `Controllers/`
2. Add DTO in `DTOs/` if needed
3. Test with Swagger
4. Update API documentation

### Adding a New Service

1. Create interface in `Interfaces/`
2. Implement in `Services/`
3. Register in `ApplicationServiceExtension`
4. Inject where needed

---

**Version:** 1.0  
**Last Updated:** 2026-04-16  
**For detailed information, refer to the full documentation in the docs/ folder**
