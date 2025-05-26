import React, { createContext, useContext, useState, ReactNode } from 'react';
import { useNavigate } from 'react-router-dom';

interface AuthContextType {
    token: string | null;
    login: (token: string, expiresAt: number) => void;
    logout: () => void;
}

const AuthContext = createContext<AuthContextType>({} as AuthContextType);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [token, setToken] = useState<string | null>(localStorage.getItem("token"));
    const navigate = useNavigate();

    const login = (t: string, expiresAt: number) => {
        setToken(t);
        localStorage.setItem("token", t);
        localStorage.setItem("expires", expiresAt.toString());
        navigate('/');
    };

    const logout = () => {
        setToken(null);
        localStorage.removeItem("token");
        localStorage.removeItem("expires");
        navigate('/login');
    };

    return (
        <AuthContext.Provider value={{ token, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);
