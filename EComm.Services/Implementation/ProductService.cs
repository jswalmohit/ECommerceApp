using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Repositories.Interface;
using ECommerceApp.EComm.Services.Interface;

namespace ECommerceApp.EComm.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepository;

        public ProductService(IProductRepo productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<ProductResponse?> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }
    }
}

