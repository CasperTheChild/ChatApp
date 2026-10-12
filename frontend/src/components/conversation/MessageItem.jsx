const MessageItem = ({ message }) => {
    return (
        <div>
            <p>{ message.content }</p>
        </div>
    );
}

export default MessageItem