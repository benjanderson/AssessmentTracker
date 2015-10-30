var showdown = require("showdown");
module.exports = ["$sanitize", function ($sanitize) {
	return {
		restrict: 'AE',
		link: function (scope, element, attrs) {
			var sd = new showdown.Converter();

			if (attrs.markdown) {
				scope.$watch(attrs.markdown, function (newVal) {
					var html = newVal ? $sanitize(sd.makeHtml(newVal)) : '';
					element.html(html);
				});
			} else {
				var html = $sanitize(sd.makeHtml(element.text()));
				element.html(html);
			}
		}
	};
}]