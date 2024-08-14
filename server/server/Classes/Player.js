var shortId = require(`shortid`);
const Vector3 = require('./Vector3');

module.exports = class Player{
	constructor() {
		this.userName = ``;
		this.id = shortId.generate();
		this.position = new Vector3();
	}
}