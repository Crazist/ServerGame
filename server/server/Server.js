const WebSocket = require(`ws`);
const Player = require(`./Classes/Player.js`);
const { createMessage } = require(`./Utils/messageUtils.js`)

const port = process.env.PORT || 52300;
const wss = new WebSocket.Server({ port });

let players = {};
let sockets = {};

wss.on('connection', (ws) => {
	console.log('Connection Made');

	const player = new Player();
	const thisPlayerID = player.id;

	players[thisPlayerID] = player;
	sockets[thisPlayerID] = ws;

	ws.send(createMessage('register', { id: thisPlayerID }));

	ws.send(createMessage('spawn', { player }));

	broadcast(createMessage('spawn', { player }), thisPlayerID);

	for (let playerID in players) {
		if (playerID !== thisPlayerID) {
			ws.send(createMessage('spawn', { player: players[playerID] }));
		}
	}

	ws.on('message', (message) => {
		console.log('Received:', message);
		// Здесь можно добавить обработку сообщений
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
