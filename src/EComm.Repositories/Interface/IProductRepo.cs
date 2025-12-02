using ECommerceApp.EComm.Commons.Modals;

namespace ECommerceApp.EComm.Repositories.Interface
{
    public interface IProductRepo
    {
        Task<List<ProductResponse>> GetAllAsync();
        Task<ProductResponse?> GetByIdAsync(string productId);
    }
}

