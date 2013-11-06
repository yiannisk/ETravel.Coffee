$(function () {
	/**************** Initialization ******************/
	var orders = new Resource('orders'), currentCountdown;

	var orderItemsFor = function (orderId) {
		return new Resource(
			'order_items', 
			coffee.app.settings.serviceLocator + '/orders/' + orderId + '/items'
		);
	};

	renderOrders();

	$('.new-order-form #ExpiresAtDate').datepicker({
		format: 'dd/mm/yyyy',
		viewMode: 'days',
		minViewMode: 'days'
	}).on('changeDate', function (evt) {
		$(this).datepicker('hide');
	});

	$('.new-order-form #ExpiresAtTime').timepicker({
        minuteStep: 5,
        showInputs: false,
        disableFocus: true,
        showMeridian: false
    });

    $('.new-order-form input').jqBootstrapValidation();

    $('.new-order-form').on('submit', function (evt) {
    	var $form = $(this);

    	evt.preventDefault();
    	evt.stopPropagation();

    	var request = {
    		data: JSON.stringify({
	    		Vendor: $form.find('#Vendor').val(),
	    		Owner: $form.find('#Owner').val(),
	    		ExpiresAt: getIsoDateFromGreekDate($form.find('#ExpiresAtDate').val()) 
	    			+ ' ' + $form.find('#ExpiresAtTime').val() + ':00'
	    	})
    	};

    	orders.post(function () {}, request);
    	return false;
    });

    $(document).on('click', '.order .remove', function () {
    	var $order = $(this).closest('.order'),
    		orderId = $order.find('.order-id').val();

    	var request = { data: JSON.stringify({ Id: orderId }) };
    	orders.delete(function () {}, request);
    });

    $(document).on('click', '.order .place-order', function () {
    	var $order = $(this).closest('.order');
    	$order.find('.order-item-form').show();
    });

    $(document).on('submit', '.order .order-item-form form', function () {
    	evt.preventDefault();
    	evt.stopPropagation();

    	console.log('Submitting order item form...');

    	return false;
    });


	/**************** Helpers ******************/
	function getIsoDateFromGreekDate(greekDateString) {
		var parts = greekDateString.split('/');
		return parts[2] + '-' + parts[1] + '-' + parts[0];
	}

	function getDateFromIsoDateString(isoDateString) {
		var dateTimeParts = isoDateString.split(' '),
			dateParts = dateTimeParts[0].split('-'),
			timeParts = dateTimeParts[1].split(':'),
			year = parseInt(dateParts[0]),
			month = parseInt(dateParts[1]) - 1, // f..k this JS month shit
			day = parseInt(dateParts[2]),
			hour = parseInt(timeParts[0]),
			minute = parseInt(timeParts[1]),
			second = parseInt(timeParts[2]);

		return new Date(year, month, day, hour, minute, second);
	}

	function renderOrder(orderData) {
		$.Mustache.load('./Templates/Order.htm')
		    .done(function () {
		        $('.orders-list').mustache('order', orderData, 'append');
				var $currentOrder = $('.orders-list .order:last');

				var expiresAtText = $currentOrder.find('.expires-at').text();
				var expirationTime = getDateFromIsoDateString(expiresAtText);
				
				$currentOrder.data('expiration', expiresAtText);

				$currentOrder.data('interval', countdown(expirationTime, function (timespan) {
					if (timespan.value < 0) {
						$currentOrder.find('.expires-at').html(timespan.toString());
					} else {
						$currentOrder.find('.expires-at')
							.html('<i>Order closed - expired at: ' + $currentOrder.data('expiration') + '</i>');
						
						clearInterval($currentOrder.data('interval'));

						// elapsed orders should not allow for adding items.
						$currentOrder.find('.place-order').hide();
					}
				}));

				$currentOrder.on('click', function () {
					var selectedItemId = $('.orders-list .selected.order .order-id').val();

					// Do nothing if the same item is clicked multiple times.
					if (selectedItemId === orderData.Id) return;

					$('.orders-list .order').removeClass('selected');
					$('.orders-list .order .order-items').hide();
					$('.orders-list .order .order-item-form').hide();
					$('.orders-list .order .actions').hide();

					$(this).addClass('selected');
					$(this).find('.order-items').show();

					$('.orders-list .selected.order .actions').show();

					$currentOrder
						.find('.order-items')
						.html('<div class="loader"><i class="fa fa-cog fa-spin large"></i> Loading...</div>');
					
					orderItemsFor(orderData.Id).get(function (data) {
						for(i = 0; i < data.length; i++) {					
							renderOrderItems($currentOrder, data[i]);
						}

						$currentOrder.find('.order-items .loader').remove();
					});
				});
		    });
	}

	function renderOrderItems($currentOrder, orderItemData) {
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