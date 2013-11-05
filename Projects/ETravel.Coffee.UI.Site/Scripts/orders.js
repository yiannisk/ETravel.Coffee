$(function () {
	renderOrders();

	function renderOrder(orderData) {
		$.Mustache.load('./Templates/Order.htm')
		    .done(function () {
		        $('.orders-list').mustache('order', orderData, 'append');

				var $currentOrder = $('.orders-list .order:last');

				$currentOrder.on('click', function () {
					console.log('order click');
					$('.orders-list .order').removeClass('selected');
					$(this).addClass('selected');
				});
		    });
	}

	function renderOrders() {
		$('.orders-list').html('');

		$.ajax({
			type: "GET",
			dataType: "json",
			url: coffee.app.settings.serviceLocator + "/orders",
			success: function (data) {
				var i, currentOrder;

				for(i = 0; i < data.length; i++) {					
					renderOrder(data[i]);
				}
			}
		});
	}
});