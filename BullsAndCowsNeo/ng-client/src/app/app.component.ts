import { Component, NgZone } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { SignalRService } from "src/services/signalr-service";
import { ChatMessage } from "src/models/chat-message.model";
import Neon, { api, u, sc } from '@cityofzion/neon-js';

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
    let shardsHash = '7939282c6428c93b3c894f41f1fc624d65fc911e';    
    const url = `http://127.0.0.1:30333`;
    const neonDbUrl = 'http://localhost:5000';
    var wif = 'KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr';
    var wif2 = 'Kz9LW1PxRpoKGtPRZcrfmhtWz9gWeBankSoRn1xmv59WrrxRDRNw';
    var account = Neon.create.account(wif);    
    var paramAddress = sc.ContractParam.byteArray(account.address, 'address');

    let args = [
      u.str2hexstring('f4aebcb2-dcf9-40e8-b678-7cc33ea7a5ad'),
      paramAddress['value']
    ];

    var script = Neon.create.script({
      scriptHash: shardsHash,
      operation: 'getstate',
      args: args
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

        Neon
          .doInvoke(config)
          .then(console.log);
      });
  }

  protected getJsonHeaders(): RequestOptions {
    const headers = new Headers({ 'Content-Type': 'application/json' });
    return new RequestOptions({ headers: headers });
  }
}