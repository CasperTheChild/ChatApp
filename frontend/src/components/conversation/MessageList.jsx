import MessageItem from "./MessageItem"
import { GetMessagesApi } from "../../api/messageApi"
import { useState, useEffect } from "react"
import { UseHub } from "../../contexts/HubContext"
import { stop, addToGroup, removeFromGroup} from "../../hubs/chat"

const MessageList = ({conversationId}) => {
    const [messages, setMessages] = useState([]);
    const { connection } = UseHub();

    useEffect(() => {
        if (!connection || !conversationId) {
            setMessages([]);
            return;
        }

        let cancelled = false;
        let joined = false;

        setMessages([]);

        const handleReceiveMessage = (newMessage) => {
            if (String(newMessage.conversationId) !== String(conversationId)) {
                return;
            }

            setMessages(previousMessages => [
                ...previousMessages,
                newMessage
            ]);
        };

        connection.on("ReceiveMessage", handleReceiveMessage);

        const initialize = async () => {
            try {
                await addToGroup(connection, conversationId);
                joined = true;

                if (cancelled) return;

                const data = await GetMessagesApi(conversationId);

                if (!cancelled) {
                    setMessages(data);
                }
            } catch (error) {
                if (!cancelled) {
                    console.error("Failed to initialize messages:", error);
                }
            }
        };

        initialize();

        return () => {
            cancelled = true;
            connection.off("ReceiveMessage", handleReceiveMessage);

            if (joined) {
                removeFromGroup(connection, conversationId).catch(error => {
                    console.error("Failed to leave conversation group:", error);
                });
            }
        };
    }, [conversationId, connection]);

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