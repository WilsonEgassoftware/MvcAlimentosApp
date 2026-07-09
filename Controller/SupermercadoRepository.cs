using MvcAlimentosApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MvcAlimentosApp.Repositories
{
    public class SupermercadoRepository : ISupermercadoRepository
    {
        private readonly List<Supermercado> _supermercados;

        public SupermercadoRepository()
        {
            // Datos quemados para simular la base de datos
            _supermercados = new List<Supermercado>
            {
                new Supermercado { Id = 1, Nombre = "Supermaxi", Direccion = "Av. Eloy Alfaro y Granados", Ciudad = "Quito" },
                new Supermercado { Id = 2, Nombre = "Mi Comisariato", Direccion = "Av. 6 de Diciembre", Ciudad = "Quito" },
                new Supermercado { Id = 3, Nombre = "Santa María", Direccion = "Av. de la Prensa", Ciudad = "Quito" },
                new Supermercado { Id = 4, Nombre = "Tía", Direccion = "Sector El Condado", Ciudad = "Quito" }
            };
        }

        public IEnumerable<Supermercado> GetAll() => _supermercados;

        public Supermercado? GetById(int id) => _supermercados.FirstOrDefault(s => s.Id == id);
    }
}
