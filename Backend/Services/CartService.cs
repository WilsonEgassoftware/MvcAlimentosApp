using SupermarketAPI.DTOs;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services
{
    public class CartService : ICartService
    {
        private readonly IProductRepository _productRepository;
        // En producción, usarías Redis o una tabla de Carrito en BD
        // Por ahora, usaremos un diccionario en memoria (no persistente entre requests)
        private static readonly Dictionary<int, Dictionary<int, int>> _carts = new();

        public CartService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<CartSummaryDTO> GetCartAsync(int userId)
        {
            var cart = GetOrCreateCart(userId);
            var items = new List<CartItemDTO>();

            foreach (var item in cart)
            {
                var product = await _productRepository.GetByIdAsync(item.Key);
                if (product != null)
                {
                    items.Add(new CartItemDTO
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = item.Value,
                        SubTotal = product.Price * item.Value,
                        ImageUrl = product.ImageUrl,
                        AvailableStock = product.Stock
                    });
                }
            }

            return new CartSummaryDTO
            {
                Items = items,
                TotalAmount = items.Sum(i => i.SubTotal),
                TotalItems = items.Sum(i => i.Quantity)
            };
        }

        public async Task<CartItemDTO> AddToCartAsync(int userId, AddToCartDTO dto)
        {
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new ArgumentException("Product not found");

            if (product.Stock < dto.Quantity)
                throw new InvalidOperationException("Insufficient stock");

            var cart = GetOrCreateCart(userId);
            if (cart.ContainsKey(dto.ProductId))
            {
                var newQuantity = cart[dto.ProductId] + dto.Quantity;
                if (newQuantity > product.Stock)
                    throw new InvalidOperationException("Insufficient stock");
                cart[dto.ProductId] = newQuantity;
            }
            else
            {
                cart[dto.ProductId] = dto.Quantity;
            }

            return new CartItemDTO
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = cart[dto.ProductId],
                SubTotal = product.Price * cart[dto.ProductId],
                ImageUrl = product.ImageUrl,
                AvailableStock = product.Stock
            };
        }

        public async Task<bool> UpdateCartItemAsync(int userId, int productId, UpdateCartItemDTO dto)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return false;

            if (dto.Quantity > product.Stock)
                throw new InvalidOperationException("Insufficient stock");

            var cart = GetOrCreateCart(userId);
            if (!cart.ContainsKey(productId)) return false;

            if (dto.Quantity <= 0)
            {
                cart.Remove(productId);
            }
            else
            {
                cart[productId] = dto.Quantity;
            }

            return true;
        }

        public Task<bool> RemoveFromCartAsync(int userId, int productId)
        {
            var cart = GetOrCreateCart(userId);
            return Task.FromResult(cart.Remove(productId));
        }

        public Task<bool> ClearCartAsync(int userId)
        {
            var cart = GetOrCreateCart(userId);
            cart.Clear();
            return Task.FromResult(true);
        }

        private Dictionary<int, int> GetOrCreateCart(int userId)
        {
            if (!_carts.ContainsKey(userId))
            {
                _carts[userId] = new Dictionary<int, int>();
            }
            return _carts[userId];
        }
    }
}
