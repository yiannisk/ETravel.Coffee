function Resource(name, url, actions, options) {
	var i, 
		actions = actions || ['get', 'post', 'put', 'delete'], 
		options = options || { dataType: 'json', contentType: 'application/json' },
		self = this;

	self.name = name;
	self.url = url || coffee.app.settings.serviceLocator + '/' + name;
	
	actions.map(function (action) {
		var action = action;

		self[action] = function (callback, methodOptions) {
			var o = $.extend(options, methodOptions, {
				url: self.url, 
				type: action,

				success: function (data) {
					if (callback) callback(data);
				}, 

				error: function () {
					if (callback) callback(null);
				}
			});

			$.ajax(o);
		};
	});

	return self;
}