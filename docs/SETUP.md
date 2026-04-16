# Setup Guide - Dating App

This comprehensive guide will walk you through setting up the Dating App on your local machine.

## Table of Contents

- [System Requirements](#system-requirements)
- [Prerequisites Installation](#prerequisites-installation)
- [Project Setup](#project-setup)
- [Database Setup](#database-setup)
- [Running the Application](#running-the-application)
- [Development Tools](#development-tools)
- [Troubleshooting](#troubleshooting)

## System Requirements

### Minimum Requirements

- **Operating System**: Windows 10+, macOS 10.15+, or Linux (Ubuntu 20.04+)
- **RAM**: 4 GB minimum, 8 GB recommended
- **Disk Space**: 2 GB free space
- **Internet Connection**: Required for initial setup

### Software Requirements

- .NET 8 SDK or higher
- Node.js 14.x or higher (16.x recommended)
- npm 6.x or higher
- Git 2.x or higher

## Prerequisites Installation

### 1. Install .NET 8 SDK

#### Windows

1. Download the installer from [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Run the installer and follow the prompts
3. Verify installation:
   ```cmd
   dotnet --version
   ```
   Expected output: `8.0.x` or higher

#### macOS

Using Homebrew:
```bash
brew install --cask dotnet-sdk
```

Or download from [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)

Verify installation:
```bash
dotnet --version
```

#### Linux (Ubuntu/Debian)

```bash
# Add Microsoft package repository
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Install .NET SDK
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0

# Verify installation
dotnet --version
```

### 2. Install Node.js and npm

#### Windows

1. Download the installer from [https://nodejs.org/](https://nodejs.org/)
2. Run the installer (choose LTS version)
3. Verify installation:
   ```cmd
   node --version
   npm --version
   ```

#### macOS

Using Homebrew:
```bash
brew install node@16
```

Or download from [https://nodejs.org/](https://nodejs.org/)

Verify installation:
```bash
node --version
npm --version
```

#### Linux (Ubuntu/Debian)

```bash
# Using NodeSource repository
curl -fsSL https://deb.nodesource.com/setup_16.x | sudo -E bash -
sudo apt-get install -y nodejs

# Verify installation
node --version
npm --version
```

### 3. Install Git

#### Windows

Download and install from [https://git-scm.com/download/win](https://git-scm.com/download/win)

#### macOS

```bash
brew install git
```

#### Linux

```bash
sudo apt-get install git
```

Verify installation (all platforms):
```bash
git --version
```

### 4. Install Entity Framework Core Tools

After installing .NET SDK:

```bash
dotnet tool install --global dotnet-ef
```

Verify installation:
```bash
dotnet ef --version
```

## Project Setup

### 1. Clone the Repository

```bash
# Clone the repository
git clone https://github.com/HimanshuVinod/DatingApp.git

# Navigate to project directory
cd DatingApp
```

### 2. Backend API Setup

```bash
# Navigate to API directory
cd API

# Restore NuGet packages
dotnet restore

# Build the project
dotnet build
```

Expected output: `Build succeeded.`

### 3. Frontend Client Setup

```bash
# Navigate to client directory (from root)
cd ../client

# Install npm packages
npm install
```

This will install all Angular dependencies. It may take a few minutes.

### 4. Configuration

#### API Configuration

Edit `API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data source=datingapp.db"
  },
  "TokenKey": "super secret ungussable key that is at least 64 characters long for security purposes and JWT token generation",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

**Important**: 
- The `TokenKey` must be at least 64 characters long
- Change this key for production deployments
- Never commit production keys to source control

#### Client Configuration

The client is pre-configured to connect to `https://localhost:5001/api`

To change the API URL, edit `client/src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api'
};
```

## Database Setup

### 1. Apply Migrations

The database is automatically created when you apply migrations:

```bash
# Navigate to API directory
cd API

# Apply database migrations
dotnet ef database update
```

This creates a SQLite database file `datingapp.db` in the API directory.

### 2. Verify Database

Check that the database file was created:

```bash
ls -la datingapp.db*
```

You should see:
- `datingapp.db` - The database file
- `datingapp.db-shm` - Shared memory file
- `datingapp.db-wal` - Write-ahead log file

### 3. Database Schema

The database includes the following table:

**User Table:**
```sql
CREATE TABLE User (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserName TEXT,
    PasswordHash BLOB,
    PasswordSalt BLOB
);
```

## Running the Application

### Method 1: Run Separately (Recommended for Development)

#### Start the API

```bash
# From API directory
cd API
dotnet run
```

Output should show:
```
Now listening on: https://localhost:5001
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

#### Start the Client (in a new terminal)

```bash
# From client directory
cd client
npm start
```

Output should show:
```
** Angular Live Development Server is listening on localhost:4200 **
```

Navigate to `http://localhost:4200` in your web browser.

### Method 2: Run with Scripts

**Windows (PowerShell):**

Create a file `start.ps1`:
```powershell
# Start API in background
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd API; dotnet run"

# Wait for API to start
Start-Sleep -Seconds 5

# Start Angular client
cd client
npm start
```

**macOS/Linux (Bash):**

Create a file `start.sh`:
```bash
#!/bin/bash

# Start API in background
cd API
dotnet run &

# Wait for API to start
sleep 5

# Start Angular client
cd ../client
npm start
```

Make it executable:
```bash
chmod +x start.sh
./start.sh
```

## Development Tools

### Recommended IDE

**Visual Studio Code** with extensions:
- C# for Visual Studio Code (ms-dotnettools.csharp)
- Angular Language Service (Angular.ng-template)
- Angular Snippets (johnpapa.Angular2)
- REST Client (humao.rest-client)
- SQLite Viewer (qwtel.sqlite-viewer)

### Testing Tools

1. **Swagger UI** (Included)
   - Navigate to `https://localhost:5001/swagger`
   - Interactive API documentation and testing

2. **Postman** (Optional)
   - Download from [https://www.postman.com/](https://www.postman.com/)
   - Import API collection for testing

3. **Browser DevTools**
   - Chrome DevTools (recommended)
   - Firefox Developer Tools

### Useful Commands

#### API Commands

```bash
# Build the project
dotnet build

# Run the project
dotnet run

# Run with watch (auto-restart on changes)
dotnet watch run

# Run tests
dotnet test

# Create a migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Rollback migration
dotnet ef database update PreviousMigrationName

# Remove last migration
dotnet ef migrations remove
```

#### Client Commands

```bash
# Start development server
npm start

# Build for production
npm run build

# Run unit tests
npm test

# Run linter
npm run lint

# Generate a component
ng generate component component-name

# Generate a service
ng generate service service-name
```

## Troubleshooting

### Common Issues and Solutions

#### 1. Port Already in Use

**Error:** `Address already in use`

**Solution:**

Windows:
```cmd
# Find process using port 5001
netstat -ano | findstr :5001

# Kill the process (replace PID with actual process ID)
taskkill /PID <PID> /F
```

macOS/Linux:
```bash
# Find and kill process using port 5001
lsof -ti:5001 | xargs kill -9

# Or use a different port
dotnet run --urls "http://localhost:5050;https://localhost:5051"
```

#### 2. .NET SDK Not Found

**Error:** `The command could not be loaded`

**Solution:**
- Ensure .NET 8 SDK is installed: `dotnet --version`
- Add .NET to PATH if needed
- Restart terminal after installation

#### 3. Node.js Version Issues

**Error:** `ERR_OSSL_EVP_UNSUPPORTED`

**Solution:**
The project is already configured to use legacy OpenSSL provider. If you still face issues:

Windows:
```cmd
set NODE_OPTIONS=--openssl-legacy-provider
npm start
```

macOS/Linux:
```bash
export NODE_OPTIONS=--openssl-legacy-provider
npm start
```

Or downgrade to Node.js 16.x:
```bash
nvm install 16
nvm use 16
```

#### 4. Database Migration Errors

**Error:** `No such table: User`

**Solution:**
```bash
cd API
dotnet ef database drop    # Drops existing database
dotnet ef database update  # Creates new database with all migrations
```

#### 5. CORS Errors in Browser

**Error:** `Access to XMLHttpRequest has been blocked by CORS policy`

**Solution:**
- Ensure API is running before starting the client
- Check CORS configuration in `Startup.cs`
- Clear browser cache and reload

#### 6. npm Install Fails

**Error:** Various npm errors during `npm install`

**Solution:**
```bash
# Clear npm cache
npm cache clean --force

# Delete node_modules and package-lock.json
rm -rf node_modules package-lock.json

# Reinstall
npm install
```

#### 7. Entity Framework Tools Not Found

**Error:** `Could not execute because the specified command or file was not found`

**Solution:**
```bash
# Install EF Core tools globally
dotnet tool install --global dotnet-ef

# Update PATH (add to ~/.bashrc or ~/.zshrc)
export PATH="$PATH:$HOME/.dotnet/tools"

# Reload shell
source ~/.bashrc
```

#### 8. Angular Build Errors

**Error:** Various Angular compilation errors

**Solution:**
```bash
# Clean build
rm -rf dist .angular

# Rebuild
npm run build
```

#### 9. SSL Certificate Issues

**Error:** `The SSL connection could not be established`

**Solution:**
```bash
# Trust the .NET development certificate
dotnet dev-certs https --trust

# Or use HTTP instead of HTTPS for development
# Update client/src/environments/environment.ts
# apiUrl: 'http://localhost:5000/api'
```

### Getting Help

If you encounter issues not covered here:

1. Check the [GitHub Issues](https://github.com/HimanshuVinod/DatingApp/issues)
2. Review the main [README.md](../README.md)
3. Check the [API Documentation](./API_DOCUMENTATION.md)
4. Open a new issue with:
   - Your operating system and version
   - .NET version (`dotnet --version`)
   - Node.js version (`node --version`)
   - Complete error message
   - Steps to reproduce

## Next Steps

After successful setup:

1. **Explore the API** - Navigate to `https://localhost:5001/swagger`
2. **Test Registration** - Create a new user account
3. **Test Login** - Login with created credentials
4. **View Users** - Browse the users list
5. **Review Code** - Explore the codebase structure
6. **Read Documentation** - Check [ARCHITECTURE.md](./ARCHITECTURE.md) for system design

## Production Deployment

For production deployment guidance, see:
- [DEPLOYMENT.md](./DEPLOYMENT.md) (coming soon)

---

**Setup complete! Happy coding! 🚀**
