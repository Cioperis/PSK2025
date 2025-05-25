import { Route, Routes, Navigate } from "react-router-dom";
import { AuthProvider, useAuth } from "./context/AuthContext";
import { LoginPage } from "./pages/Login";
import { RegisterPage } from "./pages/Register";
import AppRoutes from "./AppRoutes";
import Layout from "./Layout";

const ProtectedRoutes = () => {
    const { token } = useAuth();

    return token ? (
        <Layout>
            <Routes>
                {AppRoutes.map(({ path, element }, index) => (
                    <Route key={index} path={path} element={element} />
                ))}
            </Routes>
        </Layout>
    ) : (
        <Navigate to="/login" />
    );
};

function App() {
    return (
        <AuthProvider>
            <Routes>
                <Route path="/login"    element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />
                <Route path="/*"        element={<ProtectedRoutes />} />
            </Routes>
        </AuthProvider>
    );
}

export default App;
