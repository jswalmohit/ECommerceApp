using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid user ID in token");
            }
            return userId;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddItems([FromBody] CartItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            var result = await _cartService.AddItemAsync(userId, request);

            if (result == null)
                return BadRequest("Product not found or inactive");

            return Ok(result);
        }

        [HttpPost("add-multiple")]
        public async Task<IActionResult> AddMultipleItems([FromBody] List<CartItemRequest> requests)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            var results = new List<CartItemResponse>();

            foreach (var request in requests)
            {
                var result = await _cartService.AddItemAsync(userId, request);
                if (result != null)
                {
                    results.Add(result);
                }
            }

            return Ok(results);
        }

        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var userId = GetUserId();
            var result = await _cartService.RemoveItemAsync(userId, cartItemId);

            if (!result)
                return NotFound("Cart item not found");

            return Ok(new { message = "Item removed successfully" });
        }

        [HttpPost("remove-multiple")]
        public async Task<IActionResult> RemoveItems([FromBody] List<int> cartItemIds)
        {
            if (cartItemIds == null || !cartItemIds.Any())
                return BadRequest("Cart item IDs are required");

            var userId = GetUserId();
            var result = await _cartService.RemoveItemsAsync(userId, cartItemIds);

            if (!result)
                return NotFound("No cart items found to remove");

            return Ok(new { message = "Items removed successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            return Ok(cart);
        }
    }
}

