using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Product>> GetLowStockAsync(int threshold = 5);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
    }
}
