using Microsoft.AspNetCore.Mvc.RazorPages;
using MvAlimentosApp.Models;
using System.Collections.Generic;

namespace MvAlimentosApp.Pages.Descuento
{
    public class IndexModel : PageModel
    {
        // ✅ Esta es la propiedad que la vista utilizará
        public List<Descuento> Descuentos { get; set; } = new List<Descuento>();

        public void OnGet()
        {
            Descuentos = new List<Descuento>
            {
                new Descuento { NombreAlimento="Manzana", Empresa="Frutas SA", Porcentaje=15 },
                new Descuento { NombreAlimento="Leche", Empresa="Lácteos MX", Porcentaje=20 },
                new Descuento { NombreAlimento="Pan", Empresa="Panadería Centro", Porcentaje=25 },
                new Descuento { NombreAlimento="Pollo", Empresa="Granjas del Sol", Porcentaje=30 },
                new Descuento { NombreAlimento="Queso", Empresa="Lácteos MX", Porcentaje=35 },
                new Descuento { NombreAlimento="Huevos", Empresa="Granja Feliz", Porcentaje=40 },
                new Descuento { NombreAlimento="Arroz", Empresa="Campos MX", Porcentaje=18 },
                new Descuento { NombreAlimento="Carne", Empresa="Carnes Prime", Porcentaje=22 },
                new Descuento { NombreAlimento="Pasta", Empresa="Italian Foods", Porcentaje=28 },
                new Descuento { NombreAlimento="Yogurt", Empresa="Lácteos MX", Porcentaje=15 }
            };
        }
    }
}
