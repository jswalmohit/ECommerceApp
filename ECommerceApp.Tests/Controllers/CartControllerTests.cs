using ECommerceApp.Controllers;
using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Commons.Results;
using ECommerceApp.EComm.Services.Interface;
using ECommerceApp.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace ECommerceApp.Tests.Controllers
{
    public class CartControllerTests
    {
        private readonly Mock<ICartService> _mockCartService;
        private readonly CartController _controller;

        public CartControllerTests()
        {
            _mockCartService = new Mock<ICartService>();
            _controller = new CartController(_mockCartService.Object);
            SetupUserContext();
        }

        private void SetupUserContext()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Email, "test@example.com")
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext
            {
                User = principal
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task AddItems_ShouldReturnOk_WhenItemAdded()
        {
            // Arrange
            var request = TestDataBuilder.CreateCartItemRequest("1", 2);
            var cartItemResponse = TestDataBuilder.CreateCartItemResponse(1, 1, "1", 2);
            var serviceResult = ServiceResult<CartItemResponse>.Success(cartItemResponse);

            _mockCartService.Setup(s => s.AddItemAsync(1, request))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.AddItems(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(cartItemResponse);
        }

        [Fact]
        public async Task AddItems_ShouldReturnBadRequest_WhenModelInvalid()
        {
            // Arrange
            var request = new CartItemRequest { ProductId = string.Empty, Quantity = 0 };
            _controller.ModelState.AddModelError("ProductId", "Product ID is required");

            // Act
            var result = await _controller.AddItems(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task AddItems_ShouldReturnNotFound_WhenProductNotFound()
        {
            // Arrange
            var request = TestDataBuilder.CreateCartItemRequest("999", 2);
            var serviceResult = ServiceResult<CartItemResponse>.Failure("Product not found or inactive", 404);

            _mockCartService.Setup(s => s.AddItemAsync(1, request))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.AddItems(request);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task RemoveItem_ShouldReturnOk_WhenItemRemoved()
        {
            // Arrange
            var cartItemId = 1;
            var serviceResult = ServiceResult.Success();

            _mockCartService.Setup(s => s.RemoveItemAsync(1, cartItemId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.RemoveItem(cartItemId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task RemoveItem_ShouldReturnNotFound_WhenItemNotFound()
        {
            // Arrange
            var cartItemId = 999;
            var serviceResult = ServiceResult.Failure("Cart item not found", 404);

            _mockCartService.Setup(s => s.RemoveItemAsync(1, cartItemId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.RemoveItem(cartItemId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetCart_ShouldReturnOk_WhenCartExists()
        {
            // Arrange
            var cartResponse = TestDataBuilder.CreateCartResponse(1, 2);
            var serviceResult = ServiceResult<CartResponse>.Success(cartResponse);

            _mockCartService.Setup(s => s.GetCartByUserIdAsync(1))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetCart();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(cartResponse);
        }

        [Fact]
        public async Task RemoveItems_ShouldReturnBadRequest_WhenIdsEmpty()
        {
            // Arrange
            var cartItemIds = new List<int>();

            // Act
            var result = await _controller.RemoveItems(cartItemIds);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}

