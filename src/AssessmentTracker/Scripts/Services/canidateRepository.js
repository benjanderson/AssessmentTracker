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

		function resumeUrl(resumeFileId, resumeFileName) {
			return "files/" + resumeFileId + "/" + resumeFileName;
		}

		function assessmentUrl(assessmentFileId, assessmentFileName) {
			return "files/" + assessmentFileId + "/" + assessmentFileName;
		}

		function getQuestions(assessmentId) {
			return $http.get("questions", { params: { assessmentId: assessmentId } }).then(result => {
				return result.data;
			});
		}

		function saveQuestions(assessmentId, questions) {
			return $http.post("questions", {
				assessmentId: assessmentId,
				questions: questions
			}).then(result => {
				return result.data;
			});
		}

		svc.getCanidate = getCanidate;
		svc.saveCanidate = saveCanidate;
		svc.getOpenAssessments = getOpenAssessments;
		svc.resumeUrl = resumeUrl;
		svc.assessmentUrl = assessmentUrl;
		svc.getQuestions = getQuestions;
		svc.saveQuestions = saveQuestions;
		return svc;
	}
];