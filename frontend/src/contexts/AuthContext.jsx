import { createContext, useContext, useState } from 'react'
import { loginApi, registerApi } from '../api/authApi';

const AuthContext = createContext();

export const UseAuth = () => useContext(AuthContext);

export const AuthProvider = ( { children }) => {
    const [token, setToken] = useState(localStorage.getItem("token"));

    // useEffect(() => {
    //     const storedToken = localStorage.getItem("token");
    //     if (storedToken) {
    //         setToken(storedToken);
    //         setLoading(true);
    //     }
    // }, [])

    const login = async (username, password) => {
        try {
            const data = await loginApi(username, password);
            setToken(data);
        }
        catch (error) {
            console.error('Error during login:', error);
            throw new Error('Incorrect username or password!');
        }
    }

    const logout = () => {
        setToken(null);
    }

    const register = (username, password, confirmPassword) => {
        try {
            registerApi(username, password, confirmPassword);
        }
        catch (error) {
            console.error('Error during register:', error);
            throw new Error('Incorrect username or password!');
        }
    }

    return (
        <AuthContext.Provider value={{ token, login, logout, register }}>
            {children}
        </AuthContext.Provider>
    )
}