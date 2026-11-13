const url = "https://localhost:7158/api/Users/username/"

async function searchUsersApi(username) {
    const conversationUrl = `${url}/${username}`;
    const token = localStorage.getItem("token");

    try {
        const response = await fetch(conversationUrl, {
            method: "GET",
            headers: {
                "Content-type": "application/json",
                "Authorization": `Bearer ${token}`
            }
        });

        if (!response.ok) {
            const errorText = response.text();
            throw new Error(errorText || 'Failed to get messages');
        }

        return response.json();
    }
    catch (error) {
        console.error(error.message);
    }
}

export default {searchUsersApi}