import { useState, useEffect } from 'react';
import { useCart } from '../context/CartContext';
import { productsAPI } from '../services/api';
import { useNavigate } from 'react-router-dom';
import toast from 'react-hot-toast';

const CartDrawer = ({ isOpen, onClose }) => {
  const { cartItems, updateQuantity, removeFromCart, getTotalPrice, clearCart } = useCart();
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    if (isOpen) {
      loadProducts();
    }
  }, [isOpen]);

  const loadProducts = async () => {
    try {
      setLoading(true);
      const response = await productsAPI.getAll();
      setProducts(response.data || []);
    } catch (error) {
      console.error('Error loading products:', error);
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

  if (!isOpen) return null;

  return (
    <>
      {/* Overlay */}
      <div 
        className="fixed inset-0 bg-black bg-opacity-50 z-40"
        onClick={onClose}
      />
      
      {/* Drawer */}
      <div className="fixed right-0 top-0 h-full w-full max-w-md bg-white shadow-xl z-50 flex flex-col">
        {/* Header */}
        <div className="flex items-center justify-between p-4 border-b border-gray-200">
          <h2 className="text-xl font-bold">Carrito de Compras</h2>
          <button
            onClick={onClose}
            className="text-gray-500 hover:text-gray-700 text-2xl"
          >
            ×
          </button>
        </div>

        {/* Cart Items */}
        <div className="flex-1 overflow-y-auto p-4">
          {loading ? (
            <div className="flex items-center justify-center h-64">
              <div className="text-gray-500">Cargando...</div>
            </div>
          ) : cartWithProducts.length === 0 ? (
            <div className="flex flex-col items-center justify-center h-64 text-center">
              <div className="text-6xl mb-4">🛒</div>
              <p className="text-gray-500 text-lg mb-4">Tu carrito está vacío</p>
              <button
                onClick={() => {
                  onClose();
                  navigate('/products');
                }}
                className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-2 rounded"
              >
                Ver Productos
              </button>
            </div>
          ) : (
            <div className="space-y-4">
              {cartWithProducts.map((item) => (
                <div
                  key={item.productId}
                  className="flex items-center space-x-4 p-4 border border-gray-200 rounded-lg"
                >
                  {/* Product Image */}
                  <img
                    src={item.product.imageUrl || 'https://via.placeholder.com/300'}
                    alt={item.product.name}
                    className="w-16 h-16 object-cover rounded"
                  />
                  
                  {/* Product Info */}
                  <div className="flex-1 min-w-0">
                    <h3 className="font-semibold text-sm truncate">{item.product.name}</h3>
                    <p className="text-gray-600 text-xs">${item.product.price.toFixed(2)} c/u</p>
                  </div>

                  {/* Quantity Controls */}
                  <div className="flex items-center space-x-2">
                    <button
                      onClick={() => updateQuantity(item.productId, Math.max(1, item.quantity - 1))}
                      className="bg-gray-200 hover:bg-gray-300 w-7 h-7 rounded text-sm"
                    >
                      -
                    </button>
                    <span className="w-8 text-center text-sm">{item.quantity}</span>
                    <button
                      onClick={() => updateQuantity(item.productId, item.quantity + 1)}
                      disabled={item.quantity >= item.product.stock}
                      className="bg-gray-200 hover:bg-gray-300 w-7 h-7 rounded text-sm disabled:opacity-50"
                      title={item.quantity >= item.product.stock ? 'Stock insuficiente' : ''}
                    >
                      +
                    </button>
                  </div>

                  {/* Subtotal */}
                  <div className="text-right min-w-[80px]">
                    <p className="font-semibold text-sm">
                      ${(item.product.price * item.quantity).toFixed(2)}
                    </p>
                  </div>

                  {/* Remove Button */}
                  <button
                    onClick={() => removeFromCart(item.productId)}
                    className="text-red-600 hover:text-red-800 p-2"
                    title="Eliminar del carrito"
                  >
                    🗑️
                  </button>
                </div>
              ))}
            </div>
          )}
        </div>

        {/* Footer with Total and Checkout Button */}
        {cartWithProducts.length > 0 && (
          <div className="border-t border-gray-200 p-4 bg-gray-50">
            <div className="flex justify-between items-center mb-4">
              <span className="text-lg font-semibold">Total:</span>
              <span className="text-2xl font-bold text-green-600">
                ${total.toFixed(2)}
              </span>
            </div>
            <button
              onClick={() => {
                onClose();
                navigate('/checkout');
              }}
              className="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-3 rounded-md transition"
            >
              Proceder al Pago
            </button>
            <button
              onClick={() => {
                if (window.confirm('¿Estás seguro de que deseas vaciar el carrito?')) {
                  clearCart();
                  toast.success('Carrito vaciado');
                }
              }}
              className="w-full mt-2 bg-gray-200 hover:bg-gray-300 text-gray-800 py-2 rounded-md transition"
            >
              Vaciar Carrito
            </button>
          </div>
        )}
      </div>
    </>
  );
};

export default CartDrawer;
