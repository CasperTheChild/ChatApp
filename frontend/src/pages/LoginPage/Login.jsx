import { useState } from "react"
import { UseAuth } from "../../contexts/AuthContext.jsx"

const Login = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const { login } = UseAuth();

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await login(username, password);

            console.log("Login successful!");
        }
        catch (error) {
            console.error("Error during login in login component", error);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <input type="text"
                name="username"
                id="username"
                required
                onChange={(e) => setUsername(e.target.value)} />
            <input type="password"
                name="password"
                id="password"
                required
                onChange={(e) => setPassword(e.target.value)} />
            <button type="submit">Login</button>
        </form>
    )
}

export default Login