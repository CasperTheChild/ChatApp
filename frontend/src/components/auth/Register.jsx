import { UseAuth } from "../../contexts/AuthContext.jsx";
import { useState } from "react";

const Register = () => {
    const [validated, setValidated] = useState(false);
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const { register } = UseAuth();

    const handleSubmit = async (e) => {
        e.preventDefault();

        const form = e.currentTarget;
        setValidated(true);

        if (!form.checkValidity()) {
            return;
        }

        try {
            register(username, password, confirmPassword);
        }
        catch (error) {
            console.error("Error during register in register component", error);
        }
    }

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
                    <label for="fieldText" class="form-label">Username</label>
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
                        Please, type your username.
                    </div>
                </div>

                <div className="mb-3">
                    <label for="fieldText" class="form-label">Username</label>
                    <input class="form-control"
                        type="password"
                        name="confirmPassword"
                        id="confirmPassword"
                        required
                        onChange={(e) => setConfirmPassword(e.target.value)} />
                    
                    <div class="invalid-feedback">
                        Password is required
                    </div>
                    
                    <div class="form-text">
                        Please, type your username.
                    </div>
                </div>
                <button className="btn border btn-subtle-primary"
                    type="submit">
                    Register
                </button>
            </div>
        </form>
    )
}

export default Register