# 📋 PASO 2: Backend (API & Lógica) - COMPLETADO

## ✅ Estado: COMPLETADO

Se ha implementado toda la lógica del backend con arquitectura MVC adaptada a API REST.

---

## 📁 Estructura Creada

```
Backend/
├── DTOs/                          ✅ 8 archivos DTO
│   ├── LoginRequest.cs
│   ├── LoginResponse.cs
│   ├── RegisterRequest.cs
│   ├── ProductDTO.cs
│   ├── CategoryDTO.cs
│   ├── SupplierDTO.cs
│   ├── CartItemDTO.cs
│   ├── OrderDTO.cs
│   └── DashboardDTO.cs
│
├── Repositories/                   ✅ Interfaces y Implementaciones
│   ├── IUserRepository.cs / UserRepository.cs
│   ├── IProductRepository.cs / ProductRepository.cs
│   ├── ICategoryRepository.cs / CategoryRepository.cs
│   ├── ISupplierRepository.cs / SupplierRepository.cs
│   └── IOrderRepository.cs / OrderRepository.cs
│
├── Services/                       ✅ Lógica de Negocio
│   ├── IAuthService.cs / AuthService.cs
│   ├── IProductService.cs / ProductService.cs
│   ├── ICategoryService.cs / CategoryService.cs
│   ├── ISupplierService.cs / SupplierService.cs
│   ├── ICartService.cs / CartService.cs
│   ├── IOrderService.cs / OrderService.cs
│   └── IDashboardService.cs / DashboardService.cs
│
├── Controllers/                    ✅ 7 Controladores API
│   ├── AuthController.cs
│   ├── ProductsController.cs
│   ├── CategoriesController.cs
│   ├── SuppliersController.cs
│   ├── CartController.cs
│   ├── OrdersController.cs
│   └── DashboardController.cs
│
├── Attributes/                     ✅ Autorización personalizada
│   └── AuthorizeRolesAttribute.cs
│
└── Data/
    └── DbInitializer.cs           ✅ Datos de prueba (seed)
```

---

## 🔌 Endpoints API Implementados

### 🔐 Autenticación (`/api/auth`)
- `POST /api/auth/login` - Login de usuario (público)
- `POST /api/auth/register` - Registro de nuevo usuario (público)

### 📦 Productos (`/api/products`)
- `GET /api/products` - Listar todos (público)
- `GET /api/products/{id}` - Obtener por ID (público)
- `POST /api/products` - Crear (Admin)
- `PUT /api/products/{id}` - Actualizar (Admin)
- `DELETE /api/products/{id}` - Eliminar (Admin)
- `GET /api/products/low-stock?threshold=5` - Productos con stock bajo (Admin)

### 📂 Categorías (`/api/categories`)
- `GET /api/categories` - Listar todas (público)
- `GET /api/categories/{id}` - Obtener por ID (público)
- `POST /api/categories` - Crear (Admin)
- `PUT /api/categories/{id}` - Actualizar (Admin)
- `DELETE /api/categories/{id}` - Eliminar (Admin)

### 🏢 Proveedores (`/api/suppliers`)
- `GET /api/suppliers` - Listar todos (Admin)
- `GET /api/suppliers/{id}` - Obtener por ID (Admin)
- `POST /api/suppliers` - Crear (Admin)
- `PUT /api/suppliers/{id}` - Actualizar (Admin)
- `DELETE /api/suppliers/{id}` - Eliminar (Admin)

### 🛒 Carrito (`/api/cart`)
- `GET /api/cart` - Obtener carrito del usuario (User/Admin)
- `POST /api/cart/add` - Agregar producto (User/Admin)
- `PUT /api/cart/{productId}` - Actualizar cantidad (User/Admin)
- `DELETE /api/cart/{productId}` - Remover producto (User/Admin)
- `DELETE /api/cart/clear` - Limpiar carrito (User/Admin)

### 📋 Órdenes (`/api/orders`)
- `GET /api/orders` - Listar todas (Admin)
- `GET /api/orders/my-orders` - Mis órdenes (User/Admin)
- `GET /api/orders/{id}` - Obtener orden por ID (User/Admin)
- `POST /api/orders/checkout` - Procesar pago y crear orden (User/Admin)

### 📊 Dashboard (`/api/dashboard`)
- `GET /api/dashboard` - Estadísticas generales (Admin)

---

## 🔑 Usuarios de Prueba (Seed Data)

Al ejecutar la aplicación, se crean automáticamente:

### Administrador
- **Username:** `admin`
- **Password:** `admin123`
- **Email:** `admin@supermarket.com`
- **Role:** `Admin`

### Usuario Regular
- **Username:** `user`
- **Password:** `user123`
- **Email:** `user@supermarket.com`
- **Role:** `User`

---

## 🚀 Cómo Probar el Backend

### 1. Ejecutar el proyecto
```powershell
cd Backend
dotnet run
```

### 2. Acceder a Swagger UI
Abre tu navegador en: `https://localhost:5001/swagger`

### 3. Probar Login
1. En Swagger, ve a `POST /api/auth/login`
2. Click en "Try it out"
3. Usa el body:
```json
{
  "username": "admin",
  "password": "admin123"
}
```
4. Click en "Execute"
5. Copia el `token` de la respuesta

### 4. Autorizar en Swagger
1. Click en el botón "Authorize" (🔒) en la parte superior de Swagger
2. Pega el token en el campo "Value"
3. Click en "Authorize" y luego "Close"

### 5. Probar Endpoints Protegidos
Ahora puedes probar cualquier endpoint que requiera autenticación.

---

## 💳 Mock Payment (Checkout)

El endpoint `/api/orders/checkout` implementa:

1. **Validación de Tarjeta:**
   - Algoritmo de Luhn para validar número de tarjeta
   - Validación de longitud (13-19 dígitos)

2. **Validación de Stock:**
   - Verifica que haya suficiente stock antes de procesar
   - Si falta stock, retorna error

3. **Simulación de Pago:**
   - 90% de probabilidad de éxito
   - 10% de probabilidad de fallo (para pruebas)

4. **Procesamiento:**
   - Crea la orden en la base de datos
   - Reduce el stock de los productos
   - Limpia el carrito
   - Retorna Transaction ID

### Ejemplo de Request:
```json
{
  "cardNumber": "4532015112830366",
  "cardHolderName": "John Doe",
  "expiryDate": "12/25",
  "cvv": "123",
  "paymentMethod": "CreditCard"
}
```

---

## 🔒 Autorización y Roles

### Atributo Personalizado: `[AuthorizeRoles("Admin")]`
- Protege endpoints específicos por rol
- Se puede usar en controladores o métodos individuales

### Ejemplos:
```csharp
[Authorize] // Requiere autenticación
[AuthorizeRoles("Admin")] // Solo Admin
[AllowAnonymous] // Público (sobrescribe [Authorize])
```

---

## 📝 Notas Importantes

1. **Carrito en Memoria:**
   - El carrito actualmente está en memoria (Dictionary estático)
   - Se pierde al reiniciar el servidor
   - En producción, usar Redis o tabla en BD

2. **JWT Token:**
   - Expira en 60 minutos (configurable en `appsettings.json`)
   - Se incluye en el header: `Authorization: Bearer {token}`

3. **CORS:**
   - Configurado para `http://localhost:5173` (Vite) y `http://localhost:3000` (React)
   - Listo para conectar con el frontend

4. **Seed Data:**
   - Se ejecuta automáticamente solo en Development
   - Crea usuarios, categorías, proveedores y productos de ejemplo
   - Incluye productos con stock bajo para probar alertas

---

## ✅ Checklist de Verificación

- [ ] Ejecuté `dotnet run` sin errores
- [ ] Accedí a Swagger UI
- [ ] Probé login con usuario `admin`
- [ ] Autorizé en Swagger con el token
- [ ] Probé obtener productos (GET /api/products)
- [ ] Probé crear un producto (POST /api/products) como Admin
- [ ] Probé agregar al carrito (POST /api/cart/add)
- [ ] Probé checkout (POST /api/orders/checkout)
- [ ] Verifiqué que el stock se redujo después del checkout

---

## 🎯 Próximo Paso

Una vez verificado que el backend funciona correctamente, procederemos con el **PASO 3: Frontend (React)**.

**¿Listo para continuar?** Confirma cuando hayas probado el backend para proceder con el frontend.
