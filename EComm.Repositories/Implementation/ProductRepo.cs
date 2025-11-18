using ECommerceApp.Context;
using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Data.Entities;
using ECommerceApp.EComm.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.EComm.Repositories.Implementation
{
    public class ProductRepo(EComDbContext context) : IProductRepo
    {
        private readonly EComDbContext _context = context;

        public async Task<List<ProductResponse>> GetAllAsync()
        {
            var entities = await _context.Products
                .Where(p => p.IsActive)
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync();

            return entities.Select(ToDto).ToList();
        }

        public async Task<ProductResponse?> GetByIdAsync(int id)
        {
            var entity = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return entity == null ? null : ToDto(entity);
        }

        private static ProductResponse ToDto(ProductEntity entity)
        {
            return new ProductResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Category = entity.Category,
                ImageUrl = entity.ImageUrl,
                StockQuantity = entity.StockQuantity,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate
            };
        }
    }
}

