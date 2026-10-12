import ConversationList from "../../components/conversationList/ConversationList";
import Conversation from "../../components/conversation/Conversation";
import { useState } from "react";

const ChatPage = () => {
    const [currentConversationId, setCurrentConversationId] = useState(null);

    return (
        <>
            <ConversationList setCurrentConversationId={setCurrentConversationId} />
            
            {currentConversationId !== null &&
                <Conversation currentConversationId={ currentConversationId } />
            }
        </>
    )
}

export default ChatPage