import { UseHub } from "../../contexts/HubContext"
import { useState } from "react";

const MessageInput = ({ conversationId }) => {
    const [message, setMessage] = useState({
        "conversationId": conversationId,
        "content": ""
    });

    const { connection } = UseHub();

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!message.content.trim() || !connection) return;

        try {
            await connection.invoke("SendMessage", message);

            setMessage({
                conversationId: conversationId,
                content: ""
            });
        } catch (error) {
            console.error("Failed to send message:", error);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <input
                id="message"
                type="text"
                placeholder="Message Content"
                value={message.content}
                onChange={(e) =>
                    setMessage({
                        ...message,
                        content: e.target.value
                    })
                }
            />
            <button type="submit">Send</button>
        </form>
    )
}

export default MessageInput

