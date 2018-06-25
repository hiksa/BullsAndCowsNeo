import { Component, NgZone } from '@angular/core';
import { Http, RequestOptions, Headers } from '@angular/http';
import { SignalRService } from "src/services/signalr-service";
import { ChatMessage } from "src/models/chat-message.model";
import Neon, { rpc, tx, CONST } from '@cityofzion/neon-js';

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
    // var privKey = Neon.get.privateKeyFromWIF('KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr');
    // console.log(privKey);
    // var publicKey = Neon.get.publicKeyFromPrivateKey(privKey);
    // console.log(publicKey);

    // Neon.get.balance('MainNet', `AMMGkco62euwJmeSpxDbC1emuh4ju5Nfko`)
    //   .then((data) => {
    //     console.log(data);
    //   })
    //   .catch((err) => {
    //     console.log(err);
    //   });

    const client = new rpc.RPCClient(`http://127.0.0.1:30333`, `2.7.4`);
    client.getAccountState(`AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y`)
      .then((data) => {
        // let newBal = new Balance();
        console.log(`client`, data);

        // let neobalance: AssetBalance = null;
        // for (var data_balance of data.balances) {
        //   console.log(data_balance);
        //   if (CONST.ASSET_ID.NEO === data_balance.asset.substring(2)) {
        //     console.log(CONST.ASSET_ID.NEO);
        //     neobalance = {
        //       balance: data_balance.value,
        //       unspent: null,
        //       spent: null,
        //       unconfirmed: null
        //     };
        //     // balance["assets"]["NEO"]["balance"] = data_balance.value;
        //   }
        // }

        // let balance: Balance = new Balance();
        // balance.address = `AK2nJJpJr6o664CWJKi1QRXjqeic2zRp8y`;
        // balance.net = `PrivateNet`;
        // balance.tokenSymbols = [];
        // balance.tokens = {};
        // balance.assetSymbols = [`NEO`, `GAS`];
        // balance.assets = {
        //   "NEO": neobalance,
        //   "GAS": null
        // };

        // console.log(`balance`, balance);

        let newtx = Neon.create.tx([{ type: 128 }]);
        // console.log(newtx);
        let seri2 = tx.serializeTransaction(newtx);
        // console.log(seri2);
        let txid2 = tx.getTransactionHash(newtx);
        // console.log(txid2);
        newtx.addOutput('NEO', 1, `AY9YhXtW8D4thieeaqhmpvueV4YB2wZGz9`);
        newtx.addRemark('I am sending 1 NEO to AY9YhXtW8D4thieeaqhmpvueV4YB2wZGz9');// Add an remark
        // newtx.calculate(balance); // Now we add in the balance we retrieve from an external API and calculate the required inputs.
        newtx.sign(`1dd37fba80fec4e6a6f13fd708d8dcb3b29def768017052f6c930fa1c5d90bbb`); // Sign with the private key of the balance
        // console.log(`newtx`, newtx);
        // let hash = newtx.hash;
        // console.log(`hash`, hash);
        // let serializedTx = newtx.serialize();
        // console.log(`serializedTx`, serializedTx);
      });
    // console.log(`block count`, client.getBlockCount());
    // console.log(`rawTx`, client.getRawTransaction(`1661781752b32ef7c827c0680591e5c3981e061b3d578f3933471ea029c9f410`, 1));
    // This will throw an error as invokefunction is not supported @ 2.3.2
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