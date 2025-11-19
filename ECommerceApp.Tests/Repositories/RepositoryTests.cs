using ECommerceApp.EComm.Data.Context;
using ECommerceApp.EComm.Data.Entities;
using ECommerceApp.EComm.Repositories.Implementation;
using ECommerceApp.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerceApp.Tests.Repositories
{
    public class RepositoryTests : IDisposable
    {
        private readonly EComDbContext _context;
        private readonly Repository<ProductEntity> _repository;

        public RepositoryTests()
        {
            _context = TestDbContextFactory.CreateInMemoryDbContext();
            _repository = new Repository<ProductEntity>(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            var product = TestDataBuilder.CreateProductEntity(1);

            // Act
            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            // Assert
            var result = await _context.Products.FindAsync(1);
            result.Should().NotBeNull();
            result!.Name.Should().Be("Test Product");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenExists()
        {
            // Arrange
            var product = TestDataBuilder.CreateProductEntity(1);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEntities()
        {
            // Arrange
            var products = new List<ProductEntity>
            {
                TestDataBuilder.CreateProductEntity(1),
                TestDataBuilder.CreateProductEntity(2),
                TestDataBuilder.CreateProductEntity(3)
            };
            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task FindAsync_ShouldReturnMatchingEntities()
        {
            // Arrange
            var product1 = TestDataBuilder.CreateProductEntity(1, "Product A", 99.99m);
            var product2 = TestDataBuilder.CreateProductEntity(2, "Product B", 149.99m);
            _context.Products.AddRange(product1, product2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.FindAsync(p => p.Price > 100);

            // Assert
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Product B");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntity()
        {
            // Arrange
            var product = TestDataBuilder.CreateProductEntity(1);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            product.Name = "Updated Product";
            product.Price = 199.99m;

            // Act
            await _repository.UpdateAsync(product);
            await _repository.SaveChangesAsync();

            // Assert
            var updated = await _context.Products.FindAsync(1);
            updated!.Name.Should().Be("Updated Product");
            updated.Price.Should().Be(199.99m);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            // Arrange
            var product = TestDataBuilder.CreateProductEntity(1);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(product);
            await _repository.SaveChangesAsync();

            // Assert
            var deleted = await _context.Products.FindAsync(1);
            deleted.Should().BeNull();
        }

        [Fact]
        public async Task CountAsync_ShouldReturnCorrectCount()
        {
            // Arrange
            var products = new List<ProductEntity>
            {
                TestDataBuilder.CreateProductEntity(1),
                TestDataBuilder.CreateProductEntity(2)
            };
            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            // Act
            var count = await _repository.CountAsync();

            // Assert
            count.Should().Be(2);
        }

        [Fact]
        public async Task CountAsync_WithPredicate_ShouldReturnFilteredCount()
        {
            // Arrange
            var product1 = TestDataBuilder.CreateProductEntity(1, "Product A", 99.99m);
            var product2 = TestDataBuilder.CreateProductEntity(2, "Product B", 149.99m);
            _context.Products.AddRange(product1, product2);
            await _context.SaveChangesAsync();

            // Act
            var count = await _repository.CountAsync(p => p.Price > 100);

            // Assert
            count.Should().Be(1);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}

