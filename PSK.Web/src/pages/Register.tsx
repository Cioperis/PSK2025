import { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import axiosInstance from '../api/axiosInstance';
import { Button, Input } from 'reactstrap';
import { Link } from 'react-router-dom';

export const RegisterPage = () => {
    const [firstName, setFirst] = useState('');
    const [lastName, setLast] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isActive] = useState(true);
    const auth = useAuth();

    const handleRegister = async () => {
        try {
            const res = await axiosInstance.post('/User/CreateUser', {
                firstName,
                lastName,
                email,
                password,
                isActive,
                role: 0
            });

            const loginRes = await axiosInstance.post('/User/Login', { email, password });
            const token = loginRes.data.token;
            const expires = new Date(loginRes.data.expires).getTime();
            auth.login(token, expires);
        } catch (err) {
            console.error(err);
            alert("Registration failed");
        }
    };

    return (
        <div className="p-4" style={{ maxWidth: 400, margin: "auto" }}>
            <div className="text-center mb-4">
                <img src="/ico.png" alt="WellBeing" style={{ height: '60px' }} />
                <h2>WellBeing</h2>
            </div>
            <h3>Register</h3>
            <Input className="mb-2" placeholder="Username" onChange={e => (setFirst(e.target.value), setLast(e.target.value))} />
            <Input className="mb-2" placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} />
            <Input className="mb-2" placeholder="Password" type="password" value={password} onChange={e => setPassword(e.target.value)} />
            <Button color="success" onClick={handleRegister} block>Register</Button>
            <p className="mt-3 text-center">
                Already have an account? <Link to="/login">Login here</Link>
            </p>
        </div>
    );
};
