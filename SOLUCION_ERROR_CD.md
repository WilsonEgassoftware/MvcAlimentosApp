# 🔧 Solución al Error de `cd Backend`

## ❌ Problema

Cuando ejecutas `cd Backend`, PowerShell busca la carpeta en el directorio actual (`C:\Users\ASUS TUF F15`), no en el directorio del proyecto.

## ✅ Soluciones

### **Opción 1: Usar la ruta completa (RECOMENDADO)**

```powershell
cd "C:\Users\ASUS TUF F15\Desktop\salvame\MvcAlimentosApp\Backend"
dotnet run
```

### **Opción 2: Navegar desde la raíz del proyecto**

Primero asegúrate de estar en la raíz del proyecto:

```powershell
# Ir a la raíz del proyecto
cd "C:\Users\ASUS TUF F15\Desktop\salvame\MvcAlimentosApp"

# Luego ir a Backend
cd Backend

# Ejecutar
dotnet run
```

### **Opción 3: Usar el script PowerShell**

He creado un archivo `ejecutar-backend.ps1` en la raíz del proyecto. Simplemente ejecuta:

```powershell
.\ejecutar-backend.ps1
```

### **Opción 4: Ejecutar directamente con --project**

Desde la raíz del proyecto:

```powershell
cd "C:\Users\ASUS TUF F15\Desktop\salvame\MvcAlimentosApp"
dotnet run --project Backend\SupermarketAPI.csproj
```

---

## 🎯 Comando Definitivo (Copia y Pega)

```powershell
cd "C:\Users\ASUS TUF F15\Desktop\salvame\MvcAlimentosApp\Backend" ; dotnet run
```

---

## ✅ Verificación

Una vez ejecutado, deberías ver:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

Luego abre tu navegador en: **https://localhost:5001/swagger**

---

## 📝 Nota

El servidor está corriendo en segundo plano. Si necesitas detenerlo, presiona `Ctrl + C` en la terminal donde está ejecutándose.
