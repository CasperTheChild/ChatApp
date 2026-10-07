import { getConversationByIdApi } from "../../api/conversationApi"
import { useState, useEffect } from "react"

const Conversation = ({ currentConversationId }) => {
    const [conversation, setConversation] = useState(null);
    const [messages, setMessages] = useState([]);

    useEffect(() => {
        if (currentConversationId !== null) {
            const loadConversations = async () => {
                try {
                    const data = await getConversationByIdApi(currentConversationId);
                    setConversation(data);
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
                <p>Messages</p>
            </section>
        </section>
    )
}

export default Conversation