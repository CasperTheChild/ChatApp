import { UseAuth } from "../../contexts/AuthContext.jsx";
import { useState } from "react";

const Register = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const { register } = UseAuth();

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            register(username, password, confirmPassword);
        }
        catch (error) {
            console.error("Error during register in register component", error);
        }
    }

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
            <input type="password"
                name="confirmPassword"
                id="confirmPassword"
                required
                onChange={(e) => setConfirmPassword(e.target.value)} />
            <button type="submit">Register</button>
        </form>
    )
}

export default Register