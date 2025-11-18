using ECommerceApp.EComm.Commons.Modals;

namespace ECommerceApp.EComm.Services.Interface
{
    public interface IProductService
    {
        Task<List<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse?> GetProductByIdAsync(int id);
    }
}

