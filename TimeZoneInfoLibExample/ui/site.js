function post(url, data, success, error) {
	$.ajax({
		url: url,
		data: data,
		type: 'POST',
		dataType: 'json',
		success: success,
		error: error
	});
}
