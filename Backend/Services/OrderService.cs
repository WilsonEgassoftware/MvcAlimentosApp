using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.DTOs;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;
using Microsoft.Extensions.Logging;

namespace SupermarketAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartService _cartService;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICartService cartService,
            ApplicationDbContext context,
            IEmailService emailService,
            IUserRepository userRepository,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _cartService = cartService;
            _context = context;
            _emailService = emailService;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(MapToDTO);
        }

        public async Task<IEnumerable<OrderDTO>> GetUserOrdersAsync(int userId)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            return orders.Select(MapToDTO);
        }

        public async Task<OrderDTO?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order == null ? null : MapToDTO(order);
        }

        public async Task<CheckoutResponse> ProcessCheckoutAsync(int userId, CheckoutRequest request)
        {
            // NO validar tarjeta - acepta cualquier número ficticio para desarrollo/testing
            // Aceptamos cualquier valor, incluso vacío, para permitir pruebas
            _logger.LogInformation("ProcessCheckoutAsync iniciado para userId: {UserId}, CardNumber recibido: '{CardNumber}'", 
                userId, request?.CardNumber ?? "null");

            // Obtener carrito
            var cart = await _cartService.GetCartAsync(userId);
            if (cart.Items.Count == 0)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "El carrito está vacío"
                };
            }

            // Simular pago (mock - puede fallar aleatoriamente para pruebas)
            var random = new Random();
            var paymentSuccess = random.Next(1, 10) > 1; // 90% de éxito

            if (!paymentSuccess)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "El procesamiento del pago falló. Por favor, intenta nuevamente."
                };
            }

            // Iniciar transacción (CRÍTICO: validar stock y procesar dentro de la transacción)
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validar stock DENTRO de la transacción para evitar condiciones de carrera
                foreach (var item in cart.Items)
                {
                    // Usar el contexto directamente dentro de la transacción para garantizar consistencia
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product == null)
                    {
                        await transaction.RollbackAsync();
                        return new CheckoutResponse
                        {
                            Success = false,
                            Message = $"Producto no encontrado: {item.ProductName}"
                        };
                    }

                    // Verificar stock suficiente
                    if (product.Stock < item.Quantity)
                    {
                        await transaction.RollbackAsync();
                        return new CheckoutResponse
                        {
                            Success = false,
                            Message = $"Stock insuficiente para el producto: {item.ProductName}. Stock disponible: {product.Stock}, solicitado: {item.Quantity}"
                        };
                    }

                    // Reducir stock dentro de la misma transacción
                    product.Stock -= item.Quantity;
                }

                // Guardar cambios de stock antes de crear la orden
                await _context.SaveChangesAsync();

                // Crear orden
                var order = new Order
                {
                    UserId = userId,
                    TotalAmount = cart.TotalAmount,
                    Status = "Completed",
                    PaymentMethod = request.PaymentMethod ?? "CreditCard",
                    TransactionId = GenerateTransactionId(),
                    CreatedAt = DateTime.UtcNow,
                    CompletedAt = DateTime.UtcNow
                };

                var orderDetails = cart.Items.Select(item => new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    SubTotal = item.SubTotal
                }).ToList();

                order.OrderDetails = orderDetails;

                // Agregar orden y detalles
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Confirmar transacción (Commit)
                await transaction.CommitAsync();

                // Limpiar carrito (fuera de la transacción, después del commit exitoso)
                await _cartService.ClearCartAsync(userId);

                return new CheckoutResponse
                {
                    Success = true,
                    Message = "Orden procesada exitosamente",
                    OrderId = order.Id,
                    TransactionId = order.TransactionId,
                    TotalAmount = order.TotalAmount
                };
            }
            catch (Exception ex)
            {
                // Rollback automático en caso de error
                await transaction.RollbackAsync();
                return new CheckoutResponse
                {
                    Success = false,
                    Message = $"Error al procesar la orden: {ex.Message}"
                };
            }
        }

        // Función de validación removida - aceptamos cualquier número ficticio sin validar

        private string GenerateTransactionId()
        {
            return $"TXN-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        private OrderDTO MapToDTO(Order order)
        {
            return new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User?.Username,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                PaymentMethod = order.PaymentMethod,
                TransactionId = order.TransactionId,
                CreatedAt = order.CreatedAt,
                CompletedAt = order.CompletedAt,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDTO
                {
                    Id = od.Id,
                    ProductId = od.ProductId,
                    ProductName = od.Product?.Name ?? "",
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                    SubTotal = od.SubTotal
                }).ToList()
            };
        }

        public async Task<bool> SendInvoiceAsync(int orderId, int userId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.UserId != userId)
            {
                return false;
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            // Generar HTML de factura
            var invoiceHtml = GenerateInvoiceHtml(order);

            // Enviar email
            try
            {
                await _emailService.SendInvoiceEmailAsync(user.Email, user.Username, invoiceHtml);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GenerateInvoiceHtml(Order order)
        {
            var invoiceHtml = $@"
<div style='font-family: Arial, sans-serif; max-width: 800px; margin: 0 auto; padding: 20px; background-color: #ffffff;'>
    <div style='border-bottom: 3px solid #2563eb; padding-bottom: 20px; margin-bottom: 30px;'>
        <h1 style='color: #2563eb; margin: 0;'>🛒 Supermarket System</h1>
        <h2 style='color: #666; margin: 10px 0 0 0; font-size: 18px;'>Factura de Compra</h2>
    </div>
    
    <div style='margin-bottom: 30px;'>
        <table style='width: 100%; border-collapse: collapse;'>
            <tr>
                <td style='padding: 10px; width: 50%; vertical-align: top;'>
                    <h3 style='margin: 0 0 10px 0; color: #333;'>Información de la Orden</h3>
                    <p style='margin: 5px 0; color: #666;'><strong>Orden #:</strong> {order.Id}</p>
                    <p style='margin: 5px 0; color: #666;'><strong>Transacción:</strong> {order.TransactionId}</p>
                    <p style='margin: 5px 0; color: #666;'><strong>Fecha:</strong> {order.CreatedAt:dd/MM/yyyy HH:mm}</p>
                    <p style='margin: 5px 0; color: #666;'><strong>Estado:</strong> {order.Status}</p>
                </td>
                <td style='padding: 10px; width: 50%; vertical-align: top;'>
                    <h3 style='margin: 0 0 10px 0; color: #333;'>Cliente</h3>
                    <p style='margin: 5px 0; color: #666;'><strong>Nombre:</strong> {order.User?.FullName ?? order.User?.Username ?? "N/A"}</p>
                    <p style='margin: 5px 0; color: #666;'><strong>Email:</strong> {order.User?.Email ?? "N/A"}</p>
                </td>
            </tr>
        </table>
    </div>
    
    <div style='margin-bottom: 30px;'>
        <h3 style='margin: 0 0 15px 0; color: #333; border-bottom: 2px solid #eee; padding-bottom: 10px;'>Detalles de la Compra</h3>
        <table style='width: 100%; border-collapse: collapse;'>
            <thead>
                <tr style='background-color: #f3f4f6;'>
                    <th style='padding: 12px; text-align: left; border-bottom: 2px solid #ddd;'>Producto</th>
                    <th style='padding: 12px; text-align: center; border-bottom: 2px solid #ddd;'>Cantidad</th>
                    <th style='padding: 12px; text-align: right; border-bottom: 2px solid #ddd;'>Precio Unitario</th>
                    <th style='padding: 12px; text-align: right; border-bottom: 2px solid #ddd;'>Subtotal</th>
                </tr>
            </thead>
            <tbody>
";

            foreach (var detail in order.OrderDetails)
            {
                invoiceHtml += $@"
                <tr style='border-bottom: 1px solid #eee;'>
                    <td style='padding: 12px;'>{detail.Product?.Name ?? "N/A"}</td>
                    <td style='padding: 12px; text-align: center;'>{detail.Quantity}</td>
                    <td style='padding: 12px; text-align: right;'>${detail.UnitPrice:F2}</td>
                    <td style='padding: 12px; text-align: right;'>${detail.SubTotal:F2}</td>
                </tr>
";
            }

            invoiceHtml += $@"
            </tbody>
            <tfoot>
                <tr>
                    <td colspan='3' style='padding: 12px; text-align: right; font-weight: bold; font-size: 18px; border-top: 2px solid #2563eb;'>TOTAL:</td>
                    <td style='padding: 12px; text-align: right; font-weight: bold; font-size: 18px; color: #2563eb; border-top: 2px solid #2563eb;'>${order.TotalAmount:F2}</td>
                </tr>
            </tfoot>
        </table>
    </div>
    
    <div style='text-align: center; padding: 20px; background-color: #f9fafb; border-radius: 5px; margin-top: 30px;'>
        <p style='margin: 0; color: #666; font-size: 14px;'>Gracias por tu compra en Supermarket System</p>
        <p style='margin: 5px 0 0 0; color: #999; font-size: 12px;'>© {DateTime.UtcNow.Year} Supermarket System. Todos los derechos reservados.</p>
    </div>
</div>
";

            return invoiceHtml;
        }
    }
}
