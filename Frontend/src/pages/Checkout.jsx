import { useState, useEffect } from 'react';
import { useCart } from '../context/CartContext';
import { ordersAPI, productsAPI } from '../services/api';
import { useNavigate } from 'react-router-dom';
import toast from 'react-hot-toast';

// Función removida: Ya no usamos validación Luhn, aceptamos números ficticios

const Checkout = () => {
  const { cartItems, getTotalPrice, clearCart } = useCart();
  const [products, setProducts] = useState([]);
  const [formData, setFormData] = useState({
    cardNumber: '',
    cardHolder: '',
    expiryDate: '',
    cvv: '',
    shippingAddress: '',
  });
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [checkoutSuccess, setCheckoutSuccess] = useState(false);
  const [orderId, setOrderId] = useState(null);
  const [sendingInvoice, setSendingInvoice] = useState(false);
  const navigate = useNavigate();

  // Cargar productos
  useEffect(() => {
    const loadProducts = async () => {
      try {
        const response = await productsAPI.getAll();
        setProducts(response.data);
      } catch (error) {
        console.error('Error loading products:', error);
      }
    };
    loadProducts();
  }, []);

  const getCartWithProducts = () => {
    return cartItems.map(item => {
      const product = products.find(p => p.id === item.productId);
      return {
        ...item,
        product,
      };
    }).filter(item => item.product);
  };

  const validateForm = () => {
    const newErrors = {};

    // Validar número de tarjeta (formato básico, acepta números ficticios)
    const cleanedCardNumber = formData.cardNumber.replace(/\D/g, '');
    if (!formData.cardNumber || cleanedCardNumber.trim().length === 0) {
      newErrors.cardNumber = 'El número de tarjeta es requerido';
    } else if (cleanedCardNumber.length < 13 || cleanedCardNumber.length > 19) {
      newErrors.cardNumber = `Número de tarjeta inválido (debe tener entre 13 y 19 dígitos, tienes ${cleanedCardNumber.length})`;
    }

    // Validar nombre
    if (!formData.cardHolder || formData.cardHolder.length < 3) {
      newErrors.cardHolder = 'Nombre inválido';
    }

    // Validar fecha de expiración (MM/YY)
    const expiryRegex = /^(0[1-9]|1[0-2])\/\d{2}$/;
    if (!formData.expiryDate || !expiryRegex.test(formData.expiryDate)) {
      newErrors.expiryDate = 'Fecha inválida (MM/YY)';
    }

    // Validar CVV (3 o 4 dígitos para aceptar más formatos)
    const cleanedCVV = formData.cvv.replace(/\D/g, '');
    if (!formData.cvv || cleanedCVV.length < 3 || cleanedCVV.length > 4) {
      newErrors.cvv = 'CVV inválido (debe tener 3 o 4 dígitos)';
    }

    // Validar dirección
    if (!formData.shippingAddress || formData.shippingAddress.length < 10) {
      newErrors.shippingAddress = 'Dirección inválida';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateForm()) {
      toast.error('Por favor, corrige los errores en el formulario');
      return;
    }

    setLoading(true);

    try {
      // Simular procesamiento con spinner de 2 segundos
      await new Promise(resolve => setTimeout(resolve, 2000));

      // Asegurar que siempre haya un número de tarjeta válido (ficticio)
      let cleanedCardNumber = formData.cardNumber.replace(/\D/g, '');
      if (!cleanedCardNumber || cleanedCardNumber.length === 0) {
        cleanedCardNumber = '1234567890123456'; // Número ficticio por defecto
      }

      const checkoutData = {
        cardNumber: cleanedCardNumber,
        cardHolderName: formData.cardHolder || 'TARJETA FICTICIA',
        expiryDate: formData.expiryDate || '12/99',
        cvv: formData.cvv || '123',
        paymentMethod: 'CreditCard',
      };

      console.log('Enviando checkout data:', checkoutData);

      const response = await ordersAPI.checkout(checkoutData);
      
      if (response.data.success) {
        setOrderId(response.data.orderId);
        setCheckoutSuccess(true);
        clearCart();
        toast.success('¡Pago procesado exitosamente!');
      } else {
        toast.error(response.data.message || 'Error al procesar el pago');
      }
    } catch (error) {
      console.error('Error completo del checkout:', error);
      console.error('Response data:', error.response?.data);
      const message = error.response?.data?.message || error.response?.data?.Message || error.message || 'Error al procesar el pago';
      toast.error(message);
    } finally {
      setLoading(false);
    }
  };

  const formatCardNumber = (value) => {
    const cleaned = value.replace(/\D/g, '');
    const formatted = cleaned.match(/.{1,4}/g)?.join(' ') || cleaned;
    return formatted.substring(0, 19);
  };

  const formatExpiryDate = (value) => {
    const cleaned = value.replace(/\D/g, '');
    if (cleaned.length >= 2) {
      return cleaned.substring(0, 2) + '/' + cleaned.substring(2, 4);
    }
    return cleaned;
  };

  const cartWithProducts = getCartWithProducts();
  const total = getTotalPrice(products);

  if (checkoutSuccess) {
    return (
      <div className="max-w-2xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="bg-white rounded-lg shadow-lg p-8 text-center">
          <div className="text-6xl mb-4">✅</div>
          <h1 className="text-3xl font-bold text-green-600 mb-4">¡Compra Exitosa!</h1>
          <p className="text-gray-600 mb-6">
            Tu orden ha sido procesada correctamente. ID de orden: <strong>#{orderId}</strong>
          </p>
          
          {orderId && (
            <button
              onClick={async () => {
                setSendingInvoice(true);
                try {
                  await ordersAPI.sendInvoice(orderId);
                  toast.success('Factura enviada a tu correo electrónico');
                } catch (error) {
                  toast.error('Error al enviar la factura');
                } finally {
                  setSendingInvoice(false);
                }
              }}
              disabled={sendingInvoice}
              className="bg-blue-600 hover:bg-blue-700 text-white font-semibold py-3 px-6 rounded-md transition disabled:opacity-50 mb-4"
            >
              {sendingInvoice ? 'Enviando...' : '📧 Recibir Factura en Correo'}
            </button>
          )}
          
          <div className="flex space-x-4 justify-center mt-6">
            <button
              onClick={() => navigate('/products')}
              className="bg-gray-200 hover:bg-gray-300 text-gray-800 font-semibold py-2 px-6 rounded-md transition"
            >
              Seguir Comprando
            </button>
            <button
              onClick={() => navigate('/products')}
              className="bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 px-6 rounded-md transition"
            >
              Volver a Productos
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="max-w-2xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <h1 className="text-3xl font-bold mb-6">Checkout</h1>

      <div className="bg-white rounded-lg shadow-md p-6 mb-6">
        <h2 className="text-xl font-semibold mb-4">Resumen del Pedido</h2>
        {cartWithProducts.length > 0 && (
          <div className="space-y-2 mb-4">
            {cartWithProducts.map((item) => (
              <div key={item.productId} className="flex justify-between text-sm">
                <span>{item.product.name} x{item.quantity}</span>
                <span>${(item.product.price * item.quantity).toFixed(2)}</span>
              </div>
            ))}
          </div>
        )}
        <div className="flex justify-between items-center border-t-2 border-gray-200 pt-4">
          <span className="text-xl font-semibold">Total a Pagar:</span>
          <span className="text-3xl font-bold text-green-600">
            ${total.toFixed(2)}
          </span>
        </div>
      </div>

      <form onSubmit={handleSubmit} className="bg-white rounded-lg shadow-md p-6 space-y-4">
        <h2 className="text-xl font-semibold mb-4">Información de Pago</h2>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Número de Tarjeta
          </label>
          <input
            type="text"
            value={formData.cardNumber}
            onChange={(e) =>
              setFormData({ ...formData, cardNumber: formatCardNumber(e.target.value) })
            }
            placeholder="1234 5678 9012 3456"
            maxLength={19}
            className={`w-full px-4 py-2 border rounded-md ${
              errors.cardNumber ? 'border-red-500' : 'border-gray-300'
            }`}
          />
          {errors.cardNumber && (
            <p className="text-red-500 text-sm mt-1">{errors.cardNumber}</p>
          )}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Nombre del Titular
          </label>
          <input
            type="text"
            value={formData.cardHolder}
            onChange={(e) =>
              setFormData({ ...formData, cardHolder: e.target.value.toUpperCase() })
            }
            placeholder="JUAN PEREZ"
            className={`w-full px-4 py-2 border rounded-md ${
              errors.cardHolder ? 'border-red-500' : 'border-gray-300'
            }`}
          />
          {errors.cardHolder && (
            <p className="text-red-500 text-sm mt-1">{errors.cardHolder}</p>
          )}
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Fecha de Expiración (MM/YY)
            </label>
            <input
              type="text"
              value={formData.expiryDate}
              onChange={(e) =>
                setFormData({ ...formData, expiryDate: formatExpiryDate(e.target.value) })
              }
              placeholder="12/25"
              maxLength={5}
              className={`w-full px-4 py-2 border rounded-md ${
                errors.expiryDate ? 'border-red-500' : 'border-gray-300'
              }`}
            />
            {errors.expiryDate && (
              <p className="text-red-500 text-sm mt-1">{errors.expiryDate}</p>
            )}
          </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  CVV
                </label>
                <input
                  type="text"
                  value={formData.cvv}
                  onChange={(e) =>
                    setFormData({ ...formData, cvv: e.target.value.replace(/\D/g, '').substring(0, 4) })
                  }
                  placeholder="123"
                  maxLength={4}
                  className={`w-full px-4 py-2 border rounded-md ${
                    errors.cvv ? 'border-red-500' : 'border-gray-300'
                  }`}
                />
                {errors.cvv && (
                  <p className="text-red-500 text-sm mt-1">{errors.cvv}</p>
                )}
              </div>
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Dirección de Envío
          </label>
          <textarea
            value={formData.shippingAddress}
            onChange={(e) =>
              setFormData({ ...formData, shippingAddress: e.target.value })
            }
            placeholder="Calle, Número, Ciudad, País"
            rows={3}
            className={`w-full px-4 py-2 border rounded-md ${
              errors.shippingAddress ? 'border-red-500' : 'border-gray-300'
            }`}
          />
          {errors.shippingAddress && (
            <p className="text-red-500 text-sm mt-1">{errors.shippingAddress}</p>
          )}
        </div>

        <button
          type="submit"
          disabled={loading}
          className="w-full bg-green-600 hover:bg-green-700 text-white font-semibold py-3 rounded-md transition disabled:opacity-50 flex items-center justify-center"
        >
          {loading ? (
            <>
              <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              Procesando pago...
            </>
          ) : (
            'Procesar Pago'
          )}
        </button>
      </form>
    </div>
  );
};

export default Checkout;
