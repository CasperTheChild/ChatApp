import { useState } from 'react'
import Login from './Login';
import Register from './Register';

export default function LoginPage() {
    const [showLogin, setShowLogin] = useState(true);

    const handleLoginButton = () => {
        setShowLogin(true);
    }

    const handleRegisterButton = () => {
        setShowLogin(false);
    }

    return (
        <>
            <h1>Login Page</h1>

            <button onClick={handleLoginButton}>
                Login
            </button>
            <button onClick={handleRegisterButton}>
                Register
            </button>
            {showLogin && <Login />}
            {!showLogin && <Register />}
        </>
    )
}