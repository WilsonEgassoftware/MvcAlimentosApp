using SupermarketAPI.DTOs;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISupplierRepository _supplierRepository;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ISupplierRepository supplierRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToDTO);
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : MapToDTO(product);
        }

        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO dto)
        {
            // Validar que Category y Supplier existan
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null)
                throw new ArgumentException("Category not found");

            var supplier = await _supplierRepository.GetByIdAsync(dto.SupplierId);
            if (supplier == null)
                throw new ArgumentException("Supplier not found");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                SupplierId = dto.SupplierId,
                CreatedAt = DateTime.UtcNow
            };

            var createdProduct = await _productRepository.CreateAsync(product);
            return MapToDTO(createdProduct);
        }

        public async Task<ProductDTO?> UpdateProductAsync(int id, UpdateProductDTO dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;

            if (dto.Name != null) product.Name = dto.Name;
            if (dto.Description != null) product.Description = dto.Description;
            if (dto.Price.HasValue) product.Price = dto.Price.Value;
            if (dto.Stock.HasValue) product.Stock = dto.Stock.Value;
            if (dto.ImageUrl != null) product.ImageUrl = dto.ImageUrl;
            if (dto.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(dto.CategoryId.Value);
                if (category == null) throw new ArgumentException("Category not found");
                product.CategoryId = dto.CategoryId.Value;
            }
            if (dto.SupplierId.HasValue)
            {
                var supplier = await _supplierRepository.GetByIdAsync(dto.SupplierId.Value);
                if (supplier == null) throw new ArgumentException("Supplier not found");
                product.SupplierId = dto.SupplierId.Value;
            }

            var updatedProduct = await _productRepository.UpdateAsync(product);
            return MapToDTO(updatedProduct);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProductDTO>> GetLowStockProductsAsync(int threshold = 5)
        {
            var products = await _productRepository.GetLowStockAsync(threshold);
            return products.Select(MapToDTO);
        }

        private ProductDTO MapToDTO(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                SupplierId = product.SupplierId,
                SupplierName = product.Supplier?.Name
            };
        }
    }
}
