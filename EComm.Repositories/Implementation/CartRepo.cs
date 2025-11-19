using ECommerceApp.Context;
using ECommerceApp.EComm.Commons.Mappings;
using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Data.Entities;
using ECommerceApp.EComm.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.EComm.Repositories.Implementation
{
    public class CartRepo : ICartRepo
    {
        private readonly EComDbContext _context;

        public CartRepo(EComDbContext context)
        {
            _context = context;
        }

        public async Task<CartItemResponse?> AddItemAsync(int userId, int productId, int quantity)
        {
            // Check if product exists
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productId && p.IsActive);

            if (product == null)
                return null;

            // Check if item already exists in cart
            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (existingCartItem != null)
            {
                // Update quantity
                existingCartItem.Quantity += quantity;
                existingCartItem.UpdatedDate = DateTime.UtcNow;
            }
            else
            {
                // Create new cart item
                existingCartItem = new CartItemEntity
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    CreatedDate = DateTime.UtcNow
                };
                _context.CartItems.Add(existingCartItem);
            }

            await _context.SaveChangesAsync();

            // Return the cart item with product details
            var updatedItem = await _context.CartItems
                .Include(c => c.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == existingCartItem.Id && c.UserId == userId);

            return updatedItem?.ToDto();
        }

        public async Task<bool> RemoveItemAsync(int userId, int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

            if (cartItem == null)
                return false;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveItemsAsync(int userId, List<int> cartItemIds)
        {
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId && cartItemIds.Contains(c.Id))
                .ToListAsync();

            if (!cartItems.Any())
                return false;

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CartResponse> GetCartByUserIdAsync(int userId)
        {
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            var items = cartItems.ToDtoList();
            var totalAmount = items.Sum(i => i.SubTotal);
            var totalItems = items.Sum(i => i.Quantity);

            return new CartResponse
            {
                UserId = userId,
                Items = items,
                TotalAmount = totalAmount,
                TotalItems = totalItems
            };
        }

        public async Task<CartItemResponse?> GetCartItemByIdAsync(int cartItemId, int userId)
        {
            var cartItem = await _context.CartItems
                .Include(c => c.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

            return cartItem?.ToDto();
        }

    }
}

