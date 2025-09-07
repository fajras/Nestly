# Nestly WebAPI - AppUser CRUD Operations

This API provides full CRUD (Create, Read, Update, Delete) operations for AppUser entities.

## Database Setup

The application automatically creates the database on startup if it doesn't exist. Make sure you have SQL Server running locally.

## API Endpoints

### Get All Users

```
GET /api/AppUser
GET /api/AppUser?email=john@example.com&username=john
```

### Get User by ID

```
GET /api/AppUser/{id}
```

### Get User by Email

```
GET /api/AppUser/email/{email}
```

### Get User by Username

```
GET /api/AppUser/username/{username}
```

### Create User

```
POST /api/AppUser
Content-Type: application/json

{
  "username": "john_doe",
  "email": "john@example.com",
  "firstName": "John",
  "lastName": "Doe"
}
```

### Update User

```
PUT /api/AppUser/{id}
Content-Type: application/json

{
  "id": 1,
  "username": "john_doe_updated",
  "email": "john.updated@example.com",
  "firstName": "John",
  "lastName": "Doe Updated"
}
```

### Delete User

```
DELETE /api/AppUser/{id}
```

### Check if User Exists

```
GET /api/AppUser/{id}/exists
```

## Features

- ✅ **Full CRUD Operations**: Create, Read, Update, Delete
- ✅ **Search Functionality**: Filter by email and username
- ✅ **Validation**: Checks for duplicate emails and usernames
- ✅ **Error Handling**: Proper HTTP status codes and error messages
- ✅ **Async Operations**: All operations are asynchronous
- ✅ **Database Auto-Creation**: Database is created automatically on startup

## Database Connection

The application uses the following connection string:

```
Server=localhost;Database=NestlyDB;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true
```

## Running the Application

1. Ensure SQL Server is running locally
2. Run the application: `dotnet run`
3. The database will be created automatically
4. Access the API at: `https://localhost:7000/api/AppUser`
5. View Swagger documentation at: `https://localhost:7000/swagger`

## Testing with Swagger

1. Navigate to `/swagger` in your browser
2. Test the API endpoints directly from the Swagger UI
3. All endpoints are documented with examples
