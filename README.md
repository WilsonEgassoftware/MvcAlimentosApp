# 🛒 Sistema de Gestión de Inventario y Ventas para Supermercado

Solución completa **Full Stack** desarrollada con **.NET 8 Web API** (Backend) y **React.js con Vite** (Frontend), implementando arquitectura en capas, autenticación JWT, y preparada para despliegue en Azure.

---

## 📋 Tabla de Contenidos

- [Características](#-características)
- [Tecnologías Utilizadas](#-tecnologías-utilizadas)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Modelo de Base de Datos](#-modelo-de-base-de-datos)
- [Instalación y Configuración](#-instalación-y-configuración)
- [Endpoints de la API](#-endpoints-de-la-api)
- [Autenticación y Autorización](#-autenticación-y-autorización)
- [Frontend](#-frontend)
- [CI/CD](#-cicd)
- [Configuración de Base de Datos](#-configuración-de-base-de-datos)
- [Seguridad](#-seguridad)

---

## ✨ Características

### Backend (API REST)
- ✅ **Autenticación JWT** con roles (Admin/User)
- ✅ **Verificación de Email** con tokens
- ✅ **Gestión de Productos** (CRUD completo)
- ✅ **Subida de Imágenes** a Azure Blob Storage
- ✅ **Gestión de Categorías** (CRUD completo)
- ✅ **Gestión de Proveedores** (CRUD completo)
- ✅ **Carrito de Compras** en memoria (por usuario)
- ✅ **Sistema de Órdenes** con procesamiento de pago (mock)
- ✅ **Dashboard Administrativo** con estadísticas
- ✅ **Control de Stock** con validación transaccional
- ✅ **Envío de Facturas** por email (HTML)
- ✅ **Arquitectura en Capas** (Controllers → Services → Repositories)
- ✅ **DTOs** para transferencia de datos
- ✅ **Swagger UI** para documentación de API
- ✅ **CORS** configurado para React
- ✅ **Seed Data** automático en desarrollo

### Frontend (React + Vite)
- ✅ **Interfaz React** moderna con Tailwind CSS
- ✅ **Autenticación** (Login/Registro)
- ✅ **Verificación de Email**
- ✅ **Catálogo de Productos** público
- ✅ **Subida de Imágenes** desde el panel admin
- ✅ **Carrito de Compras** persistente
- ✅ **Proceso de Checkout** completo
- ✅ **Dashboard Admin** con estadísticas
- ✅ **Gestión de Productos** (Admin)
- ✅ **Gestión de Proveedores** (Admin)
- ✅ **Context API** para estado global
- ✅ **Rutas Protegidas** por rol
- ✅ **Responsive Design**

---

## 🛠️ Tecnologías Utilizadas

### Backend
- **.NET 8** - Framework principal
- **ASP.NET Core Web API** - Framework web
- **Entity Framework Core 8.0** - ORM (Code First)
- **SQL Server** - Base de datos
- **Azure Blob Storage** - Almacenamiento de imágenes
- **JWT Bearer Authentication** - Autenticación
- **MailKit** - Envío de emails
- **Swagger/OpenAPI** - Documentación de API

### Frontend
- **React 18** - Biblioteca UI
- **Vite** - Build tool y dev server
- **Tailwind CSS** - Framework CSS
- **Axios** - Cliente HTTP
- **React Router** - Enrutamiento
- **Context API** - Gestión de estado

### DevOps
- **GitHub Actions** - CI/CD
- **Azure App Service** - Hosting del backend
- **Azure Static Web Apps** - Hosting del frontend
- **Azure SQL Database** - Base de datos en la nube
- **Azure Blob Storage** - Almacenamiento de archivos
- **.gitignore** - Protección de secretos
- **Docker** - Containerización (preparado)

---

## 📁 Estructura del Proyecto

```
MvcAlimentosApp/
├── Backend/                          # Proyecto ASP.NET Core Web API
│   ├── Controllers/                  # Controladores API REST
│   │   ├── AuthController.cs        # Autenticación (Login/Registro)
│   │   ├── ProductsController.cs    # Gestión de productos
│   │   ├── CategoriesController.cs  # Gestión de categorías
│   │   ├── SuppliersController.cs   # Gestión de proveedores
│   │   ├── CartController.cs        # Carrito de compras
│   │   ├── OrdersController.cs      # Órdenes y checkout
│   │   └── DashboardController.cs  # Dashboard admin
│   ├── Services/                    # Lógica de negocio
│   │   ├── AuthService.cs           # Autenticación y JWT
│   │   ├── ProductService.cs        # Lógica de productos
│   │   ├── CategoryService.cs       # Lógica de categorías
│   │   ├── SupplierService.cs      # Lógica de proveedores
│   │   ├── CartService.cs          # Lógica del carrito
│   │   ├── OrderService.cs         # Procesamiento de órdenes
│   │   ├── DashboardService.cs     # Estadísticas
│   │   ├── EmailService.cs         # Envío de emails
│   │   └── BlobService.cs          # Subida de imágenes a Azure Blob Storage
│   ├── Repositories/                # Acceso a datos
│   │   ├── UserRepository.cs
│   │   ├── ProductRepository.cs
│   │   ├── CategoryRepository.cs
│   │   ├── SupplierRepository.cs
│   │   └── OrderRepository.cs
│   ├── Models/                      # Entidades de dominio
│   │   ├── Product.cs
│   │   ├── Category.cs
│   │   ├── Supplier.cs
│   │   ├── User.cs
│   │   ├── Order.cs
│   │   └── OrderDetail.cs
│   ├── DTOs/                        # Data Transfer Objects
│   │   ├── LoginRequest.cs / LoginResponse.cs
│   │   ├── RegisterRequest.cs
│   │   ├── ProductDTO.cs
│   │   ├── CategoryDTO.cs
│   │   ├── SupplierDTO.cs
│   │   ├── CartItemDTO.cs
│   │   ├── OrderDTO.cs
│   │   └── DashboardDTO.cs
│   ├── Data/                        # DbContext y Seed
│   │   ├── ApplicationDbContext.cs
│   │   └── DbInitializer.cs        # Datos de prueba
│   ├── Attributes/                  # Atributos personalizados
│   │   └── AuthorizeRolesAttribute.cs
│   ├── Migrations/                  # Migraciones EF Core
│   ├── Program.cs                   # Configuración principal
│   ├── appsettings.json            # Configuración (NO subir)
│   ├── appsettings.example.json    # Plantilla de configuración
│   └── SupermarketAPI.csproj
│
├── Frontend/                        # Proyecto React con Vite
│   ├── src/
│   │   ├── components/              # Componentes reutilizables
│   │   │   ├── Navbar.jsx
│   │   │   ├── ProductCard.jsx
│   │   │   ├── CartDrawer.jsx
│   │   │   └── ProtectedRoute.jsx
│   │   ├── pages/                   # Páginas principales
│   │   │   ├── Login.jsx
│   │   │   ├── Register.jsx
│   │   │   ├── VerifyEmail.jsx
│   │   │   ├── Products.jsx
│   │   │   ├── Cart.jsx
│   │   │   ├── Checkout.jsx
│   │   │   ├── Dashboard.jsx
│   │   │   ├── AdminProducts.jsx
│   │   │   └── AdminSuppliers.jsx
│   │   ├── context/                # Context API
│   │   │   ├── AuthContext.jsx
│   │   │   └── CartContext.jsx
│   │   ├── services/               # Servicios API
│   │   │   └── api.js              # Cliente Axios
│   │   ├── hooks/                  # Custom hooks
│   │   │   └── useLowStockCount.js
│   │   ├── App.jsx                 # Componente principal
│   │   └── main.jsx                # Punto de entrada
│   ├── package.json
│   └── vite.config.js
│
├── .github/
│   └── workflows/
│       └── dotnet.yml               # GitHub Actions CI/CD
│
├── .gitignore                       # Archivos excluidos de Git
└── README.md                        # Este archivo
```

---

## 🗄️ Modelo de Base de Datos

### Entidades Principales:

1. **Product** (Producto)
   - `Id`, `Name`, `Description`, `Price`, `Stock`, `ImageUrl`
   - Relación: `Category` (Many-to-One), `Supplier` (Many-to-One)
   - **Imágenes:** Almacenadas en Azure Blob Storage con acceso público

2. **Category** (Categoría)
   - `Id`, `Name`, `Description`, `CreatedAt`
   - Relación: `Products` (One-to-Many)

3. **Supplier** (Proveedor)
   - `Id`, `Name`, `Contact`, `Email`, `Phone`, `Address`, `CreatedAt`
   - Relación: `Products` (One-to-Many)

4. **User** (Usuario)
   - `Id`, `Username`, `PasswordHash`, `Email`, `Role` (Admin/User), `FullName`, `IsEmailVerified`, `EmailVerificationToken`, `CreatedAt`, `LastLogin`
   - Relación: `Orders` (One-to-Many)
   - Índices: `Username` (Unique), `Email` (Unique)

5. **Order** (Orden de Compra)
   - `Id`, `UserId`, `TotalAmount`, `Status`, `PaymentMethod`, `TransactionId`, `CreatedAt`, `CompletedAt`
   - Relación: `User` (Many-to-One), `OrderDetails` (One-to-Many)
   - Índices: `UserId`, `CreatedAt`

6. **OrderDetail** (Detalle de Orden)
   - `Id`, `OrderId`, `ProductId`, `Quantity`, `UnitPrice`, `SubTotal`
   - Relación: `Order` (Many-to-One), `Product` (Many-to-One)

---

## 🚀 Instalación y Configuración

### Prerrequisitos

- **.NET 8 SDK** - [Descargar](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js 18+** y **npm** - [Descargar](https://nodejs.org/)
- **SQL Server** (LocalDB, Express, o SQL Server completo)
- **Git** - [Descargar](https://git-scm.com/)

### Backend

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/WilsonEgassoftware/MvcAlimentosApp.git
   cd MvcAlimentosApp/Backend
   ```

2. **Configurar la base de datos:**
   - Copiar `appsettings.example.json` a `appsettings.json`
   - Editar `appsettings.json` con tu connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=TU_SERVIDOR;Database=SuperMarketProyect;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   }
   ```

3. **Restaurar paquetes NuGet:**
   ```powershell
   dotnet restore
   ```

4. **Aplicar migraciones:**
   ```powershell
   dotnet ef database update
   ```

5. **Ejecutar el proyecto:**
   ```powershell
   dotnet run
   ```

   El API estará disponible en:
   - **HTTP:** `http://localhost:5000`
   - **HTTPS:** `https://localhost:5001`
   - **Swagger UI:** `https://localhost:5001/swagger`

### Frontend

1. **Navegar a la carpeta Frontend:**
   ```powershell
   cd Frontend
   ```

2. **Instalar dependencias:**
   ```powershell
   npm install
   ```

3. **Ejecutar servidor de desarrollo:**
   ```powershell
   npm run dev
   ```

   El frontend estará disponible en:
   - **URL:** `http://localhost:5173`

---

## 🔌 Endpoints de la API

### 🔐 Autenticación (`/api/auth`)

| Método | Endpoint | Descripción | Autorización |
|--------|----------|-------------|--------------|
| POST | `/api/auth/login` | Iniciar sesión | Público |
| POST | `/api/auth/register` | Registrar nuevo usuario | Público |
| POST | `/api/auth/verify-email` | Verificar email con token | Público |

### 📦 Productos (`/api/products`)

| Método | Endpoint | Descripción | Autorización |
|--------|----------|-------------|--------------|
| GET | `/api/products` | Listar todos los productos | Público |
| GET | `/api/products/{id}` | Obtener producto por ID | Público |
| GET | `/api/products/low-stock?threshold=5` | Productos con stock bajo | Admin |
| POST | `/api/products` | Crear nuevo producto | Admin |
| POST | `/api/products/upload-image` | Subir imagen de producto | Admin |
| PUT | `/api/products/{id}` | Actualizar producto | Admin |
| DELETE | `/api/products/{id}` | Eliminar producto | Admin |

### 📂 Categorías (`/api/categories`)

| Método | Endpoint | Descripción | Autorización |
|--------|----------|-------------|--------------|
| GET | `/api/categories` | Listar todas las categorías | Público |
| GET | `/api/categories/{id}` | Obtener categoría por ID | Público |
| POST | `/api/categories` | Crear nueva categoría | Admin |
| PUT | `/api/categories/{id}` | Actualizar categoría | Admin |
| DELETE | `/api/categories/{id}` | Eliminar categoría | Admin |

### 🏢 Proveedores (`/api/suppliers`)

| Método | Endpoint | Descripción | Autorización |
|--------|----------|-------------|--------------|
| GET | `/api/suppliers` | Listar todos los proveedores | Admin |
| GET | `/api/suppliers/{id}` | Obtener proveedor por ID | Admin |
| POST | `/api/suppliers` | Crear nuevo proveedor | Admin |
| PUT | `/api/suppliers/{id}` | Actualizar proveedor | Admin |
| DELETE | `/api/suppliers/{id}` | Eliminar proveedor | Admin |

### 🛒 Carrito (`/api/cart`)

| Método | Endpoint | Descripción | Autorización |
|--------|----------|-------------|--------------|
| GET | `/api/cart` | Obtener carrito del usuario | User/Admin |
| POST | `/api/cart/add` | Agregar producto al carrito | User/Admin |
| PUT | `/api/cart/{productId}` | Actualizar cantidad | User/Admin |
| DELETE | `/api/cart/{productId}` | Remover producto | User/Admin |
| DELETE | `/api/cart/clear` | Limpiar carrito | User/Admin |

### 📋 Órdenes (`/api/orders`)

| Método | Endpoint | Descripción | Autorización |
|--------|----------|-------------|--------------|
| GET | `/api/orders` | Listar todas las órdenes | Admin |
| GET | `/api/orders/my-orders` | Mis órdenes | User/Admin |
| GET | `/api/orders/{id}` | Obtener orden por ID | User/Admin |
| POST | `/api/orders/checkout` | Procesar pago y crear orden | User/Admin |
| POST | `/api/orders/{id}/invoice` | Enviar factura por email | User/Admin |

### 📊 Dashboard (`/api/dashboard`)

| Método | Endpoint | Descripción | Autorización |
|--------|----------|-------------|--------------|
| GET | `/api/dashboard` | Estadísticas generales | Admin |

---

## 🔐 Autenticación y Autorización

### JWT Configuration

El sistema utiliza **JWT Bearer Tokens** para autenticación. Configuración en `appsettings.json`:

```json
"JwtSettings": {
  "SecretKey": "TU_CLAVE_SECRETA_DE_AL_MENOS_32_CARACTERES",
  "Issuer": "SupermarketAPI",
  "Audience": "SupermarketClient",
  "ExpirationMinutes": 60
}
```

### Roles

- **Admin:** Acceso completo a todas las funcionalidades
- **User:** Acceso a catálogo, carrito y sus propias órdenes

### Usuarios de Prueba (Seed Data)

Al ejecutar la aplicación en modo desarrollo, se crean automáticamente:

**Administrador:**
- Username: `admin`
- Password: `admin123`
- Email: `admin@supermarket.com`
- Role: `Admin`

**Usuario:**
- Username: `user`
- Password: `user123`
- Email: `user@supermarket.com`
- Role: `User`

### Uso de Tokens

1. **Login:** `POST /api/auth/login` retorna un token JWT
2. **Autorización:** Incluir el token en el header:
   ```
   Authorization: Bearer {tu_token_jwt}
   ```
3. **Swagger:** Usar el botón "Authorize" en Swagger UI para autenticarse

---

## 🎨 Frontend

### Características

- **React 18** con hooks modernos
- **Tailwind CSS** para estilos
- **Context API** para estado global (Auth, Cart)
- **React Router** para navegación
- **Axios** para llamadas API
- **Rutas Protegidas** por rol

### Páginas Principales

- **Login/Registro** - Autenticación de usuarios
- **Verificación de Email** - Confirmación de cuenta
- **Catálogo de Productos** - Vista pública de productos
- **Carrito de Compras** - Gestión de items
- **Checkout** - Proceso de pago (mock)
- **Dashboard Admin** - Estadísticas y gestión
- **Gestión de Productos** - CRUD (Admin) con subida de imágenes
- **Gestión de Proveedores** - CRUD (Admin)

---

## 🔄 CI/CD

### GitHub Actions

El proyecto incluye workflows de CI/CD configurados:

- ✅ **Backend:** `.github/workflows/main_app-mvcalimentos-sc.yml`
  - Build y deploy automático a Azure App Service
  - Se ejecuta en cada push a `main`

- ✅ **Frontend:** `.github/workflows/azure-static-web-apps-icy-river-0e8ac9b0f.yml`
  - Build y deploy automático a Azure Static Web Apps
  - Se ejecuta en cada push a `main`

- ✅ **Build/Test:** `.github/workflows/dotnet.yml`
  - Build y validación de código

### Protección de Secretos

- ✅ **`.gitignore`** configurado para excluir:
  - `appsettings.json` (con secretos)
  - `appsettings.*.json` (excepto `.example.json`)
  - `bin/`, `obj/`, `node_modules/`
- ✅ **`appsettings.example.json`** incluido como plantilla

---

## 🗄️ Configuración de Base de Datos

### Connection String

El proyecto está configurado para conectarse a SQL Server. Edita `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=TU_BASE_DE_DATOS;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

### Migraciones

Las migraciones de Entity Framework Core están incluidas. Para crear nuevas migraciones:

```powershell
dotnet ef migrations add NombreMigracion --project Backend
dotnet ef database update --project Backend
```

---

## ☁️ Configuración de Azure Blob Storage

### Para Desarrollo Local

Agrega la configuración en `appsettings.json`:

```json
"Blob": {
  "ConnectionString": "TU_CONNECTION_STRING_DE_AZURE_STORAGE",
  "ContainerName": "product-images"
}
```

### Para Producción (Azure App Service)

Configura las siguientes **Application Settings** en Azure Portal:

1. **`Blob__ConnectionString`** (con doble guion bajo `__`)
   - Valor: Connection string completa de tu Storage Account
   - Obtener desde: Azure Portal → Storage Account → Access keys

2. **`Blob__ContainerName`** (con doble guion bajo `__`)
   - Valor: `product-images`

### Configuración del Contenedor

1. Crea un contenedor llamado `product-images` en tu Storage Account
2. Habilita **"Allow Blob anonymous access"** en la configuración del Storage Account
3. Configura el contenedor con acceso público **"Blob"** (anonymous read access for blobs only)

### Características

- ✅ Validación de tipos de archivo (jpg, jpeg, png, gif, webp)
- ✅ Validación de tamaño máximo (5MB)
- ✅ Generación automática de nombres únicos (GUID)
- ✅ URLs públicas para acceso directo a las imágenes
- ✅ Manejo de errores tolerante (la app funciona aunque Blob Storage no esté configurado)

---

## 🔒 Seguridad

### Implementado

- ✅ **JWT Authentication** con expiración
- ✅ **Password Hashing** con BCrypt
- ✅ **CORS** configurado para orígenes específicos
- ✅ **Autorización por Roles** (Admin/User)
- ✅ **Verificación de Email** con tokens
- ✅ **Protección de Secretos** en Git
- ✅ **Validación de Stock** transaccional
- ✅ **HTTPS** en producción

### Recomendaciones para Producción

- ✅ Usar **Variables de Entorno** en Azure App Service para secretos
- ✅ Configurar **HTTPS** obligatorio
- ✅ **Azure Blob Storage** configurado con acceso público para imágenes
- ⚠️ Implementar **Rate Limiting**
- ⚠️ Configurar **Logging** y **Monitoring**
- ✅ **Azure SQL Database** con firewall configurado

---

## 📝 Estado del Proyecto

### ✅ Completado

- [x] Backend API REST completo
- [x] Autenticación JWT
- [x] Verificación de Email
- [x] Gestión de Productos, Categorías, Proveedores
- [x] **Subida de Imágenes a Azure Blob Storage**
- [x] Sistema de Carrito y Órdenes
- [x] Dashboard Administrativo
- [x] Frontend React completo
- [x] CI/CD con GitHub Actions
- [x] **Despliegue en Azure** (App Service + Static Web Apps)
- [x] Documentación Swagger
- [x] Seed Data para desarrollo

### 🚀 Próximas Mejoras

- [ ] Tests unitarios e integración
- [ ] Implementar paginación en endpoints
- [ ] Búsqueda y filtros avanzados
- [ ] Notificaciones en tiempo real
- [ ] Reportes y exportación de datos
- [ ] Optimización de imágenes (compresión, thumbnails)

---

