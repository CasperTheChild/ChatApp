const ConversationListItem = ({ item, setCurrentConversationId }) => {
    const handleClickCurrentConversation = (conversationId) => {
        setCurrentConversationId(conversationId);
    }

    return (
        <li>
            <p>{item.conversationId}</p>
            <p>{item.title}</p>
            <button onClick={() => handleClickCurrentConversation(item.conversationId)}>Open</button>
        </li>
    )
}

export default ConversationListItem