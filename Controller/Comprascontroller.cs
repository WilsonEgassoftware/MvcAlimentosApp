using Microsoft.AspNetCore.Mvc;
using MvAlimentosApp.Data;
using MvAlimentosApp.Models;
using System.Linq;

namespace MvAlimentosApp.Controllers
{
    public class ComprasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var compras = _context.Compras
                .OrderByDescending(c => c.Fecha)
                .ToList();

            return View(compras);
        }

        public IActionResult Detalle(int id)
        {
            var compra = _context.Compras.FirstOrDefault(c => c.Id == id);

            if (compra == null)
                return NotFound();

            return View(compra);
        }
    }
}
