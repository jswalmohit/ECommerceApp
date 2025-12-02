using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Data.Entities;

namespace ECommerceApp.Tests.Helpers
{
    public static class TestDataBuilder
    {
        public static ProductEntity CreateProductEntity(string productId = "1", string name = "Test Product", decimal price = 99.99m)
        {
            return new ProductEntity
            {
                ProductId = productId,
                Name = name,
                Description = "Test Description",
                Price = price,
                Category = "Test Category",
                ImageUrl = "https://example.com/image.jpg",
                StockQuantity = 100,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };
        }

        public static ProductResponse CreateProductResponse(string productId = "1", string name = "Test Product", decimal price = 99.99m)
        {
            return new ProductResponse
            {
                ProductId = productId,
                Name = name,
                Description = "Test Description",
                Price = price,
                Category = "Test Category",
                ImageUrl = "https://example.com/image.jpg",
                StockQuantity = 100,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };
        }

        public static CartItemEntity CreateCartItemEntity(int id = 1, int userId = 1, string productId = "1", int quantity = 2)
        {
            return new CartItemEntity
            {
                Id = id,
                UserId = userId,
                ProductId = productId,
                Quantity = quantity,
                CreatedDate = DateTime.UtcNow
            };
        }

        public static CartItemRequest CreateCartItemRequest(string productId = "1", int quantity = 2)
        {
            return new CartItemRequest
            {
                ProductId = productId,
                Quantity = quantity
            };
        }

        public static CartItemResponse CreateCartItemResponse(int id = 1, int userId = 1, string productId = "1", int quantity = 2)
        {
            return new CartItemResponse
            {
                Id = id,
                UserId = userId,
                ProductId = productId,
                ProductName = "Test Product",
                ProductPrice = 99.99m,
                ProductImageUrl = "https://example.com/image.jpg",
                Quantity = quantity,
                SubTotal = 99.99m * quantity,
                CreatedDate = DateTime.UtcNow
            };
        }

        public static CartResponse CreateCartResponse(int userId = 1, int itemCount = 2)
        {
            var items = new List<CartItemResponse>();
            for (int i = 1; i <= itemCount; i++)
            {
                items.Add(CreateCartItemResponse(i, userId, i.ToString(), 2));
            }

            return new CartResponse
            {
                UserId = userId,
                Items = items,
                TotalAmount = items.Sum(i => i.SubTotal),
                TotalItems = items.Sum(i => i.Quantity)
            };
        }

        public static UserEntity CreateUserEntity(int id = 1, string loginId = "testuser")
        {
            return new UserEntity
            {
                Id = id,
                LoginId = loginId,
                Email = "test@example.com",
                FullName = "Test User",
                Credentials = new UserCredentialEntity
                {
                    Id = id,
                    Password = "hashedpassword"
                }
            };
        }
    }
}

