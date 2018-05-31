// The following sample code uses modern ECMAScript 6 features 
// that aren't supported in Internet Explorer 11.
// To convert the sample for environments that do not support ECMAScript 6, 
// such as Internet Explorer 11, use a transpiler such as 
// Babel at http://babeljs.io/. 
//
// See Es5-chat.js for a Babel transpiled version of the following code:

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

connectionContract.start().catch(err => console.error(err.toString()));