using ECommerceApp.EComm.Commons.Modals;

namespace ECommerceApp.EComm.Services.Interface
{
    public interface ICartService
    {
        Task<CartItemResponse?> AddItemAsync(int userId, CartItemRequest request);
        Task<bool> RemoveItemAsync(int userId, int cartItemId);
        Task<bool> RemoveItemsAsync(int userId, List<int> cartItemIds);
        Task<CartResponse> GetCartByUserIdAsync(int userId);
    }
}

