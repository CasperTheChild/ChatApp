import { useState, useEffect } from "react"
import { searchUsersApi } from "../../api/userApi";
    
const UserSearchList = ({ username, setCurrentUserId }) => {
    const [userList, setUserList] = useState([]);

    useEffect(() => {
        const loadConversations = async () => {
            try {
                const data = await searchUsersApi(username);
                setUserList(data);
            } catch (error) {
                console.error("Failed to load conversations:", error);
            }
        };

        loadConversations();
    })

    return (
        <section>
            <ul>
                {userList.map(item => {
                    return (
                        <userListItem
                            key={item.userId}
                            item={item}
                            setCurrentUserId={setCurrentUserId}
                        />
                    )
                })}
            </ul>
        </section>
    )
}

export default UserSearchList