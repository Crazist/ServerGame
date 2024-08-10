function createMessage(type, data) {
	return JSON.stringify({
		type: type,
		data: data
	});
}


module.exports = { createMessage };
 