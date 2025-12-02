using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Commons.Results;

namespace ECommerceApp.EComm.Services.Interface
{
    public interface IProductService
    {
        Task<ServiceResult<List<ProductResponse>>> GetAllProductsAsync();
        Task<ServiceResult<ProductResponse>> GetProductByIdAsync(string productId);
    }
}

