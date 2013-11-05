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
					var selectedItemId = $('.orders-list .selected.order .order-id').val();

					// Do nothing if the same item is clicked multiple times.
					if (selectedItemId === orderData.Id) return;

					$('.orders-list .order').removeClass('selected');
					$('.orders-list .order').find('.order-items').hide();
					$('.orders-list .order .actions').hide();

					$(this).addClass('selected');
					$(this).find('.order-items').show();

					$('.orders-list .selected.order .actions').show();

					$currentOrder
						.find('.order-items')
						.html('<div class="loader"><i class="fa fa-cog fa-spin large"></i> Loading...</div>');
					
					orderItemsFor(orderData.Id).get(function (data) {
						$currentOrder.find('.order-items').html('');
						for(i = 0; i < data.length; i++) {					
							renderOrderItems($currentOrder, data[i]);
						}
					});
				});
		    });
	}

	function renderOrderItems($currentOrder, orderItemData) {
		console.log(orderItemData);

		$.Mustache.load('./Templates/OrderItem.htm')
    		.done(function () {
				$currentOrder
					.find('.order-items')
					.mustache('order_item', orderItemData, 'append');
			});
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