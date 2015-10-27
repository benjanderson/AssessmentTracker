module.exports = [
	"$http", ($http) => {
		var svc = this;

		function getCanidate(id) {
			return $http.get("assessment", {
				params: { assessmentId: id }
			}).then((result) => {
				return result.data;
			});
		};

		function getOpenAssessments() {
			return $http.get("openAssessments")
				.then((result) => {
					return result.data;
				});
		}

		function saveCanidate(canidate, assessmentFile, resumeFile, httpMethod) {
			return $http({
				method: httpMethod,
				url: "assessment",
				headers: { 'Content-Type': undefined },
				transformRequest: function(data) {
					var formData = new FormData();
					formData.append("model", angular.toJson(data.model));
					for (var i = 0; i < data.files.length; i++) {
						if (data.files[i]) {
							formData.append("file" + i, data.files[i]);
						}
					}
					return formData;
				},
				data: { model: canidate, files: [assessmentFile, resumeFile] }
			});
		};

		svc.getCanidate = getCanidate;
		svc.saveCanidate = saveCanidate;
		svc.getOpenAssessments = getOpenAssessments;
		return svc;
	}
];