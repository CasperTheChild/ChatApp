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

        const handleReceivedMessage = (newMessage) => {
            if (String(newMessage.conversationId) !== String(conversationId)) {
                return;
            }

            setMessages(previousMessages => [
                ...previousMessages,
                newMessage
            ]);
        };

        const handleReceivedUpdatedMessage = (newMessage) => {
            if (String(newMessage.conversationId) !== String(conversationId)) {
                return;
            }

            setMessages(prev => 
                prev.map(message =>
                    message.messageId == newMessage.messageId
                        ? { ...message, content: newMessage.content }
                        : message
                )
            )
        }

        const handleReceivedDeletedMessage = (deleteMessageDto) => {
            if (String(deleteMessageDto.conversationId) !== String(conversationId)) {
                return;
            }

            setMessages(prev =>
                prev.filter(message => message.messageId !== deleteMessageDto.messageId)
            )
        }

        connection.on("ReceiveMessage", handleReceivedMessage);
        connection.on("ReceiveUpdatedMessage", handleReceivedUpdatedMessage);
        connection.on("ReceiveDeletedMessage", handleReceivedDeletedMessage);

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
            connection.off("ReceiveMessage", handleReceivedMessage);
            connection.off("ReceiveUpdatedMessage", handleReceivedUpdatedMessage);
            connection.off("ReceiveDeletedMessage", handleReceivedDeletedMessage);

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