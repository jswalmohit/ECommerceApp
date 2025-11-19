using ECommerceApp.EComm.Data.Context;
using ECommerceApp.EComm.Data.Entities;
using ECommerceApp.EComm.Repositories.Implementation;
using ECommerceApp.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerceApp.Tests.Repositories
{
    public class CartRepoTests : IDisposable
    {
        private readonly EComDbContext _context;
        private readonly CartRepo _cartRepo;

        public CartRepoTests()
        {
            _context = TestDbContextFactory.CreateInMemoryDbContext();
            _cartRepo = new CartRepo(_context);
        }

        [Fact]
        public async Task AddItemAsync_ShouldCreateNewCartItem_WhenItemDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var product = TestDataBuilder.CreateProductEntity(1, "Test Product", 99.99m);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _cartRepo.AddItemAsync(userId, 1, 2);

            // Assert
            result.Should().NotBeNull();
            result!.UserId.Should().Be(userId);
            result.ProductId.Should().Be(1);
            result.Quantity.Should().Be(2);
            result.ProductName.Should().Be("Test Product");

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == 1);
            cartItem.Should().NotBeNull();
            cartItem!.Quantity.Should().Be(2);
        }

        [Fact]
        public async Task AddItemAsync_ShouldUpdateQuantity_WhenItemAlreadyExists()
        {
            // Arrange
            var userId = 1;
            var product = TestDataBuilder.CreateProductEntity(1, "Test Product", 99.99m);
            _context.Products.Add(product);

            var existingCartItem = TestDataBuilder.CreateCartItemEntity(1, userId, 1, 2);
            _context.CartItems.Add(existingCartItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _cartRepo.AddItemAsync(userId, 1, 3);

            // Assert
            result.Should().NotBeNull();
            result!.Quantity.Should().Be(5); // 2 + 3

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.Id == 1);
            cartItem.Should().NotBeNull();
            cartItem!.Quantity.Should().Be(5);
        }

        [Fact]
        public async Task AddItemAsync_ShouldReturnNull_WhenProductNotFound()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = await _cartRepo.AddItemAsync(userId, 999, 2);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddItemAsync_ShouldReturnNull_WhenProductIsInactive()
        {
            // Arrange
            var userId = 1;
            var product = TestDataBuilder.CreateProductEntity(1, "Test Product", 99.99m);
            product.IsActive = false;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _cartRepo.AddItemAsync(userId, 1, 2);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveItemAsync_ShouldReturnTrue_WhenItemRemoved()
        {
            // Arrange
            var userId = 1;
            var cartItem = TestDataBuilder.CreateCartItemEntity(1, userId, 1, 2);
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _cartRepo.RemoveItemAsync(userId, 1);

            // Assert
            result.Should().BeTrue();
            var removedItem = await _context.CartItems.FindAsync(1);
            removedItem.Should().BeNull();
        }

        [Fact]
        public async Task RemoveItemAsync_ShouldReturnFalse_WhenItemNotFound()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = await _cartRepo.RemoveItemAsync(userId, 999);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task RemoveItemAsync_ShouldReturnFalse_WhenItemBelongsToDifferentUser()
        {
            // Arrange
            var userId1 = 1;
            var userId2 = 2;
            var cartItem = TestDataBuilder.CreateCartItemEntity(1, userId1, 1, 2);
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _cartRepo.RemoveItemAsync(userId2, 1);

            // Assert
            result.Should().BeFalse();
            var item = await _context.CartItems.FindAsync(1);
            item.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCartByUserIdAsync_ShouldReturnCartWithTotals()
        {
            // Arrange
            var userId = 1;
            var product1 = TestDataBuilder.CreateProductEntity(1, "Product 1", 50.00m);
            var product2 = TestDataBuilder.CreateProductEntity(2, "Product 2", 75.00m);
            _context.Products.AddRange(product1, product2);

            var cartItem1 = TestDataBuilder.CreateCartItemEntity(1, userId, 1, 2);
            cartItem1.Product = product1;
            var cartItem2 = TestDataBuilder.CreateCartItemEntity(2, userId, 2, 3);
            cartItem2.Product = product2;
            _context.CartItems.AddRange(cartItem1, cartItem2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _cartRepo.GetCartByUserIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.Items.Should().HaveCount(2);
            result.TotalItems.Should().Be(5); // 2 + 3
            result.TotalAmount.Should().Be(325.00m); // (50 * 2) + (75 * 3)
        }

        [Fact]
        public async Task GetCartByUserIdAsync_ShouldReturnEmptyCart_WhenNoItems()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = await _cartRepo.GetCartByUserIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.Items.Should().BeEmpty();
            result.TotalItems.Should().Be(0);
            result.TotalAmount.Should().Be(0);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}

