using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Commons.Results;

namespace ECommerceApp.EComm.Services.Interface
{
    public interface ICartService
    {
        Task<ServiceResult<CartItemResponse>> AddItemAsync(int userId, CartItemRequest request);
        Task<ServiceResult> RemoveItemAsync(int userId, int cartItemId);
        Task<ServiceResult> RemoveItemsAsync(int userId, List<int> cartItemIds);
        Task<ServiceResult<CartResponse>> GetCartByUserIdAsync(int userId);
    }
}

