import * as signalR from "@microsoft/signalr";

class SignalRService {
    private connection: signalR.HubConnection | null = null;
    private socketUrl: string = "http://localhost:8093/MeasurePointHub";

    async startConnection(): Promise<void> {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(this.socketUrl)
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

        try {
            await this.connection.start();
            console.log("Connexion SignalR établie.");
        } catch (error) {
            console.error("Erreur lors de la connexion SignalR:", error);
        }
    }

    on(eventName: string, callback: (...args: any[]) => void): void {
        if (this.connection) {
            this.connection.on(eventName, callback);
        } else {
            console.error("SignalR non connecté.");
        }
    }

    async invoke<T>(methodName: string, content: object): Promise<T> {
        if (!this.connection) {
            throw new Error("Connexion SignalR non initialisée.");
        }
        try {
            return await this.connection.invoke<T>(methodName, content);
        } catch (error) {
            console.error(`Erreur lors de l'appel de '${methodName}':`, error);
            // throw error;
        }
    }

    async stopConnection(): Promise<void> {
        if (this.connection) {
            await this.connection.stop();
            console.log("Connexion SignalR arrêtée.");
        }
    }
}

export const signalRService = new SignalRService();
