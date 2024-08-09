const WebSocket = require('ws');

const port = process.env.PORT || 52300;
const wss = new WebSocket.Server({ port });

wss.on('connection', (ws) => {
	console.log('Client connected');

	// Отправка сообщения клиенту при подключении
	ws.send('Hello from server');

	ws.on('message', (message) => {
		console.log('Received:', message);
		// Отправка ответа клиенту на полученное сообщение
		ws.send('Echo: ' + message);
	});

	ws.on('close', () => {
		console.log('Client disconnected');
	});
});

console.log('WebSocket server is running on ws://localhost:' + port);
