export async function LoginApi(email, password) {
    const url = `/api/Authentication/login`
    const res = await fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password }),
    })

    if (!res.ok) {
        const errorText = await res.text();
        throw new Error(errorText || 'Failed to log in');
    }

    const text = await res.json();
    const token = text.token;

    localStorage.setItem('token', token);
    return token;
}

export async function RegisterApi(email, password, confirmPassword) {
    const url = `api/Authentication/register`
    const res = await fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password, confirmPassword }),
    })

    if (!res.ok) {
        return false;
    }

    return true;
}