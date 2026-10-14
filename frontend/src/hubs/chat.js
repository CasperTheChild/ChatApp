import { HubConnectionBuilder, HubConnectionState, LogLevel } from "@microsoft/signalr"

const url = "https://localhost:7158/hubs/chat"

var connection = null;

export default async function start(token) {
    if (connection?.state === HubConnectionState.Connected) {
        return connection;
    }

    if (connection) {
        await connection.stop();
    }
    
    connection = new HubConnectionBuilder()
        .withUrl(url, { accessTokenFactory: () => token })
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Information)
        .build();

    try {
        await connection.start();
        console.log("signalR connected!");
        return connection;
    }
    catch (error) {
        console.log(error);
        connection = null;
        throw error;
    }
}

export async function stop() {
    if (connection) {
        await connection.stop();
        connection = null;
    }
}


export async function addToGroup(connection, conversationId) {
    if (!connection) {
        throw new Error("SignalR connection is not available.");
    }

    await connection.invoke("AddToGroup", conversationId);
}

export async function removeFromGroup(connection, conversationId) {
    if (!connection) {
        throw new Error("SignalR connection is not available.");
    }

    await connection.invoke("RemoveFromGroup", conversationId);
}