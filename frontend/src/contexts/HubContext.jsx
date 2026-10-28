import { createContext, useContext, useState, useEffect } from "react";
import start, { addToGroup, removeFromGroup } from "../hubs/chat";
import { UseAuth } from "./AuthContext";

const HubContext = createContext();

export const UseHub = () => useContext(HubContext);

export const HubProvider = ({ children }) => {
    const [connection, setConnection] = useState();
    const { loggedIn, token } = UseAuth();

    useEffect(() => {
        let cancelled = false;

        const connect = async () => {
            if (!loggedIn || !token) {
                setConnection(null);
                await stop();
                return;
            }

            try {
                const conn = await start(token);

                if (cancelled) {
                    await stop();
                    return;
                }

                setConnection(conn);
            } catch (error) {
                console.error("Could not connect to chat hub:", error);
            }
        };

        connect();

        return () => {
            cancelled = true;
            stop();
        };
    }, [loggedIn, token])

    return (
        <HubContext.Provider value={{ connection, addToGroup, removeFromGroup, stop }}>
            { children }
        </HubContext.Provider>
    )
}