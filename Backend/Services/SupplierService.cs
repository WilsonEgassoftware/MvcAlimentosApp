using SupermarketAPI.DTOs;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<IEnumerable<SupplierDTO>> GetAllSuppliersAsync()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            return suppliers.Select(MapToDTO);
        }

        public async Task<SupplierDTO?> GetSupplierByIdAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            return supplier == null ? null : MapToDTO(supplier);
        }

        public async Task<SupplierDTO> CreateSupplierAsync(CreateSupplierDTO dto)
        {
            var supplier = new Supplier
            {
                Name = dto.Name,
                Contact = dto.Contact,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                CreatedAt = DateTime.UtcNow
            };

            var createdSupplier = await _supplierRepository.CreateAsync(supplier);
            return MapToDTO(createdSupplier);
        }

        public async Task<SupplierDTO?> UpdateSupplierAsync(int id, CreateSupplierDTO dto)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null) return null;

            supplier.Name = dto.Name;
            supplier.Contact = dto.Contact;
            supplier.Email = dto.Email;
            supplier.Phone = dto.Phone;
            supplier.Address = dto.Address;

            var updatedSupplier = await _supplierRepository.UpdateAsync(supplier);
            return MapToDTO(updatedSupplier);
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            return await _supplierRepository.DeleteAsync(id);
        }

        private SupplierDTO MapToDTO(Supplier supplier)
        {
            return new SupplierDTO
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Contact = supplier.Contact,
                Email = supplier.Email,
                Phone = supplier.Phone,
                Address = supplier.Address
            };
        }
    }
}
