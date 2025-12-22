using Microsoft.EntityFrameworkCore;
using MvcAlimentosApp.Data;
using MvcAlimentosApp.Interfaces;
using MvcAlimentosApp.Models;

namespace MvcAlimentosApp.Repositories
{
    public class AlimentoRepository : IAlimentoRepository
    {
        private readonly ApplicationDbContext _context;

        public AlimentoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Alimento>> GetAllAsync()
        {
            return await _context.Alimentos.ToListAsync();
        }

        public async Task<Alimento?> GetByIdAsync(int id)
        {
            return await _context.Alimentos.FindAsync(id);
        }

        public async Task AddAsync(Alimento alimento)
        {
            _context.Alimentos.Add(alimento);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Alimento alimento)
        {
            _context.Alimentos.Update(alimento);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var alimento = await GetByIdAsync(id);
            if (alimento != null)
            {
                _context.Alimentos.Remove(alimento);
                await _context.SaveChangesAsync();
            }
        }
    }
}
