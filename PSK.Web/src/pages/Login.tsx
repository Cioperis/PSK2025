import { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import axiosInstance from '../api/axiosInstance';
import { Button, Input } from 'reactstrap';
import { Link } from 'react-router-dom';

export const LoginPage = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const auth = useAuth();

    const handleLogin = async () => {
        try {
            const res = await axiosInstance.post('/User/Login', { email, password });
            const token = res.data.token;
            const expires = new Date(res.data.expires).getTime();
            auth.login(token, expires);
        } catch (err) {
            console.error(err);
            alert("Invalid credentials");
        }
    };

    return (
        <div className="p-4" style={{ maxWidth: 400, margin: "auto" }}>
            <div className="text-center mb-4">
                <img src="/ico.png" alt="WellBeing" style={{ height: '60px' }} />
                <h2>WellBeing</h2>
            </div>
            <h3>Login</h3>
            <Input className="mb-2" placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} />
            <Input className="mb-2" placeholder="Password" type="password" value={password} onChange={e => setPassword(e.target.value)} />
            <Button color="primary" onClick={handleLogin} block>Login</Button>
            <p className="mt-3 text-center">
                Don't have an account? <Link to="/register">Register here</Link>
            </p>
        </div>
    );
};
