# Sistema de Gestión de Inventario y Ventas para Supermercado

Solución completa Full Stack desarrollada con .NET 8 Web API (Backend) y React.js con Vite (Frontend), preparada para despliegue en Azure.

## 📋 Estructura del Proyecto

```
MvcAlimentosApp/
├── Backend/                    # Proyecto ASP.NET Core Web API
│   ├── Models/                 # Entidades de dominio
│   │   ├── Product.cs
│   │   ├── Category.cs
│   │   ├── Supplier.cs
│   │   ├── User.cs
│   │   ├── Order.cs
│   │   └── OrderDetail.cs
│   ├── Data/                   # DbContext y configuración de BD
│   │   └── ApplicationDbContext.cs
│   ├── Controllers/            # Controladores API (PASO 2)
│   ├── Services/               # Lógica de negocio (PASO 2)
│   ├── Repositories/           # Acceso a datos (PASO 2)
│   ├── Program.cs
│   ├── appsettings.json
│   └── SupermarketAPI.csproj
│
├── Frontend/                   # Proyecto React con Vite (PASO 3)
│   ├── src/
│   │   ├── components/
│   │   ├── pages/
│   │   ├── services/
│   │   └── App.jsx
│   ├── package.json
│   └── vite.config.js
│
└── README.md
```

## 🗄️ Modelo de Base de Datos

### Entidades Principales:

1. **Product** (Producto)
   - Id, Name, Description, Price, Stock, ImageUrl
   - Relación: Category (Many-to-One), Supplier (Many-to-One)

2. **Category** (Categoría)
   - Id, Name, Description
   - Relación: Products (One-to-Many)

3. **Supplier** (Proveedor)
   - Id, Name, Contact, Email, Phone, Address
   - Relación: Products (One-to-Many)

4. **User** (Usuario)
   - Id, Username, PasswordHash, Email, Role (Admin/User), FullName
   - Relación: Orders (One-to-Many)

5. **Order** (Orden de Compra)
   - Id, UserId, TotalAmount, Status, PaymentMethod, TransactionId
   - Relación: User (Many-to-One), OrderDetails (One-to-Many)

6. **OrderDetail** (Detalle de Orden)
   - Id, OrderId, ProductId, Quantity, UnitPrice, SubTotal
   - Relación: Order (Many-to-One), Product (Many-to-One)

## 🚀 Comandos de Inicialización

### PASO 1: Configuración del Backend

1. **Navegar a la carpeta Backend:**
   ```powershell
   cd Backend
   ```

2. **Restaurar paquetes NuGet:**
   ```powershell
   dotnet restore
   ```

3. **Crear la migración inicial de Entity Framework:**
   ```powershell
   dotnet ef migrations add InitialCreate --project .
   ```

4. **Aplicar las migraciones a la base de datos:**
   ```powershell
   dotnet ef database update
   ```

5. **Ejecutar el proyecto (puerto 5000/5001):**
   ```powershell
   dotnet run
   ```

   El API estará disponible en:
   - HTTP: `http://localhost:5000`
   - HTTPS: `https://localhost:5001`
   - Swagger UI: `https://localhost:5001/swagger`

### PASO 2: Configuración del Frontend (Pendiente - PASO 3)

```powershell
cd Frontend
npm install
npm run dev
```

## 🔧 Configuración de Base de Datos

### Connection String Local (Development)

El `appsettings.json` está configurado para usar **LocalDB**:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SupermarketDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

### Connection String para Azure SQL Database (Production)

Cuando estés listo para desplegar en Azure, actualiza el connection string en `appsettings.json` o usa **Azure App Service Configuration**:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=tcp:tu-servidor.database.windows.net,1433;Initial Catalog=SupermarketDB;Persist Security Info=False;User ID=tu-usuario;Password=tu-password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
}
```

### Pasos para crear Azure SQL Database:

1. **Crear SQL Server en Azure Portal:**
   - Ir a Azure Portal → Crear recurso → SQL Database
   - Configurar: Nombre del servidor, base de datos, credenciales

2. **Configurar Firewall:**
   - En el SQL Server, ir a "Firewalls and virtual networks"
   - Agregar regla para permitir servicios de Azure
   - Agregar tu IP pública si trabajas desde local

3. **Obtener Connection String:**
   - En la base de datos, ir a "Connection strings"
   - Copiar la cadena ADO.NET
   - Reemplazar `{your_username}` y `{your_password}`

4. **Actualizar en App Service:**
   - En Azure App Service → Configuration → Connection strings
   - Agregar: `DefaultConnection` con el valor de la cadena

## 🔐 Configuración JWT

El JWT está configurado en `appsettings.json`:

```json
"JwtSettings": {
  "SecretKey": "TuClaveSecretaMuyLargaParaProduccion",
  "Issuer": "SupermarketAPI",
  "Audience": "SupermarketClient",
  "ExpirationMinutes": 60
}
```

**⚠️ IMPORTANTE:** En producción, usa **Variables de Entorno** o **Azure Key Vault** para almacenar el SecretKey.

## 📝 Próximos Pasos

- ✅ **PASO 1:** Estructura y Base de Datos (COMPLETADO)
- ⏳ **PASO 2:** Backend (API & Lógica) - Pendiente
- ⏳ **PASO 3:** Frontend (React) - Pendiente
- ⏳ **PASO 4:** Preparación para Azure - Pendiente

## 🛠️ Tecnologías Utilizadas

- **Backend:** .NET 8, ASP.NET Core Web API, Entity Framework Core 8.0
- **Base de Datos:** SQL Server (LocalDB / Azure SQL)
- **Autenticación:** JWT Bearer Tokens
- **Frontend:** React.js, Vite (Pendiente)
- **ORM:** Entity Framework Core (Code First)

---

**Nota:** Este es el PASO 1 completado. Espera confirmación para proceder con el PASO 2 (Backend - Controllers, Services, Repositories).
