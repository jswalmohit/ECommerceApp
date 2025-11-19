using ECommerceApp.Controllers;
using ECommerceApp.EComm.Commons.Results;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Xunit;

namespace ECommerceApp.Tests.Controllers
{
    public class BaseControllerTests
    {
        private class TestController : BaseController
        {
            public TestController() { }
        }

        [Fact]
        public void GetUserId_ShouldReturnUserId_WhenValidClaimExists()
        {
            // Arrange
            var controller = new TestController();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext
            {
                User = principal
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var userId = controller.GetUserId();

            // Assert
            userId.Should().Be(123);
        }

        [Fact]
        public void GetUserId_ShouldThrowException_WhenInvalidClaim()
        {
            // Arrange
            var controller = new TestController();
            var claims = new List<Claim>();
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext
            {
                User = principal
            };
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act & Assert
            var action = () => controller.GetUserId();
            action.Should().Throw<UnauthorizedAccessException>()
                .WithMessage("Invalid user ID in token");
        }

        [Fact]
        public void HandleResult_ShouldReturnOk_WhenSuccess()
        {
            // Arrange
            var controller = new TestController();
            var serviceResult = ServiceResult<string>.Success("test data");

            // Act
            var result = controller.HandleResult(serviceResult);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().Be("test data");
        }

        [Fact]
        public void HandleResult_ShouldReturnNotFound_WhenErrorCodeIs404()
        {
            // Arrange
            var controller = new TestController();
            var serviceResult = ServiceResult<string>.Failure("Not found", 404);

            // Act
            var result = controller.HandleResult(serviceResult);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void HandleResult_ShouldReturnUnauthorized_WhenErrorCodeIs401()
        {
            // Arrange
            var controller = new TestController();
            var serviceResult = ServiceResult<string>.Failure("Unauthorized", 401);

            // Act
            var result = controller.HandleResult(serviceResult);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public void HandleResult_ShouldReturnBadRequest_WhenValidationErrorsExist()
        {
            // Arrange
            var controller = new TestController();
            var validationErrors = new Dictionary<string, string[]>
            {
                { "Email", new[] { "Email is required" } }
            };
            var serviceResult = ServiceResult<string>.ValidationFailure(validationErrors);

            // Act
            var result = controller.HandleResult(serviceResult);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}

