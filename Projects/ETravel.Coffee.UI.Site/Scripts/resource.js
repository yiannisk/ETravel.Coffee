function Resource(name, url, actions, options) {
	var i, 
		actions = actions || ['get', 'post', 'put', 'delete'], 
		options = options || { dataType: 'json' }
		self = this;

	this.name = name;
	this.url = url || coffee.app.settings.serviceLocator + '/' + name;
	
	for(i = 0, currentAction = actions[i]; i < actions.length; i ++) {
		this[currentAction] = function (callback, methodOptions) {
			var callback = callback, 
				action = currentAction;

			$.ajax($.extend(options, methodOptions, {
				url: self.url, 
				type: action, 
				success: function (data) {
					if (callback) callback(data);
				}, 

				error: function () {
					if (callback) callback(null);
				}
			}));
		};
	}
}