const url = "https://localhost:7158/api/messages"

export async function GetMessagesApi(conversationId) {
    const conversationUrl = `${url}/conversations/${conversationId}`;
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

export async function SendMessageApi(conversationId, content) {
    const token = localStorage.getItem("token");

    try {
        const response = await fetch(Url, {
            method: "POST",
            headers: {
                "Content-type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: {
                "ConversationId": conversationId,
                "Content": content
            }
        })

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