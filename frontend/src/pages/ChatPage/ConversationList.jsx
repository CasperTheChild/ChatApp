import { useState, useEffect } from "react"
import { getConversationsApi } from "../../api/conversationApi";

const ConversationList = () => {
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

    const conversationListItems = conversationList.map(item => 
        <li key={ item.conversationId}>
            <p>{item.conversationId}</p>
            <p>{item.title}</p>
        </li>
    )

    const handleAgain = async () => {
        setConversationList(await getConversationsApi())
    }

    return (
        <>
            <ul>
                {conversationListItems}
            </ul>

            <button onClick={handleAgain}>
                Again
            </button>

            
        </>
    )
}

export default ConversationList