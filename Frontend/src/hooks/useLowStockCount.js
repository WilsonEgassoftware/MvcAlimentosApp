import { useState, useEffect, useCallback } from 'react';
import { productsAPI } from '../services/api';

export const useLowStockCount = (enabled = true) => {
  const [lowStockCount, setLowStockCount] = useState(0);
  const [loading, setLoading] = useState(false);

  const loadLowStockCount = useCallback(async () => {
    if (!enabled) {
      setLowStockCount(0);
      setLoading(false);
      return;
    }

    try {
      setLoading(true);
      const response = await productsAPI.getLowStock(5);
      setLowStockCount(response.data?.length || 0);
    } catch (error) {
      // Si no está autenticado o no es admin, simplemente no mostrar
      setLowStockCount(0);
    } finally {
      setLoading(false);
    }
  }, [enabled]);

  useEffect(() => {
    if (!enabled) {
      setLowStockCount(0);
      setLoading(false);
      return;
    }

    loadLowStockCount();
    // Actualizar cada 30 segundos solo si está habilitado
    const interval = setInterval(() => {
      loadLowStockCount();
    }, 30000);
    return () => clearInterval(interval);
  }, [enabled, loadLowStockCount]);

  return { lowStockCount, loading, refresh: loadLowStockCount };
};
