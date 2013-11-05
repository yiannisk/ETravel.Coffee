$(function () {
	var orders = new Resource('orders');

	var orderItemsFor = function (orderId) {
		return new Resource(
			'order_items', 
			coffee.app.settings.serviceLocator + '/orders/' + orderId + '/items'
		);
	};

	renderOrders();

	function renderOrder(orderData) {
		$.Mustache.load('./Templates/Order.htm')
		    .done(function () {
		        $('.orders-list').mustache('order', orderData, 'append');
				var $currentOrder = $('.orders-list .order:last');

				$currentOrder.on('click', function () {
					var selectedItemId = $('.orders-list .selected.order .id').val();

					// Do nothing if the same item is clicked multiple times.
					if (selectedItemId === orderData.Id) return;

					$('.orders-list .order').removeClass('selected');
					$(this).addClass('selected');
					
					orderItemsFor(orderData.Id).get(function (data) {
						renderOrderItems(data);
					});
				});
		    });
	}

	function renderOrderItems(orderItemData) {
		console.log(orderItemData);
	}

	function renderOrders() {
		orders.get(function (data) {
			var i, currentOrder;

			$('.orders-list').html('');

			for(i = 0; i < data.length; i++) {					
				renderOrder(data[i]);
			}
		});
	}
});