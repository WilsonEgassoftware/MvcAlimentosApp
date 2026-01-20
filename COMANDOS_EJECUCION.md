# 🚀 Comandos para Ejecutar el Proyecto

## ⚠️ Problema con Espacios en la Ruta

Si tu ruta tiene espacios (como `ASUS TUF F15`), usa comillas en PowerShell.

---

## 📍 Opción 1: Desde la Raíz del Proyecto

```powershell
# Navegar a Backend (con comillas si hay espacios)
cd "C:\Users\ASUS TUF F15\Desktop\salvame\MvcAlimentosApp\Backend"

# Restaurar paquetes (si es necesario)
dotnet restore

# Ejecutar el proyecto
dotnet run
```

---

## 📍 Opción 2: Usando Ruta Relativa

Si ya estás en la carpeta raíz del proyecto (`MvcAlimentosApp`):

```powershell
# Verificar dónde estás
Get-Location

# Si estás en MvcAlimentosApp, navegar a Backend
cd .\Backend

# Ejecutar
dotnet run
```

---

## 📍 Opción 3: Ejecutar Directamente con Ruta Completa

```powershell
# Ejecutar directamente desde cualquier ubicación
cd "C:\Users\ASUS TUF F15\Desktop\salvame\MvcAlimentosApp\Backend" ; dotnet run
```

---

## ✅ Verificar que el Servidor Está Corriendo

Una vez ejecutado `dotnet run`, deberías ver algo como:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

### Acceder a Swagger:
- **HTTPS:** https://localhost:5001/swagger
- **HTTP:** http://localhost:5000/swagger

---

## 🔧 Si Hay Errores

### Error: "No se puede encontrar el proyecto"
```powershell
# Verificar que estás en la carpeta correcta
Get-Location

# Debería mostrar: ...\MvcAlimentosApp\Backend
```

### Error: "No se encuentra la ruta"
```powershell
# Usar comillas para rutas con espacios
cd "C:\Users\ASUS TUF F15\Desktop\salvame\MvcAlimentosApp\Backend"
```

### Error: "dotnet no se reconoce"
- Asegúrate de tener .NET 8 SDK instalado
- Verifica con: `dotnet --version`

---

## 🛑 Detener el Servidor

Presiona `Ctrl + C` en la terminal donde está corriendo.

---

## 📝 Notas

- El servidor se ejecuta en segundo plano si usas `;` en PowerShell
- Para ver los logs en tiempo real, ejecuta `dotnet run` sin el `;`
- El seed data se ejecuta automáticamente la primera vez que inicias la app
