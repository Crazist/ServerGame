const eventBus = require('../Utils/EventBus');

/**
 * @param {
 * { Input: string },
 * { Rofl: int }
 * } data
 */
function handleRegister(data) {
	console.log('Handling register with data:', data + " Input = " + data.Input);
	// Логика регистрации
}

function handleSpawn({ data }) {
	console.log('Handling spawn with data:', data);
	// Логика спауна
}

// Регистрация обработчиков в EventBus
eventBus.on('register', handleRegister);
eventBus.on('spawn', handleSpawn);
