using Microsoft.AspNetCore.Mvc;
using MvcAlimentosApp.Models;

namespace MvcAlimentosApp.Controllers
{
    public class AuthController : Controller
    {
        private static List<Usuario> listaUsuarios = new List<Usuario>
        {
            new Usuario { Username = "admin", Password = "1234", Rol = "Admin" },
            new Usuario { Username = "wilson", Password = "abcd", Rol = "Usuario" }
        };

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            var usuario = listaUsuarios
                .FirstOrDefault(u => u.Username == Username && u.Password == Password);

            if (usuario != null)
            {
                HttpContext.Session.SetString("Usuario", usuario.Username);
                HttpContext.Session.SetString("Rol", usuario.Rol);
                return RedirectToAction("Index", "Alimentos");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
