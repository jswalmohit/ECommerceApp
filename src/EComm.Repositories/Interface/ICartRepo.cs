using ECommerceApp.EComm.Commons.Modals;

namespace ECommerceApp.EComm.Repositories.Interface
{
    public interface ICartRepo
    {
        Task<CartItemResponse?> AddItemAsync(int userId, string productId, int quantity);
        Task<bool> RemoveItemAsync(int userId, int cartItemId);
        Task<bool> RemoveItemsAsync(int userId, List<int> cartItemIds);
        Task<CartResponse> GetCartByUserIdAsync(int userId);
        Task<CartItemResponse?> GetCartItemByIdAsync(int cartItemId, int userId);
    }
}

