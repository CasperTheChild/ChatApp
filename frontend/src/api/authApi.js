const url = "https://localhost:7158/api/authentication"

export async function loginApi(username, password) {
    try {
        const loginUrl = url + "/login"

        const response = await fetch(loginUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                userName: username,
                password: password
            })
        })

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText || 'Failed to log in');
        }

        const text = await response.json();
        const token = text.token;

        localStorage.setItem('token', token)

        return token;
    }
    catch (error) {
        console.log(error.message);
    }
}

export async function registerApi(username, password, confirmPassword) {
    try {
        const registerUrl = url + "/register"

        const response = await fetch(registerUrl, {
            method: "POST",
            headers: {
                "Content-type": "application/json"
            },
            body: JSON.stringify({
                UserName: username,
                Password: password,
                ConfirmPassword: confirmPassword
            })
        })

        if (!response.ok) {
            return false;
        }

        return true;
    }
    catch (error) {
        console.log(error.message);
    }
}