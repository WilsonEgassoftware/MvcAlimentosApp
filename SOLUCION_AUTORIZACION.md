# 🔧 Solución: Problema de Autorización 401 Unauthorized

## ✅ Problema Resuelto

He cambiado la autorización de un atributo personalizado a **Políticas de Autorización nativas de ASP.NET Core**, que son más confiables con JWT.

---

## 🔄 Cambios Realizados

### 1. Políticas de Autorización en `Program.cs`
```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("User", "Admin"));
});
```

### 2. Actualización de Controladores
Todos los controladores ahora usan:
- `[Authorize(Policy = "AdminOnly")]` en lugar de `[AuthorizeRoles("Admin")]`

### 3. Configuración Mejorada de Swagger
- Cambiado de `SecuritySchemeType.ApiKey` a `SecuritySchemeType.Http`
- Agregado `BearerFormat = "JWT"`

---

## 🔄 Pasos para Aplicar los Cambios

### 1. Detener el Servidor
Presiona `Ctrl + C` en la terminal donde está corriendo.

### 2. Reiniciar el Servidor
```powershell
dotnet run --project "C:\Users\ASUS TUF F15\Desktop\salvame\MvcAlimentosApp\Backend\SupermarketAPI.csproj"
```

### 3. Refrescar Swagger
- Abre o refresca `https://localhost:5001/swagger`
- Haz login nuevamente con `admin/admin123`
- Copia el nuevo token
- Click en "Authorize" (🔒)
- **IMPORTANTE:** Pega SOLO el token (sin "Bearer")
- O pega: `Bearer [tu-token-completo]`
- Click en "Authorize" y "Close"

---

## ✅ Verificación

Después de reiniciar, prueba estos endpoints:

1. **`GET /api/dashboard`** - Debería funcionar ahora (Admin)
2. **`GET /api/products/low-stock`** - Debería funcionar ahora (Admin)
3. **`GET /api/products`** - Debería seguir funcionando (público)

---

## ⚠️ Nota sobre el Token en Swagger

Cuando pegues el token en el campo "Value" del modal "Authorize":
- **Opción 1:** Pega solo el token: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`
- **Opción 2:** Pega con Bearer: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`

Swagger automáticamente agregará "Bearer " si no lo incluyes.

---

## 🐛 Si Aún No Funciona

1. Verifica que el token no tenga comillas dobles
2. Asegúrate de que el token sea el más reciente (haz login de nuevo)
3. Verifica que el rol en el token sea "Admin" (puedes decodificar el JWT en jwt.io)

---

**Reinicia el servidor y prueba nuevamente.**
