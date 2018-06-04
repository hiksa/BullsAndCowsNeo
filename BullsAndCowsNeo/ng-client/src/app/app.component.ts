import { Component, NgZone } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { SignalRService } from "src/services/signalr-service";
import { ChatMessage } from "src/models/chat-message.model";

import Neon from '@cityofzion/neon-js';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  currentMessage: ChatMessage;
  allMessages: ChatMessage[];
  canSendMessage: boolean;

  constructor(
    private _signalRService: SignalRService,
    private _ngZone: NgZone,
    private _http: Http
  ) {
    this.subscribeToEvents();
    this.currentMessage = new ChatMessage();
    this.allMessages = [];
  }

  sendMessage() {
    if (this.canSendMessage) {
      this.currentMessage.sent = new Date();
      this._http.post(`http://localhost:5000/api/values`,
          { value: this.currentMessage.message },
          this.getJsonHeaders())
        .subscribe();
    }
  }

  protected getJsonHeaders(): RequestOptions {
    const headers = new Headers({ 'Content-Type': 'application/json' });
    return new RequestOptions({ headers: headers });
  }

  private subscribeToEvents(): void {
    this._signalRService.connectionEstablished.subscribe(() => {
      this.canSendMessage = true;
    });

    this._signalRService.messageReceived.subscribe((message: any) => {
      this._ngZone.run(() => {
        this.currentMessage = new ChatMessage();
        this.allMessages.push(
          new ChatMessage(message.value)
        );
      });
    });
  }
}
