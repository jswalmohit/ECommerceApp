using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Commons.Results;
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

        public async Task<ServiceResult<List<ProductResponse>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return ServiceResult<List<ProductResponse>>.Success(products);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<ProductResponse>>.Failure($"Error retrieving products: {ex.Message}");
            }
        }

        public async Task<ServiceResult<ProductResponse>> GetProductByIdAsync(string productId)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    return ServiceResult<ProductResponse>.Failure("Product not found", 404);
                }
                return ServiceResult<ProductResponse>.Success(product);
            }
            catch (Exception ex)
            {
                return ServiceResult<ProductResponse>.Failure($"Error retrieving product: {ex.Message}");
            }
        }
    }
}

