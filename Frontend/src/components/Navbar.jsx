import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { useCart } from '../context/CartContext';
import { useLowStockCount } from '../hooks/useLowStockCount';

const Navbar = () => {
  const { user, logout, isAdmin, loading: authLoading } = useAuth();
  const { getTotalItems } = useCart();
  const isAdminUser = user && isAdmin();
  const { lowStockCount } = useLowStockCount(isAdminUser);
  const navigate = useNavigate();

  // Evitar errores mientras carga
  if (authLoading) {
    return (
      <nav className="bg-blue-600 text-white shadow-lg">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center h-16">
            <div className="text-xl font-bold">🛒 Supermarket</div>
            <div>Cargando...</div>
          </div>
        </div>
      </nav>
    );
  }

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="bg-blue-600 text-white shadow-lg">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          <div className="flex items-center space-x-8">
            <Link to="/" className="text-xl font-bold">
              🛒 Supermarket
            </Link>
            
            {user && (
              <>
                <Link to="/products" className="hover:text-blue-200 transition">
                  Productos
                </Link>
                {isAdmin() && (
                  <>
                    <Link to="/dashboard" className="hover:text-blue-200 transition">
                      Dashboard
                    </Link>
                    <Link to="/admin/products" className="hover:text-blue-200 transition">
                      Gestión de Productos
                    </Link>
                    <Link to="/admin/suppliers" className="hover:text-blue-200 transition">
                      Gestión de Proveedores
                    </Link>
                  </>
                )}
              </>
            )}
          </div>

          <div className="flex items-center space-x-4">
            {user ? (
              <>
                {isAdminUser && lowStockCount > 0 && (
                  <Link
                    to="/admin/products"
                    className="relative hover:text-blue-200 transition mr-2"
                    title="Productos con stock bajo"
                  >
                    ⚠️ Stock Bajo
                    <span className="absolute -top-2 -right-2 bg-red-500 text-white text-xs rounded-full w-5 h-5 flex items-center justify-center font-bold">
                      {lowStockCount}
                    </span>
                  </Link>
                )}
                <Link
                  to="/cart"
                  className="relative hover:text-blue-200 transition"
                >
                  🛒 Carrito
                  {getTotalItems() > 0 && (
                    <span className="absolute -top-2 -right-2 bg-red-500 text-white text-xs rounded-full w-5 h-5 flex items-center justify-center">
                      {getTotalItems()}
                    </span>
                  )}
                </Link>
                <span className="text-sm">
                  {user.username} ({user.role})
                </span>
                <button
                  onClick={handleLogout}
                  className="bg-red-500 hover:bg-red-600 px-4 py-2 rounded transition"
                >
                  Salir
                </button>
              </>
            ) : (
              <Link
                to="/login"
                className="bg-white text-blue-600 hover:bg-blue-50 px-4 py-2 rounded transition"
              >
                Iniciar Sesión
              </Link>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
