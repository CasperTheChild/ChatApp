import { useState, useEffect } from "react"
import { getConversationByIdApi } from "../../api/conversationApi"
import MessageList from "./MessageList";
import MessageInput from "./MessageInput";

const Conversation = ({ currentConversationId }) => {
    const [conversation, setConversation] = useState(null);

    useEffect(() => {
        if (currentConversationId !== null) {
            const loadConversations = async () => {
                try {
                    const chatData = await getConversationByIdApi(currentConversationId);
                    setConversation(chatData);
                }
                catch (error) {
                    console.error("Failed to load conversation in conversation:", error);
                }
            }

            loadConversations();
        }
    }, [currentConversationId])

    return (
        <section>
            <section>
                <p>{conversation?.title}</p>
                <p>{conversation?.description}</p>
            </section>
            <hr/>
            <section>
                <MessageList
                    conversationId={currentConversationId}
                />
            </section>
            <section>
                <MessageInput
                    conversationId={currentConversationId}
                />
            </section>
        </section>
    )
}

export default Conversation