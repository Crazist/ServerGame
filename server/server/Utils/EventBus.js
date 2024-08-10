class EventBus {
	constructor() {
		this.handlers = {};
	}

	on(eventType, handler) {
		if (!this.handlers[eventType]) {
			this.handlers[eventType] = [];
		}
		this.handlers[eventType].push(handler);
	}

	off(eventType, handler) {
		if (this.handlers[eventType]) {
			this.handlers[eventType] = this.handlers[eventType].filter(h => h !== handler);
		}
	}

	emit(eventType, data) {
		if (this.handlers[eventType]) {
			this.handlers[eventType].forEach(handler => handler(data));
		} else {
			console.log(`No handlers found for event type: ${eventType}`);
		}
	}

}

module.exports = new EventBus();
