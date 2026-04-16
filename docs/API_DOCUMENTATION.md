# API Documentation - Dating App

## Table of Contents

- [Overview](#overview)
- [Base URL](#base-url)
- [Authentication](#authentication)
- [Endpoints](#endpoints)
  - [Account](#account-endpoints)
  - [Users](#user-endpoints)
- [Error Handling](#error-handling)
- [Examples](#examples)
- [Testing](#testing)

## Overview

The Dating App API is a RESTful web service built with .NET 8 that provides authentication and user management functionality.

### Key Features

- User registration and authentication
- JWT token-based authorization
- User profile retrieval
- Swagger/OpenAPI documentation

### API Version

**Current Version:** 1.0 (implicit)

### Content Type

All requests and responses use JSON format.

**Request Header:**
```
Content-Type: application/json
```

**Response Header:**
```
Content-Type: application/json; charset=utf-8
```

## Base URL

### Development

```
HTTP:  http://localhost:5000/api
HTTPS: https://localhost:5001/api
```

### Production

```
https://your-domain.com/api
```

## Authentication

The API uses JWT (JSON Web Token) for authentication.

### Getting a Token

Obtain a token by registering a new account or logging in:

```http
POST /api/account/register
POST /api/account/login
```

Both endpoints return a token in the response.

### Using the Token

Include the token in the `Authorization` header for protected endpoints:

```http
Authorization: Bearer {your-jwt-token}
```

### Token Expiration

Tokens expire after **7 days**. After expiration, you must obtain a new token by logging in again.

### Token Structure

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
  }
}
```

## Endpoints

### Account Endpoints

#### Register a New User

Creates a new user account and returns a JWT token.

**Endpoint:**
```http
POST /api/account/register
```

**Authentication:** Not required

**Request Body:**
```json
{
  "username": "string",
  "password": "string"
}
```

**Request Parameters:**

| Field    | Type   | Required | Description                    |
|----------|--------|----------|--------------------------------|
| username | string | Yes      | Desired username (unique)      |
| password | string | Yes      | User's password                |

**Response:**

**Success (200 OK):**
```json
{
  "username": "testuser",
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9..."
}
```

**Error (400 Bad Request):**
```json
"Username is taken Already"
```

**Example Request:**

```bash
curl -X POST https://localhost:5001/api/account/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john_doe",
    "password": "SecurePass123!"
  }'
```

**Example Response:**

```json
{
  "username": "john_doe",
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJqb2huX2RvZSIsIm5iZiI6MTc3NjMyNTMwMiwiZXhwIjoxNzc2OTMwMTAyLCJpYXQiOjE3NzYzMjUzMDJ9.signature"
}
```

---

#### Login

Authenticates an existing user and returns a JWT token.

**Endpoint:**
```http
POST /api/account/login
```

**Authentication:** Not required

**Request Body:**
```json
{
  "username": "string",
  "password": "string"
}
```

**Request Parameters:**

| Field    | Type   | Required | Description        |
|----------|--------|----------|--------------------|
| username | string | Yes      | User's username    |
| password | string | Yes      | User's password    |

**Response:**

**Success (200 OK):**
```json
{
  "username": "testuser",
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9..."
}
```

**Error (401 Unauthorized):**
```json
"Invalid username"
```
or
```json
"Invalid password"
```

**Example Request:**

```bash
curl -X POST https://localhost:5001/api/account/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john_doe",
    "password": "SecurePass123!"
  }'
```

**Example Response:**

```json
{
  "username": "john_doe",
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJqb2huX2RvZSIsIm5iZiI6MTc3NjMyNTMwMiwiZXhwIjoxNzc2OTMwMTAyLCJpYXQiOjE3NzYzMjUzMDJ9.signature"
}
```

---

### User Endpoints

#### Get All Users

Retrieves a list of all registered users.

**Endpoint:**
```http
GET /api/users
```

**Authentication:** Not required (public endpoint)

**Query Parameters:** None

**Response:**

**Success (200 OK):**
```json
[
  {
    "id": 1,
    "userName": "john_doe"
  },
  {
    "id": 2,
    "userName": "jane_smith"
  }
]
```

**Example Request:**

```bash
curl -X GET https://localhost:5001/api/users
```

**Example Response:**

```json
[
  {
    "id": 1,
    "userName": "testuser"
  },
  {
    "id": 2,
    "userName": "john_doe"
  },
  {
    "id": 3,
    "userName": "jane_smith"
  }
]
```

**Notes:**
- Password hash and salt are never returned
- Returns an empty array `[]` if no users exist

---

#### Get User by ID

Retrieves details of a specific user by their ID.

**Endpoint:**
```http
GET /api/users/{id}
```

**Authentication:** Required (JWT token)

**Path Parameters:**

| Parameter | Type    | Required | Description           |
|-----------|---------|----------|-----------------------|
| id        | integer | Yes      | The ID of the user    |

**Headers:**
```http
Authorization: Bearer {your-jwt-token}
```

**Response:**

**Success (200 OK):**
```json
{
  "id": 1,
  "userName": "john_doe"
}
```

**Error (401 Unauthorized):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401
}
```

**Error (404 Not Found):**
Returns `null` if user not found.

**Example Request:**

```bash
curl -X GET https://localhost:5001/api/users/1 \
  -H "Authorization: Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9..."
```

**Example Response:**

```json
{
  "id": 1,
  "userName": "testuser"
}
```

---

## Error Handling

### HTTP Status Codes

| Status Code | Meaning                 | Description                                      |
|-------------|-------------------------|--------------------------------------------------|
| 200         | OK                      | Request succeeded                                |
| 400         | Bad Request             | Invalid request data or validation error         |
| 401         | Unauthorized            | Missing or invalid authentication token          |
| 404         | Not Found               | Resource not found                               |
| 500         | Internal Server Error   | Server-side error                                |

### Error Response Format

**Validation Error (400):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Username": [
      "The Username field is required."
    ],
    "Password": [
      "The Password field is required."
    ]
  }
}
```

**Unauthorized Error (401):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401
}
```

**Simple Error Message:**
```json
"Username is taken Already"
```

## Examples

### Complete Registration Flow

```bash
# 1. Register a new user
curl -X POST https://localhost:5001/api/account/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "newuser",
    "password": "MySecurePass123!"
  }'

# Response:
# {
#   "username": "newuser",
#   "token": "eyJhbGci..."
# }

# 2. Use the token to access protected endpoints
TOKEN="eyJhbGci..."

curl -X GET https://localhost:5001/api/users/1 \
  -H "Authorization: Bearer $TOKEN"
```

### Complete Login Flow

```bash
# 1. Login with existing credentials
curl -X POST https://localhost:5001/api/account/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "Test123!"
  }'

# Response:
# {
#   "username": "testuser",
#   "token": "eyJhbGci..."
# }

# 2. Store the token
TOKEN="eyJhbGci..."

# 3. Get all users (no auth required)
curl -X GET https://localhost:5001/api/users

# 4. Get specific user (auth required)
curl -X GET https://localhost:5001/api/users/1 \
  -H "Authorization: Bearer $TOKEN"
```

### JavaScript/TypeScript Example

```typescript
// Register
async function register(username: string, password: string) {
  const response = await fetch('https://localhost:5001/api/account/register', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ username, password }),
  });
  
  const data = await response.json();
  localStorage.setItem('token', data.token);
  return data;
}

// Login
async function login(username: string, password: string) {
  const response = await fetch('https://localhost:5001/api/account/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ username, password }),
  });
  
  const data = await response.json();
  localStorage.setItem('token', data.token);
  return data;
}

// Get users with authentication
async function getUser(id: number) {
  const token = localStorage.getItem('token');
  
  const response = await fetch(`https://localhost:5001/api/users/${id}`, {
    headers: {
      'Authorization': `Bearer ${token}`,
    },
  });
  
  return await response.json();
}

// Get all users (no auth)
async function getAllUsers() {
  const response = await fetch('https://localhost:5001/api/users');
  return await response.json();
}
```

### C# Example

```csharp
using System.Net.Http;
using System.Net.Http.Json;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    
    public ApiClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:5001/api/")
        };
    }
    
    public async Task<UserDto> Register(string username, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("account/register", 
            new { username, password });
        
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }
    
    public async Task<UserDto> Login(string username, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("account/login", 
            new { username, password });
        
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }
    
    public async Task<List<AppUser>> GetUsers()
    {
        return await _httpClient.GetFromJsonAsync<List<AppUser>>("users");
    }
    
    public async Task<AppUser> GetUser(int id, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
        
        return await _httpClient.GetFromJsonAsync<AppUser>($"users/{id}");
    }
}
```

## Testing

### Using Swagger UI

1. Start the API: `dotnet run` (from API directory)
2. Navigate to: `https://localhost:5001/swagger`
3. Expand an endpoint
4. Click "Try it out"
5. Fill in parameters
6. Click "Execute"
7. View the response

### Using curl

**Test Registration:**
```bash
curl -k -X POST https://localhost:5001/api/account/register \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"Test123!"}'
```

**Test Login:**
```bash
curl -k -X POST https://localhost:5001/api/account/login \
  -H "Content-Type: application/json" \
  -d '{"username":"testuser","password":"Test123!"}'
```

**Test Get Users:**
```bash
curl -k https://localhost:5001/api/users
```

**Test Get User by ID (with token):**
```bash
TOKEN="your-jwt-token-here"
curl -k https://localhost:5001/api/users/1 \
  -H "Authorization: Bearer $TOKEN"
```

### Using Postman

1. **Import Collection:** Create a new collection "Dating App API"

2. **Setup Environment Variables:**
   - `baseUrl`: `https://localhost:5001/api`
   - `token`: (will be set after login)

3. **Create Requests:**

   - **Register**
     - Method: POST
     - URL: `{{baseUrl}}/account/register`
     - Body (JSON):
       ```json
       {
         "username": "testuser",
         "password": "Test123!"
       }
       ```
     - Tests (save token):
       ```javascript
       pm.environment.set("token", pm.response.json().token);
       ```

   - **Login**
     - Method: POST
     - URL: `{{baseUrl}}/account/login`
     - Body (JSON):
       ```json
       {
         "username": "testuser",
         "password": "Test123!"
       }
       ```
     - Tests (save token):
       ```javascript
       pm.environment.set("token", pm.response.json().token);
       ```

   - **Get Users**
     - Method: GET
     - URL: `{{baseUrl}}/users`

   - **Get User by ID**
     - Method: GET
     - URL: `{{baseUrl}}/users/1`
     - Authorization: Bearer Token = `{{token}}`

## Rate Limiting

**Current:** No rate limiting implemented

**Recommendation for Production:**
- Implement rate limiting middleware
- Limit: 100 requests per minute per IP
- Use `AspNetCoreRateLimit` NuGet package

## Pagination

**Current:** No pagination implemented

**Recommendation for Future:**
```http
GET /api/users?page=1&pageSize=10
```

**Response:**
```json
{
  "items": [...],
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 5,
  "totalCount": 47
}
```

## Filtering and Sorting

**Future Enhancement:**

```http
GET /api/users?gender=male&minAge=18&maxAge=30&sortBy=age
```

## Versioning

**Current:** No explicit versioning (v1 implicit)

**Future Recommendation:**
- URL Versioning: `/api/v2/users`
- Header Versioning: `X-API-Version: 2.0`

## Support

For API support:
- Review [README.md](../README.md)
- Check [SETUP.md](./SETUP.md) for configuration
- See [ARCHITECTURE.md](./ARCHITECTURE.md) for design details
- Open an issue on GitHub

---

**API Version:** 1.0  
**Last Updated:** 2026-04-16  
**Documentation Format:** Markdown
