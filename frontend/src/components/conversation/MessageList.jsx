import MessageItem from "./MessageItem"
import { GetMessagesApi } from "../../api/messageApi"
import { useState, useEffect } from "react"
import { UseHub } from "../../contexts/HubContext"

const MessageList = ({conversationId}) => {
    const [messages, setMessages] = useState([]);
    const { connection } = UseHub();

    useEffect(() => {
        if (!connection || !conversationId) return;
        
        const handleReceiveMessage = (newMessage) => {
            if (newMessage.conversationId !== conversationId) return;

            setMessages(previousMessages => [
                ...previousMessages,
                newMessage
            ]);
        }

        connection.on("ReceiveMessage", handleReceiveMessage);

        const loadMessages = async () => {
            try {
                const data = await GetMessagesApi(conversationId);
                setMessages(data);
            }
            catch (error) {
                console.error("Failed to load messages in message list:", error);
            }
        }
        loadMessages();

        
        return () => {
            connection.off("ReceiveMessage", handleReceiveMessage);
        };
    }, [conversationId, connection])

    return (
        <>
            <ul>
                {messages.map(item => {
                    return (
                        <MessageItem
                            key={item.messageId}
                            message={item}
                        />
                    )
                })}
            </ul>
        </>
    )
}

export default MessageList