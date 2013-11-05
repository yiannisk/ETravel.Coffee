$(function () {
	renderOrders();

	function renderOrders() {
		$.ajax({
			type: "GET",
			dataType: "json",
			url: coffee.app.settings.serviceLocator + "/orders",
			success: function (data) {
				console.log(data);
			}
		});
	}
});