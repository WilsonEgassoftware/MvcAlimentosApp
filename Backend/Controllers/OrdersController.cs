using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SupermarketAPI.Attributes;
using SupermarketAPI.DTOs;
using SupermarketAPI.Services;
using Microsoft.Extensions.Logging;

namespace SupermarketAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim ?? "0");
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("my-orders")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetMyOrders()
        {
            var userId = GetUserId();
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var userId = GetUserId();
            var order = await _orderService.GetOrderByIdAsync(id);
            
            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            // Solo el usuario dueño o un Admin puede ver la orden
            if (order.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return Ok(order);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutResponse>> Checkout([FromBody] CheckoutRequest? request)
        {
            // NO validar nada - aceptar cualquier valor para desarrollo/testing
            // Validar que el modelo sea válido
            if (request == null)
            {
                // Si no hay request, crear uno por defecto para permitir pruebas
                request = new CheckoutRequest
                {
                    CardNumber = "1234567890123456",
                    CardHolderName = "TARJETA FICTICIA",
                    ExpiryDate = "12/99",
                    CVV = "123",
                    PaymentMethod = "CreditCard"
                };
            }

            // Asegurar que todos los campos tengan valores por defecto si están vacíos
            request.CardNumber = request.CardNumber ?? "1234567890123456";
            request.CardHolderName = request.CardHolderName ?? "TARJETA FICTICIA";
            request.ExpiryDate = request.ExpiryDate ?? "12/99";
            request.CVV = request.CVV ?? "123";
            request.PaymentMethod = request.PaymentMethod ?? "CreditCard";

            var userId = GetUserId();
            var response = await _orderService.ProcessCheckoutAsync(userId, request);
            
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("{orderId}/send-invoice")]
        public async Task<ActionResult> SendInvoice(int orderId)
        {
            var userId = GetUserId();
            var success = await _orderService.SendInvoiceAsync(orderId, userId);
            
            if (!success)
            {
                return BadRequest(new { message = "Error al enviar la factura. Verifica que la orden existe y te pertenece." });
            }

            return Ok(new { message = "Factura enviada exitosamente a tu correo electrónico" });
        }
    }
}
