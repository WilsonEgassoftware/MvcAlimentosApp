using MvcAlimentosApp.Interfaces;
using MvcAlimentosApp.Models;

namespace MvcAlimentosApp.Services
{
    public class AlimentoService
    {
        private readonly IAlimentoRepository _repository;

        public AlimentoService(IAlimentoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Alimento>> ObtenerTodos()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Alimento?> ObtenerPorId(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task Crear(Alimento alimento)
        {
            await _repository.AddAsync(alimento);
        }

        public async Task Actualizar(Alimento alimento)
        {
            await _repository.UpdateAsync(alimento);
        }

        public async Task Eliminar(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
