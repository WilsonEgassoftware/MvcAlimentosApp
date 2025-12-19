Services\AlimentoService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using MvcAlimentosApp.Models;
using MvcAlimentosApp.Repositories;

namespace MvcAlimentosApp.Services
{
    /// <summary>
    /// Implementación del servicio de Alimentos.
    /// - Encapsula la lógica que hoy reside en el controlador (sin modificarla).
    /// - Permite aplicar OCP/DIP: si cambia la lógica interna, se modifica el servicio sin tocar controladores.
    /// </summary>
    public class AlimentoService : IAlimentoService
    {
        private readonly IAlimentoRepository _repository;

        public AlimentoService(IAlimentoRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Alimento>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Alimento?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task AddAsync(Alimento alimento)
        {
            return _repository.AddAsync(alimento);
        }

        public Task UpdateAsync(Alimento alimento)
        {
            return _repository.UpdateAsync(alimento);
        }

        public Task DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}