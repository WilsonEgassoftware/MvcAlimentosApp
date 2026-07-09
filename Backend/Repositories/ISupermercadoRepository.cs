using MvcAlimentosApp.Models;
using System.Collections.Generic;

namespace MvcAlimentosApp.Repositories
{
    public interface ISupermercadoRepository
    {
        IEnumerable<Supermercado> GetAll();
        Supermercado? GetById(int id);
    }
}
