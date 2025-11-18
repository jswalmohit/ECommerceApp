using ECommerceApp.EComm.Commons.Modals;
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

        public async Task<CartItemResponse?> AddItemAsync(int userId, CartItemRequest request)
        {
            return await _cartRepository.AddItemAsync(userId, request.ProductId, request.Quantity);
        }

        public async Task<bool> RemoveItemAsync(int userId, int cartItemId)
        {
            return await _cartRepository.RemoveItemAsync(userId, cartItemId);
        }

        public async Task<bool> RemoveItemsAsync(int userId, List<int> cartItemIds)
        {
            return await _cartRepository.RemoveItemsAsync(userId, cartItemIds);
        }

        public async Task<CartResponse> GetCartByUserIdAsync(int userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }
    }
}

