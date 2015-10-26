module.exports = function () {
	return {
		restrict: 'A',
		link: function (scope, element, attrs) {
			var onChangeFunc = scope.$eval(attrs.fileUpload);
			element.bind('change', function (event) {
				var files = event.target.files;
				onChangeFunc(files);
			});

			element.bind('click', function () {
				element.val('');
			});
		}
	};
}