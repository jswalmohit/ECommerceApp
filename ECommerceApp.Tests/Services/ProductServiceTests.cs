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
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepo> _mockProductRepo;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockProductRepo = new Mock<IProductRepo>();
            _productService = new ProductService(_mockProductRepo.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnSuccess_WhenProductsExist()
        {
            // Arrange
            var products = new List<ProductResponse>
            {
                TestDataBuilder.CreateProductResponse(1, "Product 1", 99.99m),
                TestDataBuilder.CreateProductResponse(2, "Product 2", 149.99m)
            };

            _mockProductRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
            result.Data.Should().BeEquivalentTo(products);
            _mockProductRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnSuccess_WhenNoProductsExist()
        {
            // Arrange
            var products = new List<ProductResponse>();
            _mockProductRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            _mockProductRepo.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Error retrieving products");
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnSuccess_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var product = TestDataBuilder.CreateProductResponse(productId);

            _mockProductRepo.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(product);
            _mockProductRepo.Verify(repo => repo.GetByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnFailure_WhenProductNotFound()
        {
            // Arrange
            var productId = 999;
            _mockProductRepo.Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((ProductResponse?)null);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Product not found");
            result.ErrorCode.Should().Be(404);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var productId = 1;
            _mockProductRepo.Setup(repo => repo.GetByIdAsync(productId))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Error retrieving product");
        }
    }
}

