const url = "https://localhost:7158/api/conversations"

export async function getConversationsApi() {
    const token = localStorage.getItem("token");

    try {
        const response = await fetch(url, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            }
        });

        if (!response.ok) {
            const errorText = response.text();
            throw new Error(errorText || 'Failed to log in');
        }

        return response.json();
    }
    catch (error) {
        console.error(error.message);
    }
}

export async function getConversationByIdApi(id) {
    const conversationUrl = '${url}/${id}';

    try {
        const response = await fetch(conversationUrl, {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            }
        });

        if (!response.ok) {
            const errorText = response.text();
            throw new Error(errorText || 'Failed to log in');
        }

        return response.json();
    }
    catch (error) {
        console.error(error.message);
    }
}

export default { getConversationByIdApi, getConversationsApi }