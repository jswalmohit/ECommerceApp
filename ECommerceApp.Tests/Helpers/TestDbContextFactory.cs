using ECommerceApp.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Tests.Helpers
{
    public static class TestDbContextFactory
    {
        public static EComDbContext CreateInMemoryDbContext(string? dbName = null)
        {
            var options = new DbContextOptionsBuilder<EComDbContext>()
                .UseInMemoryDatabase(databaseName: dbName ?? Guid.NewGuid().ToString())
                .Options;

            return new EComDbContext(options);
        }
    }
}

