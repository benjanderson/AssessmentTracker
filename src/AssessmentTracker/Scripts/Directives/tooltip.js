module.exports = ["$sanitize", function ($sanitize) {
	return {
		restrict: 'AE',
		link: function (scope, element, attrs) {
		element.attr("title", attrs.tooltip);
			if (attrs.tooltip) {
				scope.$watch(attrs.tooltip, function (newVal) {
					element.attr("title", newVal);
					element.tooltip();
				});
			} else {
				element.tooltip();
			}
		}
	};
}]