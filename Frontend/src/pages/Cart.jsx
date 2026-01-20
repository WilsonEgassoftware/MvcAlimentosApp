import { useState, useEffect } from 'react';
import { useCart } from '../context/CartContext';
import { productsAPI } from '../services/api';
import { useNavigate } from 'react-router-dom';
import toast from 'react-hot-toast';

const Cart = () => {
  const { cartItems, updateQuantity, removeFromCart, getTotalPrice, clearCart } = useCart();
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    loadProducts();
  }, []);

  const loadProducts = async () => {
    try {
      setLoading(true);
      const response = await productsAPI.getAll();
      setProducts(response.data);
    } catch (error) {
      toast.error('Error al cargar productos');
    } finally {
      setLoading(false);
    }
  };

  const getCartWithProducts = () => {
    return cartItems.map(item => {
      const product = products.find(p => p.id === item.productId);
      return {
        ...item,
        product,
      };
    }).filter(item => item.product);
  };

  const cartWithProducts = getCartWithProducts();
  const total = getTotalPrice(products);

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-xl">Cargando carrito...</div>
      </div>
    );
  }

  return (
    <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <h1 className="text-3xl font-bold mb-6">Carrito de Compras</h1>

      {cartWithProducts.length === 0 ? (
        <div className="text-center py-12">
          <p className="text-gray-500 text-lg mb-4">Tu carrito está vacío</p>
          <button
            onClick={() => navigate('/products')}
            className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-2 rounded"
          >
            Ver Productos
          </button>
        </div>
      ) : (
        <>
          <div className="bg-white rounded-lg shadow-md overflow-hidden">
            {cartWithProducts.map((item) => (
              <div
                key={item.productId}
                className="border-b border-gray-200 p-4 flex items-center justify-between"
              >
                <div className="flex items-center space-x-4 flex-1">
                  <img
                    src={item.product.imageUrl || 'https://via.placeholder.com/300'}
                    alt={item.product.name}
                    className="w-20 h-20 object-cover rounded"
                  />
                  <div className="flex-1">
                    <h3 className="font-semibold">{item.product.name}</h3>
                    <p className="text-gray-600 text-sm">
                      ${item.product.price.toFixed(2)} c/u
                    </p>
                  </div>
                </div>
                <div className="flex items-center space-x-4">
                  <div className="flex items-center space-x-2">
                    <button
                      onClick={() => updateQuantity(item.productId, Math.max(1, item.quantity - 1))}
                      className="bg-gray-200 hover:bg-gray-300 w-8 h-8 rounded"
                    >
                      -
                    </button>
                    <span className="w-12 text-center">{item.quantity}</span>
                    <button
                      onClick={() => updateQuantity(item.productId, item.quantity + 1)}
                      disabled={item.quantity >= item.product.stock}
                      className="bg-gray-200 hover:bg-gray-300 w-8 h-8 rounded disabled:opacity-50"
                    >
                      +
                    </button>
                  </div>
                  <span className="font-semibold w-24 text-right">
                    ${(item.product.price * item.quantity).toFixed(2)}
                  </span>
                  <button
                    onClick={() => removeFromCart(item.productId)}
                    className="text-red-600 hover:text-red-800"
                  >
                    🗑️
                  </button>
                </div>
              </div>
            ))}
          </div>

          <div className="mt-6 bg-gradient-to-r from-blue-50 to-indigo-50 rounded-lg shadow-lg p-6 border-2 border-blue-200">
            <div className="mb-4">
              <h2 className="text-2xl font-bold text-gray-800 mb-2">Resumen de Compra</h2>
              <div className="space-y-2">
                {cartWithProducts.map((item) => (
                  <div key={item.productId} className="flex justify-between text-sm text-gray-600">
                    <span>{item.product.name} x{item.quantity}</span>
                    <span>${(item.product.price * item.quantity).toFixed(2)}</span>
                  </div>
                ))}
              </div>
            </div>
            <div className="border-t-2 border-blue-300 pt-4">
              <div className="flex justify-between items-center">
                <span className="text-2xl font-bold text-gray-800">Total a Pagar:</span>
                <span className="text-3xl font-extrabold text-green-600">
                  ${total.toFixed(2)}
                </span>
              </div>
            </div>
            <div className="flex space-x-4">
              <button
                onClick={() => navigate('/products')}
                className="flex-1 bg-gray-200 hover:bg-gray-300 text-gray-800 py-2 rounded"
              >
                Seguir Comprando
              </button>
              <button
                onClick={() => navigate('/checkout')}
                className="flex-1 bg-blue-600 hover:bg-blue-700 text-white py-2 rounded"
              >
                Proceder al Pago
              </button>
            </div>
          </div>
        </>
      )}
    </div>
  );
};

export default Cart;
