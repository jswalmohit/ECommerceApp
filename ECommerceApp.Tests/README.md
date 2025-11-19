# ECommerceApp.Tests

This project contains unit tests for the ECommerceApp application.

## Test Structure

### Helpers
- **TestDataBuilder**: Provides helper methods to create test data (entities, DTOs, etc.)
- **TestDbContextFactory**: Creates in-memory database contexts for repository testing

### Services Tests
- **ProductServiceTests**: Tests for ProductService business logic
- **CartServiceTests**: Tests for CartService business logic

### Repositories Tests
- **ProductRepoTests**: Tests for ProductRepo data access layer (uses in-memory database)
- **CartRepoTests**: Tests for CartRepo data access layer (uses in-memory database)

### Controllers Tests
- **ProductsControllerTests**: Tests for ProductsController endpoints
- **CartControllerTests**: Tests for CartController endpoints
- **BaseControllerTests**: Tests for BaseController common functionality

## Running Tests

### Using .NET CLI
```bash
dotnet test
```

### Using Visual Studio
1. Open Test Explorer (Test > Test Explorer)
2. Run All Tests

### Using Visual Studio Code
1. Install .NET Test Explorer extension
2. Run tests from the Test Explorer panel

## Test Coverage

The tests cover:
- ✅ Service layer business logic
- ✅ Repository layer data access
- ✅ Controller endpoints and HTTP responses
- ✅ Base controller functionality
- ✅ Success scenarios
- ✅ Error handling scenarios
- ✅ Edge cases

## Dependencies

- **xUnit**: Testing framework
- **Moq**: Mocking framework for dependencies
- **FluentAssertions**: Fluent assertion library
- **Microsoft.EntityFrameworkCore.InMemory**: In-memory database for repository tests
- **Microsoft.AspNetCore.Mvc.Testing**: Testing utilities for ASP.NET Core

