import Conversation from "./Conversation"
import ConversationList from "./ConversationList"
import { useState } from "react";

const ChatPage = () => {
    const [currentConversationId, setCurrentConversationId] = useState(null);

    return (
        <>
            <ConversationList setCurrentConversationId={ setCurrentConversationId } />
            <Conversation currentConversationId={ currentConversationId } />
        </>
    )
}

export default ChatPage