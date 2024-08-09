var shortId = require(`shortid`);

module.exports = class Player{
	constructor() {
		this.userName = ``;
		this.id = shortId.generate();
	}
}