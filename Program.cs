var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
// Registrar el Repositorio como Singleton para este ejercicio (mantiene los datos vivos en memoria)
builder.Services.AddSingleton<ISupermercadoRepository, SupermercadoRepository>();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
