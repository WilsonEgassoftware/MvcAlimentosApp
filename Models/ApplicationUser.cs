using Microsoft.AspNetCore.Identity;

namespace MvcAlimentosApp.Models
{
    // Heredar de IdentityUser asegura que .NET cree los campos de Login y Registro de forma automática
    public class ApplicationUser : IdentityUser
    {
        // Puedes agregar campos personalizados aquí si lo deseas en el futuro
        public string NombreCompleto { get; set; } = string.Empty;
    }
}
