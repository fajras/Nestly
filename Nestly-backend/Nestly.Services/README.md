# Nestly.Services Database Configuration

This project contains the Entity Framework Core configuration for the Nestly application with SQL Server support.

## Features

- **Entity Framework Core 8.0** with SQL Server provider
- **Repository Pattern** implementation
- **Unit of Work** pattern for transaction management
- **Configuration-based** database settings
- **Retry policies** for resilience
- **Dependency Injection** ready

## Setup

### 1. Configuration

Add the following to your `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NestlyDB;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
  },
  "Database": {
    "Name": "NestlyDB",
    "Server": "localhost",
    "UserId": "",
    "Password": "",
    "UseIntegratedSecurity": true,
    "CommandTimeout": 30,
    "EnableRetryOnFailure": true,
    "MaxRetryCount": 3,
    "RetryDelaySeconds": 5
  }
}
```

### 2. Service Registration

In your `Program.cs` or `Startup.cs`:

```csharp
using Nestly.Services.Extensions;

// Add database services
builder.Services.AddNestlyDatabase(builder.Configuration);

// Add repositories
builder.Services.AddNestlyRepositories();
```

### 3. Database Migrations

Create and apply migrations:

```bash
# Create migration
dotnet ef migrations add InitialCreate --project Nestly.Services --startup-project Nestly

# Apply migration
dotnet ef database update --project Nestly.Services --startup-project Nestly
```

## Usage

### Using Repository Pattern

```csharp
public class UserService
{
    private readonly IRepository<AppUser> _userRepository;

    public UserService(IRepository<AppUser> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<AppUser> CreateUserAsync(AppUser user)
    {
        return await _userRepository.AddAsync(user);
    }
}
```

### Using Unit of Work

```csharp
public class TransactionService
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateUserWithProfileAsync(AppUser user, BabyProfile profile)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var userRepo = _unitOfWork.Repository<AppUser>();
            var profileRepo = _unitOfWork.Repository<BabyProfile>();

            var createdUser = await userRepo.AddAsync(user);
            profile.ParentId = createdUser.Id;
            await profileRepo.AddAsync(profile);

            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
```

## Architecture

- **NestlyDbContext**: Main DbContext with all entity configurations
- **IRepository<T>**: Generic repository interface
- **Repository<T>**: Generic repository implementation
- **IUnitOfWork**: Unit of Work interface for transaction management
- **UnitOfWork**: Unit of Work implementation
- **IDatabaseConfiguration**: Database configuration interface
- **DatabaseConfiguration**: Database configuration implementation

## Entity Relationships

The DbContext includes configurations for:

- User management (AppUser)
- Baby profiles and growth tracking
- Blog posts and categories
- Chat functionality
- Pregnancy tracking
- Health and medication logs
- Feeding and sleep logs
- Calendar events and milestones

## Best Practices

1. **Use repositories** for data access instead of direct DbContext usage
2. **Use Unit of Work** for operations that require transactions
3. **Configure connection strings** in appsettings.json
4. **Enable retry policies** for production resilience
5. **Use async/await** for all database operations
6. **Dispose of Unit of Work** properly in your services
