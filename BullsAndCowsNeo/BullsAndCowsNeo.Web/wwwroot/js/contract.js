function ab2str(buf) {
    return String.fromCharCode.apply(null, new Uint8Array(buf));
}

function hexstring2ab(str) {
    const result = []
    while (str.length >= 2) {
        result.push(parseInt(str.substring(0, 2), 16))
        str = str.substring(2, str.length)
    }
    return result
}

const connectionContract = new signalR.HubConnectionBuilder()
    .withUrl("/contracthub")
    .build();

connectionContract.on("UpdateContractInfo", (user, message) => {
    const encodedMsg = user + message;
    let decodedMsg = ab2str(hexstring2ab(message));
    var decodedMsg_uri_enc = encodeURIComponent(decodedMsg);
    var decodedMsg_uri_dec = decodeURIComponent(decodedMsg_uri_enc);
    console.log(decodedMsg_uri_dec);
    jQuery('#contractInfo').text(encodedMsg);
});

document.getElementById("sendContractButton").addEventListener("click", event => {
    connectionContract.invoke("UpdateContractInfo", "asdasduser", "message").catch(err => console.error(err.toString()));
    event.preventDefault();
});

connectionContract.start().catch(err => console.error(err.toString()));