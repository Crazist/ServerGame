const http = require('http');

const port = 8080;

const server = http.createServer((req, res) => {
	// Разрешение CORS
	res.setHeader('Access-Control-Allow-Origin', '*');
	res.setHeader('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PUT, PATCH, DELETE');
	res.setHeader('Access-Control-Allow-Headers', 'X-Requested-With,content-type');
	res.setHeader('Access-Control-Allow-Credentials', true);

	// Отправка ответа
	res.writeHead(200, { 'Content-Type': 'text/plain' });
	res.end('Hello number one\n');
});

server.listen(port, () => {
	console.log(`Server listening on: http://localhost:${port}`);
});
