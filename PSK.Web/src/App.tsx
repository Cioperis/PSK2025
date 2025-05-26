import React, { useEffect } from 'react';
import { Route, Routes, Navigate, useLocation } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import { LoginPage } from './pages/Login';
import { RegisterPage } from './pages/Register';
import AppRoutes from './AppRoutes';
import Layout from './Layout';

const ProtectedRoutes = () => {
    const { token, logout } = useAuth();
    const location = useLocation();

    useEffect(() => {
        const storedExpiry = localStorage.getItem('expires');
        if (!storedExpiry || Date.now() >= parseInt(storedExpiry, 10)) {
            logout();
        }
    }, [location.pathname, logout]);

    if (!token) {
        return <Navigate to="/login" replace />;
    }

    return (
        <Layout>
            <Routes>
                {AppRoutes.map(({ path, element }, index) => (
                    <Route key={index} path={path} element={element} />
                ))}
            </Routes>
        </Layout>
    );
};

function App() {
    return (
        <AuthProvider>
            <Routes>
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />
                <Route path="/*" element={<ProtectedRoutes />} />
            </Routes>
        </AuthProvider>
    );
}

export default App;