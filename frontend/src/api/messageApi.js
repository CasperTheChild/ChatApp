const url = "https://localhost:7158/api/messages"

export async function SendMessageApi() {

}

export async function GetMessagesApi(conversationId) {
    const conversationUrl = `${url}/${conversationId}`;
    const token = localStorage.getItem("token");

    try {
        const response = await fetch(conversationUrl, {
            method: "GET",
            headers: {
                "Content-type": "application/json",
                "Authorization": `Bearer ${token}`
            }
        });
    }
}