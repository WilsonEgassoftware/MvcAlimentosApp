using SupermarketAPI.Models;

namespace SupermarketAPI.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Verificar si ya hay datos
            if (context.Users.Any())
            {
                return; // La base de datos ya tiene datos
            }

            // Crear usuarios de prueba (marcados como verificados ya que son seed)
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@supermarket.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin",
                FullName = "Administrator",
                IsEmailConfirmed = true, // Admin seed verificado
                CreatedAt = DateTime.UtcNow
            };

            var regularUser = new User
            {
                Username = "user",
                Email = "user@supermarket.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                Role = "User",
                FullName = "Regular User",
                IsEmailConfirmed = true, // User seed verificado
                CreatedAt = DateTime.UtcNow
            };

            context.Users.AddRange(adminUser, regularUser);

            // Crear categorías de ejemplo
            var categories = new List<Category>
            {
                new Category { Name = "Frutas y Verduras", Description = "Productos frescos de temporada, incluyendo frutas frescas, verduras orgánicas, hortalizas y productos agrícolas locales. Ideal para una alimentación saludable y nutritiva.", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Carnes", Description = "Carnes frescas de res, cerdo, pollo y pescado, así como embutidos de alta calidad. Productos seleccionados cuidadosamente para garantizar frescura y sabor.", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Lácteos", Description = "Productos lácteos frescos como leche entera y descremada, quesos variados, yogures naturales y de sabores, mantequilla y cremas. Ricos en calcio y proteínas.", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Bebidas", Description = "Amplia variedad de bebidas incluyendo refrescos, jugos naturales y procesados, aguas minerales, bebidas energéticas y bebidas alcohólicas para adultos. Hidratación para todos los gustos.", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Panadería", Description = "Productos de panadería frescos diariamente: pan artesanal, pan de molde, pasteles, galletas, bollería y repostería. Elaborados con ingredientes de calidad para el deleite de toda la familia.", CreatedAt = DateTime.UtcNow }
            };

            context.Categories.AddRange(categories);

            // Crear proveedores de ejemplo
            var suppliers = new List<Supplier>
            {
                new Supplier { Name = "Proveedor ABC", Contact = "Juan Pérez", Email = "juan@abc.com", Phone = "123-456-7890", Address = "Calle Principal 123, Ciudad Capital, CP 12345", CreatedAt = DateTime.UtcNow },
                new Supplier { Name = "Distribuidora XYZ", Contact = "María García", Email = "maria@xyz.com", Phone = "098-765-4321", Address = "Avenida Central 456, Zona Industrial, CP 67890", CreatedAt = DateTime.UtcNow },
                new Supplier { Name = "Alimentos Premium", Contact = "Carlos López", Email = "carlos@premium.com", Phone = "555-123-4567", Address = "Boulevard Comercial 789, Distrito Centro, CP 11111", CreatedAt = DateTime.UtcNow }
            };

            context.Suppliers.AddRange(suppliers);

            await context.SaveChangesAsync();

            // Crear productos de ejemplo
            var products = new List<Product>
            {
                new Product { Name = "Manzanas", Description = "Manzanas rojas frescas", Price = 2.50m, Stock = 50, ImageUrl = "https://via.placeholder.com/300", CategoryId = 1, SupplierId = 1, CreatedAt = DateTime.UtcNow },
                new Product { Name = "Leche Entera", Description = "Leche fresca 1L", Price = 3.00m, Stock = 30, ImageUrl = "https://via.placeholder.com/300", CategoryId = 3, SupplierId = 2, CreatedAt = DateTime.UtcNow },
                new Product { Name = "Pan Integral", Description = "Pan de trigo integral", Price = 1.50m, Stock = 20, ImageUrl = "https://via.placeholder.com/300", CategoryId = 5, SupplierId = 3, CreatedAt = DateTime.UtcNow },
                new Product { Name = "Pollo Pechuga", Description = "Pechuga de pollo fresca 500g", Price = 5.50m, Stock = 15, ImageUrl = "https://via.placeholder.com/300", CategoryId = 2, SupplierId = 1, CreatedAt = DateTime.UtcNow },
                new Product { Name = "Agua Mineral", Description = "Agua mineral 500ml", Price = 1.00m, Stock = 100, ImageUrl = "https://via.placeholder.com/300", CategoryId = 4, SupplierId = 2, CreatedAt = DateTime.UtcNow },
                new Product { Name = "Queso Cheddar", Description = "Queso cheddar 250g", Price = 4.50m, Stock = 3, ImageUrl = "https://via.placeholder.com/300", CategoryId = 3, SupplierId = 3, CreatedAt = DateTime.UtcNow }, // Stock bajo para pruebas
                new Product { Name = "Plátanos", Description = "Plátanos maduros", Price = 1.75m, Stock = 2, ImageUrl = "https://via.placeholder.com/300", CategoryId = 1, SupplierId = 1, CreatedAt = DateTime.UtcNow }, // Stock bajo
                new Product { Name = "Refresco Cola", Description = "Refresco de cola 2L", Price = 2.25m, Stock = 40, ImageUrl = "https://via.placeholder.com/300", CategoryId = 4, SupplierId = 2, CreatedAt = DateTime.UtcNow }
            };

            context.Products.AddRange(products);

            await context.SaveChangesAsync();
        }
    }
}
