import { useState, useEffect } from "react"
import { getConversationsApi } from "../../api/conversationApi";
import ConversationListItem from "./ConversationListItem"

const ConversationList = ({ setCurrentConversationId }) => {
    const [conversationList, setConversationList] = useState([]);

    useEffect(() => {
        const loadConversations = async () => {
            try {
                const data = await getConversationsApi();
                setConversationList(data);
            } catch (error) {
                console.error("Failed to load conversations:", error);
            }
        };

        loadConversations();
    }, [])

    const handleAgain = async () => {
        setConversationList(await getConversationsApi())
    }

    return (
        <>
            <ul>
                {conversationList.map(item => {
                    return (
                        <ConversationListItem
                            key={item.key}
                            item={item}
                            setCurrentConversationId={setCurrentConversationId}
                        />
                    )
                })}
            </ul>

            <button onClick={handleAgain}>
                Again
            </button>
        </>
    )
}

export default ConversationList