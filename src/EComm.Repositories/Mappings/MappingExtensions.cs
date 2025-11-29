using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Data.Entities;

namespace ECommerceApp.EComm.Repositories.Mappings;

public static class MappingExtensions
{
    // Product Mappings
    public static ProductResponse ToDto(this ProductEntity entity)
    {
        return new ProductResponse
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            Category = entity.Category,
            ImageUrl = entity.ImageUrl,
            StockQuantity = entity.StockQuantity,
            IsActive = entity.IsActive,
            CreatedDate = entity.CreatedDate,
            UpdatedDate = entity.UpdatedDate
        };
    }

    public static List<ProductResponse> ToDtoList(this IEnumerable<ProductEntity> entities)
    {
        return entities.Select(ToDto).ToList();
    }

    // User Mappings
    public static UserRequest ToDto(this UserEntity entity)
    {
        return new UserRequest
        {
            Id = entity.Id,
            LoginId = entity.LoginId,
            Email = entity.Email,
            FullName = entity.FullName,
            Credentials = entity.Credentials == null
                ? new LoginModel()
                : new LoginModel
                {
                    Password = entity.Credentials.Password
                }
        };
    }

    // Cart Mappings
    public static CartItemResponse ToDto(this CartItemEntity entity)
    {
        return new CartItemResponse
        {
            Id = entity.Id,
            UserId = entity.UserId,
            ProductId = entity.ProductId,
            ProductName = entity.Product?.Name ?? string.Empty,
            ProductPrice = entity.Product?.Price ?? 0,
            ProductImageUrl = entity.Product?.ImageUrl ?? string.Empty,
            Quantity = entity.Quantity,
            SubTotal = (entity.Product?.Price ?? 0) * entity.Quantity,
            CreatedDate = entity.CreatedDate,
            UpdatedDate = entity.UpdatedDate
        };
    }

    public static List<CartItemResponse> ToDtoList(this IEnumerable<CartItemEntity> entities)
    {
        return entities.Select(ToDto).ToList();
    }
}


