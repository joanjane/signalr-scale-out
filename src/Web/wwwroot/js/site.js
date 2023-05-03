function broadcastMessage() {
    fetch("/api/broadcast");
}

function log(message) {
    var logElem = document.querySelector('#log');
    if (!logElem) {
        logElem = document.createElement('pre');
        logElem.id = 'log';
        document.body.appendChild(logElem);
    }
    logElem.innerText += message + "\n";
}

function connectHub() {
    var connection = new signalR.HubConnectionBuilder().withUrl("/test").build();

    connection.on('welcomeMessage', (connectionId) => {
        log(`Welcome, you are ${connectionId}`);
    });

    connection.on('newFriendConnected', friendId => {
        log(`You have a new friend: ${friendId}`);
    });

    connection.on('friendLeft', friendId => {
        log(`${friendId} left`);
    });

    connection.on('globalMessage', globalMessage => {
        log(globalMessage);
    });

    connection.start();
}