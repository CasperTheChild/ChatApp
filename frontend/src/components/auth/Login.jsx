import { useState } from "react"
import { UseAuth } from "../../contexts/AuthContext.jsx"

const Login = () => {
    const [validated, setValidated] = useState(false);
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const { login } = UseAuth();

    const handleSubmit = async (e) => {
        e.preventDefault();

        const form = e.currentTarget;
        setValidated(true);

        if (!form.checkValidity()) {
            return;
        }
        
        try {
            await login(username, password);

            console.log("Login successful!");
        }
        catch (error) {
            console.error("Error during login in login component", error);
        }
    };

    return (
        <form
            className={validated ? 'was-validated' : ''}
            noValidate
            onSubmit={handleSubmit}>
            <div class="form-field">
                <div className="mb-3">
                    <label for="fieldText" class="form-label">Username</label>
                    <input class="form-control"
                        type="text"
                        name="username"
                        id="username"
                        placeholder="username"
                        required
                        onChange={(e) => setUsername(e.target.value)} />
                    
                    <div class="invalid-feedback">
                        Username is required
                    </div>
                    
                    <div class="form-text">
                        Please, type your username.
                    </div>
                </div>

                <div className="mb-3">
                    <label for="fieldText" class="form-label">Password</label>
                    <input class="form-control"
                        type="password"
                        name="password"
                        id="password"
                        required
                        onChange={(e) => setPassword(e.target.value)} />
                    
                    <div class="invalid-feedback">
                        Password is required
                    </div>

                    <div class="form-text">
                        We’ll never share your password with anyone else.
                    </div>
                </div>

                <button className="btn border btn-subtle-primary"
                    type="submit">
                    Login
                </button>
            </div>
        </form>
    )
}

export default Login