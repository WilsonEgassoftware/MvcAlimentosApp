using SupermarketAPI.DTOs;

namespace SupermarketAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO?> GetProductByIdAsync(int id);
        Task<ProductDTO> CreateProductAsync(CreateProductDTO dto);
        Task<ProductDTO?> UpdateProductAsync(int id, UpdateProductDTO dto);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ProductDTO>> GetLowStockProductsAsync(int threshold = 5);
    }
}
