import { Component, NgZone } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { SignalRService } from "src/services/signalr-service";
import { ChatMessage } from "src/models/chat-message.model";
import Neon, { api } from '@cityofzion/neon-js';

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
    const url = `http://127.0.0.1:30333`;
    const neonDbUrl = 'http://localhost:5000';
    var address = 'AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y';
    var wif = 'KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr';
    
    var account = Neon.create.account(wif);
    
    var script = Neon.create.script({
      scriptHash: '85ed39acd8f0616b8b610cbf8395fb4ae6c4a752',
      operation: 'name',
      args: []
    });

    var config = {
      net: neonDbUrl,
      url: url,
      script: script,
      address: account.address,
      privateKey: account.privateKey,
      publicKey: account.publicKey,
      gas: 1,
      balance: null
    };

    api
      .neonDB
      .getBalance(neonDbUrl, account.address)
      .then(x => {
        config.balance = x;
        config.url = url;

        // Neon
        //   .doInvoke(config)
        //   .then(console.log);
      });
  }

  protected getJsonHeaders(): RequestOptions {
    const headers = new Headers({ 'Content-Type': 'application/json' });
    return new RequestOptions({ headers: headers });
  }
}