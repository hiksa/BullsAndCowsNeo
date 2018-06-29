import { EventEmitter, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { ChatMessage } from "src/models/chat-message.model";

@Injectable()
export class SignalRService {
  foodchanged = new EventEmitter();
  messageReceived = new EventEmitter<any>();
  newCpuValue = new EventEmitter<Number>();
  connectionEstablished = new EventEmitter<Boolean>();

  private connectionIsEstablished = false;
  private _hubConnection: HubConnection;

  constructor() {
    // this.createConnection();
    // this.registerOnServerEvents();
    // this.startConnection();
  }

  sendChatMessage(message: ChatMessage) {
    this._hubConnection.invoke('SendMessage', 'user', message.message);
  }

  private createConnection() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(`http://localhost:5000/chathub`)
      .build();
  }

  private startConnection(): void {
    this._hubConnection
      .start()
      .then(() => {
        this.connectionIsEstablished = true;
        console.log('Hub connection started');
        this.connectionEstablished.emit(true);
      })
      .catch(err => {
        console.log('Error while establishing connection, retrying...');
        setTimeout(this.startConnection(), 5000);
      });
  }

  private registerOnServerEvents(): void {
    this._hubConnection.on('ReceiveMessage', (message: any) => {
      this.messageReceived.emit(message);
    });
  }
}
