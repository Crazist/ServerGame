const WebSocket = require(`ws`);
const Player = require(`./Classes/Player.js`);
const eventBus = require('./Utils/EventBus');
const playerModule = require('./Modules/PlayerModule.js');
const { createMessage } = require(`./Utils/messageUtils.js`)

const port = process.env.PORT || 52300;
const wss = new WebSocket.Server({ port });

let players = {};
let sockets = {};

wss.on('connection', (ws) => {
	console.log('Connection Made');

	const player = new Player();
	const thisPlayerID = player.id;

	player.position.x = Math.random() * 10 - 5; 
	player.position.y = 1;                     
	player.position.z = Math.random() * 10 - 5;

	players[thisPlayerID] = player;
	sockets[thisPlayerID] = ws;

	ws.send(createMessage('register', { id: thisPlayerID }));

	ws.send(createMessage('spawn', { player }));

	broadcast(createMessage('spawn', { player }), thisPlayerID);

	ws.on('message', (message) => {
		try {
			let parsedMessage = JSON.parse(message);
			let parsedData = JSON.parse(parsedMessage.Data);

			eventBus.emit(parsedMessage.Type, parsedData);
		} catch (error) {
			console.error('Failed to parse message:', error);
		}
	});


	ws.on('close', () => {
		console.log('A player has disconnected');

		delete players[thisPlayerID];
		delete sockets[thisPlayerID];

		broadcast(createMessage('disconnect', { id: thisPlayerID }));
	});
});

function broadcast(message, excludeID) {
	for (let playerID in sockets) {
		if (playerID !== excludeID) {
			sockets[playerID].send(message);
		}
	}
}

console.log('WebSocket server is running on ws://localhost:' + port);
