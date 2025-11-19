using ECommerceApp.EComm.Data.Context;
using ECommerceApp.EComm.Data.Entities;
using ECommerceApp.EComm.Repositories.Implementation;
using ECommerceApp.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerceApp.Tests.Repositories
{
    public class ProductRepoTests : IDisposable
    {
        private readonly EComDbContext _context;
        private readonly ProductRepo _productRepo;

        public ProductRepoTests()
        {
            _context = TestDbContextFactory.CreateInMemoryDbContext();
            _productRepo = new ProductRepo(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOnlyActiveProducts()
        {
            // Arrange
            var activeProduct = TestDataBuilder.CreateProductEntity(1, "Active Product", 99.99m);
            activeProduct.IsActive = true;

            var inactiveProduct = TestDataBuilder.CreateProductEntity(2, "Inactive Product", 149.99m);
            inactiveProduct.IsActive = false;

            _context.Products.AddRange(activeProduct, inactiveProduct);
            await _context.SaveChangesAsync();

            // Act
            var result = await _productRepo.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Active Product");
            result.First().IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoActiveProducts()
        {
            // Arrange
            var inactiveProduct = TestDataBuilder.CreateProductEntity(1, "Inactive Product", 99.99m);
            inactiveProduct.IsActive = false;

            _context.Products.Add(inactiveProduct);
            await _context.SaveChangesAsync();

            // Act
            var result = await _productRepo.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnProductsOrderedByName()
        {
            // Arrange
            var product1 = TestDataBuilder.CreateProductEntity(1, "Zebra Product", 99.99m);
            var product2 = TestDataBuilder.CreateProductEntity(2, "Apple Product", 149.99m);
            var product3 = TestDataBuilder.CreateProductEntity(3, "Banana Product", 79.99m);

            _context.Products.AddRange(product1, product2, product3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _productRepo.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result[0].Name.Should().Be("Apple Product");
            result[1].Name.Should().Be("Banana Product");
            result[2].Name.Should().Be("Zebra Product");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenExists()
        {
            // Arrange
            var product = TestDataBuilder.CreateProductEntity(1, "Test Product", 99.99m);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _productRepo.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Name.Should().Be("Test Product");
            result.Price.Should().Be(99.99m);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenProductNotFound()
        {
            // Act
            var result = await _productRepo.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}

