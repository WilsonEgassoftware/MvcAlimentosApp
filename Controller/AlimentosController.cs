using Microsoft.AspNetCore.Mvc;
using MvcAlimentosApp.Models;

namespace MvcAlimentosApp.Controllers
{
    public class AlimentosController : Controller
    {
        private static List<Alimento> listaAlimentos = new List<Alimento>
        {
            new Alimento { Id = 1, Nombre = "Manzana", Categoria = "Frutas", Empresa = "La Favorita" },
            new Alimento { Id = 2, Nombre = "Arroz", Categoria = "Cereales", Empresa = "Nestle" },
            new Alimento { Id = 3, Nombre = "Leche Descremada", Categoria = "Lácteos", Empresa = "NutriLeche" },
            new Alimento { Id = 4, Nombre = "Pan Integral", Categoria = "Cereales", Empresa = "Panetti" },
            new Alimento { Id = 5, Nombre = "Yogur Natural", Categoria = "Lácteos", Empresa = "Gloria" },
            new Alimento { Id = 6, Nombre = "Pechuga de Pollo", Categoria = "Proteínas", Empresa = "Mr. Pollo" },
            new Alimento { Id = 7, Nombre = "Zanahoria", Categoria = "Verduras", Empresa = "La Huerta" },
            new Alimento { Id = 8, Nombre = "Tomate", Categoria = "Verduras", Empresa = "AgroAndes" },
            new Alimento { Id = 9, Nombre = "Queso Fresco", Categoria = "Lácteos", Empresa = "Los Andes" },
            new Alimento { Id = 10, Nombre = "Avena", Categoria = "Cereales", Empresa = "Quaker" },
            new Alimento { Id = 11, Nombre = "Plátano", Categoria = "Frutas", Empresa = "Chiquita" },
            new Alimento { Id = 12, Nombre = "Pescado Tilapia", Categoria = "Proteínas", Empresa = "AcuAndes" },
            new Alimento { Id = 13, Nombre = "Lechuga", Categoria = "Verduras", Empresa = "La Granja" },
            new Alimento { Id = 14, Nombre = "Huevo", Categoria = "Proteínas", Empresa = "Huevos Kiko" },
            new Alimento { Id = 15, Nombre = "Cereal de Maíz", Categoria = "Cereales", Empresa = "CornFlakes" },
            new Alimento { Id = 16, Nombre = "Papas", Categoria = "Tubérculos", Empresa = "PapasAndinas" },
            new Alimento { Id = 17, Nombre = "Mantequilla", Categoria = "Lácteos", Empresa = "SierraLeche" }
        };

        private bool UsuarioAutenticado()
        {
            return HttpContext.Session.GetString("Usuario") != null;
        }

        private bool EsAdmin()
        {
            return HttpContext.Session.GetString("Rol") == "Admin";
        }

        public IActionResult Index()
        {
            if (!UsuarioAutenticado())
                return RedirectToAction("Login", "Auth");

            ViewBag.EsAdmin = EsAdmin();
            return View(listaAlimentos);
        }

        public IActionResult Create()
        {
            if (!UsuarioAutenticado() || !EsAdmin())
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Alimento alimento)
        {
            if (!EsAdmin())
                return RedirectToAction("Index");

            alimento.Id = listaAlimentos.Max(a => a.Id) + 1;
            listaAlimentos.Add(alimento);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (!UsuarioAutenticado() || !EsAdmin())
                return RedirectToAction("Index");

            var alimento = listaAlimentos.FirstOrDefault(a => a.Id == id);
            return View(alimento);
        }

        [HttpPost]
        public IActionResult Edit(Alimento alimento)
        {
            if (!EsAdmin())
                return RedirectToAction("Index");

            var existente = listaAlimentos.FirstOrDefault(a => a.Id == alimento.Id);
            if (existente != null)
            {
                existente.Nombre = alimento.Nombre;
                existente.Categoria = alimento.Categoria;
                existente.Empresa = alimento.Empresa;
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (!EsAdmin())
                return RedirectToAction("Index");

            var alimento = listaAlimentos.FirstOrDefault(a => a.Id == id);
            if (alimento != null)
                listaAlimentos.Remove(alimento);

            return RedirectToAction("Index");
        }
    }
}
