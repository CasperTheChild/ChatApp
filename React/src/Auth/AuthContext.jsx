import { createContext, useContext, useState, useEffect } from 'react';
import { LogInApi } from '../Api/AuthApi.js'

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
    const [token, setToken] = useState(null);

    useEffect(() => {
        const storedToken = localStorage.getItem("token");
        if (storedToken) {
            setToken(storedToken);
        }
    }, []);

    async function logIn(email, password) {
        try {
            const data = await LogInApi(email, password);
            setToken(data);
        }
        catch (error) {
            console.error('Error during login:', error);
            throw new Error('Incorrect email or password!');
        }
    }

    function logOut() {
        localStorage.removeItem('token');
        setToken(null);
    }

    return (
        <AuthContext.Provider value={{ token, logIn, logOut }}>
            {children}
        </AuthContext.Provider>
    )
}

export const useAuth = () => useContext(AuthContext);