using ECommerceApp.EComm.Commons.Results;
using FluentAssertions;
using Xunit;

namespace ECommerceApp.Tests.Commons
{
    public class ServiceResultTests
    {
        [Fact]
        public void Success_ShouldCreateSuccessfulResult()
        {
            // Arrange
            var data = "test data";

            // Act
            var result = ServiceResult<string>.Success(data);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().Be(data);
            result.ErrorMessage.Should().BeNull();
            result.ErrorCode.Should().BeNull();
        }

        [Fact]
        public void Failure_ShouldCreateFailedResult()
        {
            // Arrange
            var errorMessage = "Error occurred";

            // Act
            var result = ServiceResult<string>.Failure(errorMessage);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeNull();
            result.ErrorMessage.Should().Be(errorMessage);
            result.ErrorCode.Should().BeNull();
        }

        [Fact]
        public void Failure_WithErrorCode_ShouldCreateFailedResultWithCode()
        {
            // Arrange
            var errorMessage = "Not found";
            var errorCode = 404;

            // Act
            var result = ServiceResult<string>.Failure(errorMessage, errorCode);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be(errorMessage);
            result.ErrorCode.Should().Be(errorCode);
        }

        [Fact]
        public void ValidationFailure_ShouldCreateFailedResultWithValidationErrors()
        {
            // Arrange
            var validationErrors = new Dictionary<string, string[]>
            {
                { "Email", new[] { "Email is required" } },
                { "Password", new[] { "Password must be at least 8 characters" } }
            };

            // Act
            var result = ServiceResult<string>.ValidationFailure(validationErrors);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ValidationErrors.Should().NotBeNull();
            result.ValidationErrors.Should().BeEquivalentTo(validationErrors);
            result.ErrorMessage.Should().Be("Validation failed");
        }

        [Fact]
        public void ServiceResult_Success_ShouldCreateSuccessfulResult()
        {
            // Act
            var result = ServiceResult.Success();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();
            result.ErrorCode.Should().BeNull();
        }

        [Fact]
        public void ServiceResult_Failure_ShouldCreateFailedResult()
        {
            // Arrange
            var errorMessage = "Operation failed";

            // Act
            var result = ServiceResult.Failure(errorMessage);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be(errorMessage);
        }

        [Fact]
        public void ServiceResult_ValidationFailure_ShouldCreateFailedResultWithValidationErrors()
        {
            // Arrange
            var validationErrors = new Dictionary<string, string[]>
            {
                { "Field", new[] { "Error message" } }
            };

            // Act
            var result = ServiceResult.ValidationFailure(validationErrors);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ValidationErrors.Should().NotBeNull();
            result.ValidationErrors.Should().BeEquivalentTo(validationErrors);
        }
    }
}

