import { createContext, useContext, useState, useEffect } from 'react';
import { cartAPI } from '../services/api';
import toast from 'react-hot-toast';

const CartContext = createContext();

export const useCart = () => {
  const context = useContext(CartContext);
  if (!context) {
    throw new Error('useCart must be used within a CartProvider');
  }
  return context;
};

export const CartProvider = ({ children }) => {
  const [cartItems, setCartItems] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    // Solo cargar el carrito si hay token
    const token = localStorage.getItem('token');
    if (token) {
      loadCart();
    } else {
      // Si no hay token, usar localStorage como fallback
      const savedCart = localStorage.getItem('cart');
      if (savedCart) {
        try {
          const parsed = JSON.parse(savedCart);
          setCartItems(Array.isArray(parsed) ? parsed : []);
        } catch (e) {
          console.error('Error loading cart from localStorage:', e);
          setCartItems([]);
        }
      } else {
        setCartItems([]);
      }
      setLoading(false);
    }
  }, []);

  const loadCart = async () => {
    try {
      setLoading(true);
      const response = await cartAPI.get();
      const data = response.data;
      
      // El carrito devuelve CartSummaryDTO con Items
      if (data && data.items && Array.isArray(data.items)) {
        // Convertir CartItemDTO a formato simple para el contexto
        const formattedItems = data.items.map(item => ({
          productId: item.productId,
          quantity: item.quantity
        }));
        setCartItems(formattedItems);
        localStorage.setItem('cart', JSON.stringify(formattedItems));
      } else if (Array.isArray(data)) {
        // Fallback: si viene como array directamente
        setCartItems(data);
        localStorage.setItem('cart', JSON.stringify(data));
      } else {
        setCartItems([]);
        localStorage.setItem('cart', JSON.stringify([]));
      }
    } catch (error) {
      console.error('Error loading cart:', error);
      // Si no está autenticado, usar localStorage como fallback
      const savedCart = localStorage.getItem('cart');
      if (savedCart) {
        try {
          const parsed = JSON.parse(savedCart);
          setCartItems(Array.isArray(parsed) ? parsed : []);
        } catch (e) {
          console.error('Error loading cart from localStorage:', e);
          setCartItems([]);
        }
      } else {
        setCartItems([]);
      }
    } finally {
      setLoading(false);
    }
  };

  const addToCart = async (productId, quantity = 1) => {
    try {
      // Llamar al endpoint para agregar al carrito
      await cartAPI.add(productId, quantity);
      
      // Recargar el carrito completo para obtener el estado actualizado
      const cartResponse = await cartAPI.get();
      const cartData = cartResponse.data;
      
      // El carrito devuelve CartSummaryDTO con Items
      if (cartData && cartData.items && Array.isArray(cartData.items)) {
        // Convertir CartItemDTO a formato simple para el contexto
        const formattedItems = cartData.items.map(item => ({
          productId: item.productId,
          quantity: item.quantity
        }));
        setCartItems(formattedItems);
        localStorage.setItem('cart', JSON.stringify(formattedItems));
        toast.success('Producto agregado al carrito');
      } else {
        // Si no hay estructura esperada, usar array vacío
        setCartItems([]);
        localStorage.setItem('cart', JSON.stringify([]));
        toast.success('Producto agregado al carrito');
      }
    } catch (error) {
      console.error('Error adding to cart:', error);
      // Fallback a localStorage si no está autenticado o hay error
      const currentItems = Array.isArray(cartItems) ? cartItems : [];
      // Verificar si el producto ya existe en el carrito
      const existingItemIndex = currentItems.findIndex(item => item.productId === productId);
      
      let updatedCart;
      if (existingItemIndex >= 0) {
        // Si existe, sumar la cantidad
        updatedCart = currentItems.map((item, index) => 
          index === existingItemIndex 
            ? { ...item, quantity: (item.quantity || 0) + quantity }
            : item
        );
      } else {
        // Si no existe, agregar nuevo
        updatedCart = [...currentItems, { productId, quantity }];
      }
      
      setCartItems(updatedCart);
      localStorage.setItem('cart', JSON.stringify(updatedCart));
      
      // Mostrar mensaje según el error
      if (error.response?.status === 401) {
        toast.success('Producto agregado al carrito (modo local - inicia sesión para sincronizar)');
      } else {
        const errorMessage = error.response?.data?.message || 'Error al agregar al carrito';
        toast.error(errorMessage);
      }
    }
  };

  const updateQuantity = async (productId, quantity) => {
    try {
      // Actualizar cantidad en el backend
      await cartAPI.update(productId, quantity);
      
      // Recargar el carrito completo
      const cartResponse = await cartAPI.get();
      const cartData = cartResponse.data;
      
      if (cartData && cartData.items && Array.isArray(cartData.items)) {
        const formattedItems = cartData.items.map(item => ({
          productId: item.productId,
          quantity: item.quantity
        }));
        setCartItems(formattedItems);
        localStorage.setItem('cart', JSON.stringify(formattedItems));
      } else if (Array.isArray(cartData)) {
        setCartItems(cartData);
        localStorage.setItem('cart', JSON.stringify(cartData));
      }
    } catch (error) {
      console.error('Error updating quantity:', error);
      // Fallback a localStorage
      const currentItems = Array.isArray(cartItems) ? cartItems : [];
      const updatedCart = currentItems.map(item =>
        item.productId === productId ? { ...item, quantity } : item
      );
      setCartItems(updatedCart);
      localStorage.setItem('cart', JSON.stringify(updatedCart));
    }
  };

  const removeFromCart = async (productId) => {
    try {
      // Eliminar del backend
      await cartAPI.remove(productId);
      
      // Recargar el carrito completo
      const cartResponse = await cartAPI.get();
      const cartData = cartResponse.data;
      
      if (cartData && cartData.items && Array.isArray(cartData.items)) {
        const formattedItems = cartData.items.map(item => ({
          productId: item.productId,
          quantity: item.quantity
        }));
        setCartItems(formattedItems);
        localStorage.setItem('cart', JSON.stringify(formattedItems));
      } else if (Array.isArray(cartData)) {
        setCartItems(cartData);
        localStorage.setItem('cart', JSON.stringify(cartData));
      } else {
        setCartItems([]);
        localStorage.setItem('cart', JSON.stringify([]));
      }
      
      toast.success('Producto eliminado del carrito');
    } catch (error) {
      console.error('Error removing from cart:', error);
      // Fallback a localStorage
      const currentItems = Array.isArray(cartItems) ? cartItems : [];
      const updatedCart = currentItems.filter(item => item.productId !== productId);
      setCartItems(updatedCart);
      localStorage.setItem('cart', JSON.stringify(updatedCart));
      toast.success('Producto eliminado del carrito');
    }
  };

  const clearCart = async () => {
    try {
      await cartAPI.clear();
      setCartItems([]);
      localStorage.removeItem('cart');
    } catch (error) {
      setCartItems([]);
      localStorage.removeItem('cart');
    }
  };

  const getTotalItems = () => {
    if (!Array.isArray(cartItems)) {
      return 0;
    }
    return cartItems.reduce((total, item) => total + (item.quantity || 0), 0);
  };

  const getTotalPrice = (products) => {
    if (!Array.isArray(cartItems) || !Array.isArray(products)) {
      return 0;
    }
    return cartItems.reduce((total, item) => {
      const product = products.find(p => p.id === item.productId);
      if (product) {
        return total + (product.price * (item.quantity || 0));
      }
      return total;
    }, 0);
  };

  const getCartTotal = () => {
    // Esta función calcula el total sin necesidad de pasar productos
    // Se puede usar cuando ya se tienen los productos cargados
    return getTotalPrice;
  };

  return (
    <CartContext.Provider
      value={{
        cartItems,
        addToCart,
        updateQuantity,
        removeFromCart,
        clearCart,
        getTotalItems,
        getTotalPrice,
        loadCart,
        loading,
      }}
    >
      {children}
    </CartContext.Provider>
  );
};
