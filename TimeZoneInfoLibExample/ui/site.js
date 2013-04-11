function post(url, data) {
	return $.ajax({
		url: url,

		data: data,
		type: 'POST',
		dataType: 'json',
	});
}
