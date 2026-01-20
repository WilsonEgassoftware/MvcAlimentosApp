using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SupermarketAPI.DTOs;
using SupermarketAPI.Services;

namespace SupermarketAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            return int.Parse(userIdClaim ?? "0");
        }

        [HttpGet]
        public async Task<ActionResult<CartSummaryDTO>> GetCart()
        {
            var userId = GetUserId();
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<ActionResult<CartItemDTO>> AddToCart([FromBody] AddToCartDTO dto)
        {
            try
            {
                var userId = GetUserId();
                var item = await _cartService.AddToCartAsync(userId, dto);
                return Ok(item);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateCartItem(int productId, [FromBody] UpdateCartItemDTO dto)
        {
            try
            {
                var userId = GetUserId();
                var result = await _cartService.UpdateCartItemAsync(userId, productId, dto);
                if (!result)
                {
                    return NotFound(new { message = "Item not found in cart" });
                }
                return Ok(new { message = "Cart item updated" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = GetUserId();
            var result = await _cartService.RemoveFromCartAsync(userId, productId);
            if (!result)
            {
                return NotFound(new { message = "Item not found in cart" });
            }
            return Ok(new { message = "Item removed from cart" });
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            await _cartService.ClearCartAsync(userId);
            return Ok(new { message = "Cart cleared" });
        }
    }
}
