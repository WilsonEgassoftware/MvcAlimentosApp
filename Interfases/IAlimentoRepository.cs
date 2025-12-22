using MvcAlimentosApp.Models;

namespace MvcAlimentosApp.Interfaces
{
    public interface IAlimentoRepository
    {
        Task<List<Alimento>> GetAllAsync();
        Task<Alimento?> GetByIdAsync(int id);
        Task AddAsync(Alimento alimento);
        Task UpdateAsync(Alimento alimento);
        Task DeleteAsync(int id);
    }
}
