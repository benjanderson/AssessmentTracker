function focusIf($timeout) {
	function link($scope, $element, $attrs) {
		var dom = $element[0];
		function focus(condition) {
			if (condition) {
				$timeout(function () {
					dom.focus();
				}, $scope.$eval($attrs.focusDelay) || 0);
			}
		}

		if ($attrs.focusIf) {
			$scope.$watch($attrs.focusIf, focus);
		} else {
			focus(true);
		}
	}
	return {
		restrict: 'A',
		link: link
	};
}

module.exports = focusIf;

