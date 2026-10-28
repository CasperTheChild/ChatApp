import { UseHub } from "../../contexts/HubContext";
import MessageUpdateInput from "./MessageUpdateInput";

const MessageItem = ({ message }) => {
    const { connection } = UseHub();
    const messageDeleteDto = {
        conversationId: message.conversationId,
        messageId: message.messageId
    }

    const handleDeleteButton = () => {
        if (!connection)
            return

        try {
            console.log("start invoking delete message method")
            connection.invoke("DeleteMessage", messageDeleteDto);
        }
        catch (error) {
            console.error("Failed to delete a message", error);
        }
    }

    return (
        <div>
            <section>
                <p>{ message.content }</p>
            </section>

            <button onClick={handleDeleteButton}>
                    delete
            </button>

            <MessageUpdateInput
                message={message}
            />
        </div>
    );
}

export default MessageItem