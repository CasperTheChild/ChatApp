import { useState } from 'react'
import Login from '../../components/auth/Login'
import Register from '../../components/auth/Register'

export default function LoginPage() {
    const [showLogin, setShowLogin] = useState(true);

    const handleLoginButton = () => {
        setShowLogin(true);
    }

    const handleRegisterButton = () => {
        setShowLogin(false);
    }

    return (
        <section
            className="p-4 d-flex flex-column gap-4 border border-1 rounded-4 bg-white w-33">
            <h2
                className="mt-3 center text-center">Welcome to ChatApp</h2>

            <div
                className="">
                {showLogin && <Login />}
                {!showLogin && <Register />}
            </div>

            <hr className="p-0 m-0"/>

            <div
                className="">
                {!showLogin && 
                    <button className="btn border btn-subtle"
                        onClick={handleLoginButton}>
                        You got an account already?
                    </button>
                }
                {showLogin &&
                    <button className="btn border btn-subtle"
                        onClick={handleRegisterButton}>
                        Not registered yet?
                    </button>
                }
            </div>
        </section>
    )
}