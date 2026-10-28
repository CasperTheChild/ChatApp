import { UseHub } from "../../contexts/HubContext";
import { useState } from "react";

const MessageUpdateInput = (message) => {
    const { connection } = UseHub();

    const [newMessage, setNewMessage] = useState({
        conversationId: message.conversationId,
        messageId: message.messageId,
        content: message.content
    });

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!newMessage.content.trim()
            || !connection)
            return;
        
        try {
            connection.invoke("UpdateMessage", message.conversationId, message.messageId, newMessage);
        }
        catch (error) {
            console.error("Couldn't update a message", error);
        }
    }

    return (
        <section>
            <form onSubmit={handleSubmit}>
                <input
                    id="message"
                    type="text"
                    value={message.content}
                    onChange={(e) =>
                        setNewMessage({
                            ...message,
                            content: e.target.value
                        })
                    }
                />
                <button type="submit">Send</button>
            </form>
        </section>
    )
}

export default MessageUpdateInput