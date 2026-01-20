import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { authAPI } from '../services/api';
import toast from 'react-hot-toast';

const VerifyEmail = () => {
  const [searchParams] = useSearchParams();
  const token = searchParams.get('token');
  const [status, setStatus] = useState('verifying'); // 'verifying', 'success', 'error'
  const navigate = useNavigate();

  useEffect(() => {
    if (!token) {
      setStatus('error');
      return;
    }

    const verifyEmail = async () => {
      try {
        await authAPI.verifyEmail(token);
        setStatus('success');
        toast.success('Email verificado exitosamente');
        setTimeout(() => {
          navigate('/login');
        }, 3000);
      } catch (error) {
        setStatus('error');
        const errorMessage = error.response?.data?.message || 'Token inválido o expirado';
        toast.error(errorMessage);
      }
    };

    verifyEmail();
  }, [token, navigate]);

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <div className="bg-white p-8 rounded-lg shadow-md w-full max-w-md text-center">
        {status === 'verifying' && (
          <>
            <div className="text-6xl mb-4">⏳</div>
            <h1 className="text-2xl font-bold mb-4">Verificando email...</h1>
            <p className="text-gray-600">Por favor espera un momento</p>
          </>
        )}
        {status === 'success' && (
          <>
            <div className="text-6xl mb-4">✅</div>
            <h1 className="text-2xl font-bold mb-4 text-green-600">¡Email verificado!</h1>
            <p className="text-gray-600 mb-4">
              Tu cuenta ha sido activada exitosamente.
            </p>
            <p className="text-sm text-gray-500">
              Serás redirigido al login en unos segundos...
            </p>
          </>
        )}
        {status === 'error' && (
          <>
            <div className="text-6xl mb-4">❌</div>
            <h1 className="text-2xl font-bold mb-4 text-red-600">Error de verificación</h1>
            <p className="text-gray-600 mb-4">
              El token de verificación es inválido o ha expirado.
            </p>
            <button
              onClick={() => navigate('/login')}
              className="bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 px-4 rounded-md transition"
            >
              Ir al Login
            </button>
          </>
        )}
      </div>
    </div>
  );
};

export default VerifyEmail;
