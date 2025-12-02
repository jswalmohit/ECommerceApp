using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Commons.Results;
using ECommerceApp.EComm.Repositories.Interface;
using ECommerceApp.EComm.Services.Implementation;
using ECommerceApp.Tests.Helpers;
using FluentAssertions;
using Moq;
using Xunit;

namespace ECommerceApp.Tests.Services
{
    public class CartServiceTests
    {
        private readonly Mock<ICartRepo> _mockCartRepo;
        private readonly CartService _cartService;

        public CartServiceTests()
        {
            _mockCartRepo = new Mock<ICartRepo>();
            _cartService = new CartService(_mockCartRepo.Object);
        }

        [Fact]
        public async Task AddItemAsync_ShouldReturnSuccess_WhenItemAdded()
        {
            // Arrange
            var userId = 1;
            var request = TestDataBuilder.CreateCartItemRequest("1", 2);
            var cartItemResponse = TestDataBuilder.CreateCartItemResponse(1, userId, "1", 2);

            _mockCartRepo.Setup(repo => repo.AddItemAsync(userId, request.ProductId, request.Quantity))
                .ReturnsAsync(cartItemResponse);

            // Act
            var result = await _cartService.AddItemAsync(userId, request);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(cartItemResponse);
            _mockCartRepo.Verify(repo => repo.AddItemAsync(userId, request.ProductId, request.Quantity), Times.Once);
        }

        [Fact]
        public async Task AddItemAsync_ShouldReturnFailure_WhenProductNotFound()
        {
            // Arrange
            var userId = 1;
            var request = TestDataBuilder.CreateCartItemRequest("999", 2);

            _mockCartRepo.Setup(repo => repo.AddItemAsync(userId, request.ProductId, request.Quantity))
                .ReturnsAsync((CartItemResponse?)null);

            // Act
            var result = await _cartService.AddItemAsync(userId, request);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Product not found or inactive");
            result.ErrorCode.Should().Be(404);
        }

        [Fact]
        public async Task AddItemAsync_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var userId = 1;
            var request = TestDataBuilder.CreateCartItemRequest("1", 2);

            _mockCartRepo.Setup(repo => repo.AddItemAsync(userId, request.ProductId, request.Quantity))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _cartService.AddItemAsync(userId, request);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Error adding item to cart");
        }

        [Fact]
        public async Task RemoveItemAsync_ShouldReturnSuccess_WhenItemRemoved()
        {
            // Arrange
            var userId = 1;
            var cartItemId = 1;

            _mockCartRepo.Setup(repo => repo.RemoveItemAsync(userId, cartItemId))
                .ReturnsAsync(true);

            // Act
            var result = await _cartService.RemoveItemAsync(userId, cartItemId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            _mockCartRepo.Verify(repo => repo.RemoveItemAsync(userId, cartItemId), Times.Once);
        }

        [Fact]
        public async Task RemoveItemAsync_ShouldReturnFailure_WhenItemNotFound()
        {
            // Arrange
            var userId = 1;
            var cartItemId = 999;

            _mockCartRepo.Setup(repo => repo.RemoveItemAsync(userId, cartItemId))
                .ReturnsAsync(false);

            // Act
            var result = await _cartService.RemoveItemAsync(userId, cartItemId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Cart item not found");
            result.ErrorCode.Should().Be(404);
        }

        [Fact]
        public async Task RemoveItemsAsync_ShouldReturnSuccess_WhenItemsRemoved()
        {
            // Arrange
            var userId = 1;
            var cartItemIds = new List<int> { 1, 2, 3 };

            _mockCartRepo.Setup(repo => repo.RemoveItemsAsync(userId, cartItemIds))
                .ReturnsAsync(true);

            // Act
            var result = await _cartService.RemoveItemsAsync(userId, cartItemIds);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            _mockCartRepo.Verify(repo => repo.RemoveItemsAsync(userId, cartItemIds), Times.Once);
        }

        [Fact]
        public async Task RemoveItemsAsync_ShouldReturnFailure_WhenNoItemsFound()
        {
            // Arrange
            var userId = 1;
            var cartItemIds = new List<int> { 999, 998 };

            _mockCartRepo.Setup(repo => repo.RemoveItemsAsync(userId, cartItemIds))
                .ReturnsAsync(false);

            // Act
            var result = await _cartService.RemoveItemsAsync(userId, cartItemIds);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("No cart items found to remove");
            result.ErrorCode.Should().Be(404);
        }

        [Fact]
        public async Task GetCartByUserIdAsync_ShouldReturnSuccess_WhenCartExists()
        {
            // Arrange
            var userId = 1;
            var cartResponse = TestDataBuilder.CreateCartResponse(userId, 2);

            _mockCartRepo.Setup(repo => repo.GetCartByUserIdAsync(userId))
                .ReturnsAsync(cartResponse);

            // Act
            var result = await _cartService.GetCartByUserIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(cartResponse);
            _mockCartRepo.Verify(repo => repo.GetCartByUserIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetCartByUserIdAsync_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var userId = 1;

            _mockCartRepo.Setup(repo => repo.GetCartByUserIdAsync(userId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _cartService.GetCartByUserIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Error retrieving cart");
        }
    }
}

