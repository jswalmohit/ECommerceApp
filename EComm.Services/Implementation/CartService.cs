using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Commons.Results;
using ECommerceApp.EComm.Repositories.Interface;
using ECommerceApp.EComm.Services.Interface;

namespace ECommerceApp.EComm.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly ICartRepo _cartRepository;

        public CartService(ICartRepo cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<ServiceResult<CartItemResponse>> AddItemAsync(int userId, CartItemRequest request)
        {
            try
            {
                var result = await _cartRepository.AddItemAsync(userId, request.ProductId, request.Quantity);
                if (result == null)
                {
                    return ServiceResult<CartItemResponse>.Failure("Product not found or inactive", 404);
                }
                return ServiceResult<CartItemResponse>.Success(result);
            }
            catch (Exception ex)
            {
                return ServiceResult<CartItemResponse>.Failure($"Error adding item to cart: {ex.Message}");
            }
        }

        public async Task<ServiceResult> RemoveItemAsync(int userId, int cartItemId)
        {
            try
            {
                var result = await _cartRepository.RemoveItemAsync(userId, cartItemId);
                if (!result)
                {
                    return ServiceResult.Failure("Cart item not found", 404);
                }
                return ServiceResult.Success();
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Error removing item from cart: {ex.Message}");
            }
        }

        public async Task<ServiceResult> RemoveItemsAsync(int userId, List<int> cartItemIds)
        {
            try
            {
                var result = await _cartRepository.RemoveItemsAsync(userId, cartItemIds);
                if (!result)
                {
                    return ServiceResult.Failure("No cart items found to remove", 404);
                }
                return ServiceResult.Success();
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure($"Error removing items from cart: {ex.Message}");
            }
        }

        public async Task<ServiceResult<CartResponse>> GetCartByUserIdAsync(int userId)
        {
            try
            {
                var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                return ServiceResult<CartResponse>.Success(cart);
            }
            catch (Exception ex)
            {
                return ServiceResult<CartResponse>.Failure($"Error retrieving cart: {ex.Message}");
            }
        }
    }
}

