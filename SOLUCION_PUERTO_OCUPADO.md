# 🔧 Solución: Puerto 5000 Ocupado

## ✅ Problema Resuelto

El puerto 5000 estaba siendo usado por otro proceso (PID 3224). Ya fue detenido.

---

## 🚀 Ejecutar el Servidor

Ahora puedes ejecutar el servidor con:

```powershell
dotnet run --project "C:\Users\ASUS TUF F15\Desktop\salvame\MvcAlimentosApp\Backend\SupermarketAPI.csproj"
```

---

## 🔍 Si Vuelve a Pasar

Si el error vuelve a aparecer, ejecuta estos comandos:

### 1. Encontrar el proceso que usa el puerto 5000:
```powershell
netstat -ano | findstr :5000
```

### 2. Detener el proceso (reemplaza PID con el número que aparezca):
```powershell
taskkill /F /PID [PID]
```

### 3. O detener todos los procesos dotnet:
```powershell
taskkill /F /IM dotnet.exe
```

---

## 📝 Alternativa: Cambiar el Puerto

Si prefieres usar otro puerto, edita `Backend/Properties/launchSettings.json` y cambia:

```json
"applicationUrl": "http://localhost:5000"
```

Por ejemplo, a:

```json
"applicationUrl": "http://localhost:5002"
```

---

## ✅ Verificación

Una vez que el servidor inicie, deberías ver:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

Luego abre: **https://localhost:5001/swagger**
