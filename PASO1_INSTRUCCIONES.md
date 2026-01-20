# 📋 PASO 1: Estructura y Base de Datos - INSTRUCCIONES DE EJECUCIÓN

## ✅ Estado: COMPLETADO

Se ha creado la estructura completa del proyecto con todas las entidades y configuración de base de datos.

---

## 📁 Estructura Creada

```
MvcAlimentosApp/
├── Backend/                          ✅ CREADO
│   ├── Models/                       ✅ 6 Entidades definidas
│   │   ├── Product.cs
│   │   ├── Category.cs
│   │   ├── Supplier.cs
│   │   ├── User.cs
│   │   ├── Order.cs
│   │   └── OrderDetail.cs
│   ├── Data/
│   │   └── ApplicationDbContext.cs   ✅ DbContext configurado
│   ├── Program.cs                    ✅ Configurado con CORS y JWT
│   ├── appsettings.json              ✅ Connection string LocalDB
│   ├── appsettings.Development.json
│   └── SupermarketAPI.csproj        ✅ Paquetes NuGet configurados
│
└── README.md                         ✅ Documentación completa
```

---

## 🚀 COMANDOS PARA EJECUTAR (PowerShell)

### 1. Navegar a la carpeta Backend
```powershell
cd Backend
```

### 2. Restaurar paquetes NuGet
```powershell
dotnet restore
```

### 3. Instalar Entity Framework Core Tools (si no está instalado globalmente)
```powershell
dotnet tool install --global dotnet-ef
```

### 4. Crear la migración inicial
```powershell
dotnet ef migrations add InitialCreate
```

**Nota:** Si aparece un error sobre el DbContext, verifica que el namespace sea correcto. El namespace usado es `SupermarketAPI.Data.ApplicationDbContext`.

### 5. Aplicar las migraciones a la base de datos
```powershell
dotnet ef database update
```

Esto creará la base de datos `SupermarketDB` en LocalDB con todas las tablas y relaciones.

### 6. Ejecutar el proyecto
```powershell
dotnet run
```

El API estará disponible en:
- **HTTP:** `http://localhost:5000`
- **HTTPS:** `https://localhost:5001`
- **Swagger UI:** `https://localhost:5001/swagger`

---

## 🗄️ Modelo de Base de Datos Creado

### Tablas y Relaciones:

1. **Products** (Productos)
   - Campos: Id, Name, Description, Price, Stock, ImageUrl, CategoryId, SupplierId
   - Relaciones: → Category (FK), → Supplier (FK)

2. **Categories** (Categorías)
   - Campos: Id, Name, Description, CreatedAt
   - Relaciones: ← Products (One-to-Many)

3. **Suppliers** (Proveedores)
   - Campos: Id, Name, Contact, Email, Phone, Address, CreatedAt
   - Relaciones: ← Products (One-to-Many)

4. **Users** (Usuarios)
   - Campos: Id, Username, PasswordHash, Email, Role, FullName, CreatedAt, LastLogin
   - Relaciones: ← Orders (One-to-Many)
   - Índices: Username (Unique), Email (Unique)

5. **Orders** (Órdenes de Compra)
   - Campos: Id, UserId, TotalAmount, Status, PaymentMethod, TransactionId, CreatedAt, CompletedAt
   - Relaciones: → User (FK), ← OrderDetails (One-to-Many)
   - Índices: UserId, CreatedAt

6. **OrderDetails** (Detalles de Orden)
   - Campos: Id, OrderId, ProductId, Quantity, UnitPrice, SubTotal
   - Relaciones: → Order (FK), → Product (FK)

---

## ⚙️ Configuración Actual

### Connection String (LocalDB)
```json
"Server=(localdb)\\mssqllocaldb;Database=SupermarketDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

### JWT Settings
```json
{
  "SecretKey": "YourSuperSecretKeyForJWTTokenGenerationThatShouldBeAtLeast32CharactersLongForProductionUseEnvironmentVariables",
  "Issuer": "SupermarketAPI",
  "Audience": "SupermarketClient",
  "ExpirationMinutes": 60
}
```

### CORS Configurado
- Orígenes permitidos: `http://localhost:5173` (Vite) y `http://localhost:3000` (React)
- Métodos: Todos
- Headers: Todos
- Credentials: Permitido

---

## ✅ Verificación

Después de ejecutar `dotnet ef database update`, puedes verificar que la base de datos se creó correctamente:

1. Abre **SQL Server Object Explorer** en Visual Studio
2. Busca `(localdb)\mssqllocaldb`
3. Expande **Databases** → `SupermarketDB`
4. Verifica que existan las 6 tablas: Products, Categories, Suppliers, Users, Orders, OrderDetails

---

## ⚠️ Notas Importantes

1. **LocalDB:** Si no tienes LocalDB instalado, puedes usar SQL Server Express o cambiar el connection string en `appsettings.json`.

2. **Entity Framework Tools:** Si el comando `dotnet ef` no funciona, instálalo globalmente con:
   ```powershell
   dotnet tool install --global dotnet-ef
   ```

3. **Namespace:** Todos los modelos están en el namespace `SupermarketAPI.Models` y el DbContext en `SupermarketAPI.Data`.

4. **Próximo Paso:** Una vez que verifiques que la base de datos se creó correctamente, confirma para proceder con el **PASO 2: Backend (API & Lógica)**.

---

## 📝 Checklist de Verificación

- [ ] Ejecuté `dotnet restore` sin errores
- [ ] Ejecuté `dotnet ef migrations add InitialCreate` exitosamente
- [ ] Ejecuté `dotnet ef database update` y se creó la BD
- [ ] Verifiqué las tablas en SQL Server Object Explorer
- [ ] Ejecuté `dotnet run` y el API inició correctamente
- [ ] Accedí a Swagger UI en `https://localhost:5001/swagger`

---

**¿Listo para continuar?** Confirma cuando hayas completado estos pasos para proceder con el **PASO 2**.
