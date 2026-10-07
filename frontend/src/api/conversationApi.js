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
            throw new Error(errorText || 'Failed to get conversations');
        }

        return response.json();
    }
    catch (error) {
        console.error(error.message);
    }
}

export async function getConversationByIdApi(conversationId) {
    const conversationUrl = `${url}/${conversationId}`;
    const token = localStorage.getItem("token");

    try {
        const response = await fetch(conversationUrl, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            }
        });

        if (!response.ok) {
            const errorText = response.text();
            throw new Error(errorText || 'Failed to get a conversation by id');
        }

        return response.json();
    }
    catch (error) {
        console.error(error.message);
    }
}

export async function createConversationApi(conversationTitle, conversationDescription) {
    const token = localStorage.getItem("token");

    try {
        const response = await fetch(url, {
            method: "POST",
            headers: {
                "Content-type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify({
                Title: conversationTitle,
                Description: conversationDescription
            })
        })

        if (response.ok()) {
            const errorText = response.text();
            throw new Error(errorText || 'Failed to create a conversation');
        }

        return response.json();
    }
    catch (error) {
        console.error(error.message);
    }
}

export async function leaveConversationApi(conversationId) {
    const token = localStorage.getItem("token");
    const leaveUrl = `${url}/${conversationId}/leave`

    try {
        const response = await fetch(leaveUrl, {
            method: "POST",
            headers: {
                "Content-type": "application/json",
                "Authorization": `Bearer ${token}`
            }
        })

        if (!response.ok()) {
            const errorText = response.text();
            throw new Error(errorText || 'Failed to leave a conversation');
        }

        return response.json();
    }
    catch (error) {
        console.error(error.message);
    }
}

export default { getConversationByIdApi, getConversationsApi, createConversationApi, leaveConversationApi }