using SupermarketAPI.DTOs;

namespace SupermarketAPI.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDTO>> GetAllSuppliersAsync();
        Task<SupplierDTO?> GetSupplierByIdAsync(int id);
        Task<SupplierDTO> CreateSupplierAsync(CreateSupplierDTO dto);
        Task<SupplierDTO?> UpdateSupplierAsync(int id, CreateSupplierDTO dto);
        Task<bool> DeleteSupplierAsync(int id);
    }
}
