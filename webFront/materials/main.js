function retrieveAllContracts() {
    var targetUrl = "https://api.jcdecaux.com/vls/v3/contracts?apiKey=" + document.getElementById("apiKey").value;
    var requestType = "GET";

    var caller = new XMLHttpRequest();
    caller.open(requestType, targetUrl, true);
    caller.setRequestHeader("Accept", "application/json");
    caller.onload = contractsRetrieved;

    caller.send();
}

function contractsRetrieved() {
    // Let's parse the response:
    var response = JSON.parse(this.responseText);
    console.log(response);
}