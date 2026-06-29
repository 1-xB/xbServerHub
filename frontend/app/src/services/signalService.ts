import * as SignalR from "@microsoft/signalr";

let connection: SignalR.HubConnection | null = null;


function createConnection() : SignalR.HubConnection {
    return new SignalR.HubConnectionBuilder()
        .withUrl("/hubs/metrics", {
            withCredentials: true,
        })
        .withAutomaticReconnect()
        .configureLogging(SignalR.LogLevel.Information)
        .build();
}

export function getConnection() : SignalR.HubConnection {
    if (!connection) {
        connection = createConnection();
    }
    return connection;
}

export async function startConnection() : Promise<void> {
    const conn = getConnection();
    if (conn.state === SignalR.HubConnectionState.Disconnected) {
        try {
            await conn.start();
        } catch (err) {
            console.error("Error while starting SignalR connection:", err);
        }
    }
}

export async function stopConnection() : Promise<void> {
    if (connection && connection.state !== SignalR.HubConnectionState.Disconnected) {
        await connection.stop();
    }
    connection = null;
}