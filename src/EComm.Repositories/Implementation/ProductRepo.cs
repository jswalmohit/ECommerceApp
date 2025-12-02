using ECommerceApp.EComm.Data.Context;
using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Data.Entities;
using ECommerceApp.EComm.Repositories.Interface;
using ECommerceApp.EComm.Repositories.Mappings;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.EComm.Repositories.Implementation
{
    public class ProductRepo : Repository<ProductEntity>, IProductRepo
    {
        private readonly EComDbContext _context;

        public ProductRepo(EComDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ProductResponse>> GetAllAsync()
        {
            var entities = await _context.Products
                .Where(p => p.IsActive)
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync();

            return entities.ToDtoList();
        }

        public async Task<ProductResponse?> GetByIdAsync(string productId)
        {
            var entity = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            return entity?.ToDto();
        }
    }
}

