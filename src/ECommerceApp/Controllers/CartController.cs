using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddItems([FromBody] CartItemRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            var result = await _cartService.AddItemAsync(userId, request);
            return HandleResult(result);
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
                if (result.IsSuccess && result.Data != null)
                {
                    results.Add(result.Data);
                }
            }

            return Ok(results);
        }

        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var userId = GetUserId();
            var result = await _cartService.RemoveItemAsync(userId, cartItemId);
            return HandleResult(result);
        }

        [HttpPost("remove-multiple")]
        public async Task<IActionResult> RemoveItems([FromBody] List<int> cartItemIds)
        {
            if (cartItemIds == null || !cartItemIds.Any())
                return BadRequest("Cart item IDs are required");

            var userId = GetUserId();
            var result = await _cartService.RemoveItemsAsync(userId, cartItemIds);
            return HandleResult(result);
        }

        [HttpGet("get-cart-items")]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            var result = await _cartService.GetCartByUserIdAsync(userId);
            return HandleResult(result);
        }
    }
}

