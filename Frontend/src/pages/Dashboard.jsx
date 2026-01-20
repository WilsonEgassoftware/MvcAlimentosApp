import { useState, useEffect } from 'react';
import { dashboardAPI, productsAPI } from '../services/api';
import toast from 'react-hot-toast';

const Dashboard = () => {
  const [dashboard, setDashboard] = useState(null);
  const [lowStockProducts, setLowStockProducts] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadDashboard();
  }, []);

  const loadDashboard = async () => {
    try {
      setLoading(true);
      const [dashboardRes, lowStockRes] = await Promise.all([
        dashboardAPI.get(),
        productsAPI.getLowStock(5),
      ]);
      setDashboard(dashboardRes.data);
      setLowStockProducts(lowStockRes.data);
    } catch (error) {
      toast.error('Error al cargar el dashboard');
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-xl">Cargando dashboard...</div>
      </div>
    );
  }

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <h1 className="text-3xl font-bold mb-6">Dashboard Administrativo</h1>

      {/* Estadísticas */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-6 mb-8">
        <div className="bg-blue-500 text-white p-6 rounded-lg shadow">
          <h3 className="text-lg font-semibold mb-2">Total Productos</h3>
          <p className="text-3xl font-bold">{dashboard?.totalProducts || 0}</p>
        </div>
        <div className="bg-green-500 text-white p-6 rounded-lg shadow">
          <h3 className="text-lg font-semibold mb-2">Total Categorías</h3>
          <p className="text-3xl font-bold">{dashboard?.totalCategories || 0}</p>
        </div>
        <div className={`${lowStockProducts.length > 0 ? 'bg-red-500' : 'bg-yellow-500'} text-white p-6 rounded-lg shadow`}>
          <h3 className="text-lg font-semibold mb-2">Productos Stock Bajo</h3>
          <p className="text-3xl font-bold">{lowStockProducts.length}</p>
        </div>
        <div className="bg-purple-500 text-white p-6 rounded-lg shadow">
          <h3 className="text-lg font-semibold mb-2">Total Órdenes</h3>
          <p className="text-3xl font-bold">{dashboard?.totalOrders || 0}</p>
        </div>
      </div>

      {/* Alertas de Stock Bajo */}
      {lowStockProducts.length > 0 && (
        <div className="bg-red-50 border-l-4 border-red-500 p-6 mb-6 rounded-lg shadow-md">
          <div className="flex justify-between items-center mb-4">
            <h2 className="text-2xl font-bold text-red-800">
              ⚠️ Alertas de Stock Bajo ({lowStockProducts.length})
            </h2>
            <a
              href="/admin/products"
              className="text-blue-600 hover:text-blue-800 underline text-sm"
            >
              Ver todos los productos →
            </a>
          </div>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {lowStockProducts.map((product) => (
              <div
                key={product.id}
                className="bg-white p-4 rounded-lg shadow border-2 border-red-300 hover:border-red-500 transition"
              >
                <div className="flex items-start justify-between">
                  <div className="flex-1">
                    <h3 className="font-semibold text-red-800 mb-1">{product.name}</h3>
                    <p className="text-sm text-gray-600 mb-2">
                      {product.categoryName || 'Sin categoría'}
                    </p>
                    <div className="flex items-center space-x-2">
                      <span className="px-2 py-1 bg-red-100 text-red-800 text-xs font-bold rounded">
                        Stock: {product.stock} unidades
                      </span>
                      {product.stock === 0 && (
                        <span className="px-2 py-1 bg-red-600 text-white text-xs font-bold rounded">
                          AGOTADO
                        </span>
                      )}
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* Resumen de Ventas */}
      <div className="bg-white p-6 rounded-lg shadow">
        <h2 className="text-xl font-bold mb-4">Resumen de Ventas</h2>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div>
            <p className="text-gray-600">Total Vendido (Hoy)</p>
            <p className="text-2xl font-bold text-green-600">
              ${(dashboard?.todaySales || 0).toFixed(2)}
            </p>
          </div>
          <div>
            <p className="text-gray-600">Total Vendido (Este Mes)</p>
            <p className="text-2xl font-bold text-blue-600">
              ${(dashboard?.monthSales || 0).toFixed(2)}
            </p>
          </div>
          <div>
            <p className="text-gray-600">Promedio por Orden</p>
            <p className="text-2xl font-bold text-purple-600">
              ${(dashboard?.averageOrderValue || 0).toFixed(2)}
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
