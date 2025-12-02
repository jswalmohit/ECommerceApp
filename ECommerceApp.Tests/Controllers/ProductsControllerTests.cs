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
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductsController(_mockProductService.Object);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnOk_WhenProductsExist()
        {
            // Arrange
            var products = new List<ProductResponse>
            {
                TestDataBuilder.CreateProductResponse("1", "Product 1", 99.99m),
                TestDataBuilder.CreateProductResponse("2", "Product 2", 149.99m)
            };

            var serviceResult = ServiceResult<List<ProductResponse>>.Success(products);
            _mockProductService.Setup(s => s.GetAllProductsAsync())
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnBadRequest_WhenServiceFails()
        {
            // Arrange
            var serviceResult = ServiceResult<List<ProductResponse>>.Failure("Error occurred");
            _mockProductService.Setup(s => s.GetAllProductsAsync())
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetProductById_ShouldReturnOk_WhenProductExists()
        {
            // Arrange
            var productId = "1";
            var product = TestDataBuilder.CreateProductResponse(productId);
            var serviceResult = ServiceResult<ProductResponse>.Success(product);

            _mockProductService.Setup(s => s.GetProductByIdAsync(productId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNotFound_WhenProductNotFound()
        {
            // Arrange
            var productId = "999";
            var serviceResult = ServiceResult<ProductResponse>.Failure("Product not found", 404);

            _mockProductService.Setup(s => s.GetProductByIdAsync(productId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}

