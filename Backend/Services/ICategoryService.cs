using SupermarketAPI.DTOs;

namespace SupermarketAPI.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO?> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO dto);
        Task<CategoryDTO?> UpdateCategoryAsync(int id, CreateCategoryDTO dto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
