﻿module.exports = [
	"$http", "$q", function ($http, $q) {
		var svc = this;

		function getCandidate(id) {
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

		function saveCandidate(candidate, assessmentFile, resumeFile, httpMethod) {
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
				data: { model: candidate, files: [assessmentFile, resumeFile] }
			});
		};

		function resumeUrl(resumeFileId, resumeFileName) {
		    if (resumeFileId && resumeFileName) {
		        return "files/" + resumeFileId + "/" + resumeFileName;
		    } else {
		        return false;
		    }
		}

		function assessmentUrl(assessmentFileId, assessmentFileName) {
		    if (assessmentFileId && assessmentFileName) {
		        return "files/" + assessmentFileId + "/" + assessmentFileName;
		    } else {
		        return false;
		    }
		}

		function getQuestions(assessmentId) {
			return $http.get("questions/", { params: { assessmentId: assessmentId } }).then(result => {
				return result.data;
			});
		}

		function saveQuestions(assessmentId, questions) {
			return $http.post("questions", {
				assessmentId: assessmentId,
				questions: questions
			}).then(result => {
				var defer = $q.defer();
				defer.resolve(result.data);
				return defer.promise;
			});
		}

		function getAssessmentSummary(assessmentId)
		{
			return $http.get("assessment/summary/", {
				params: { assessmentId: assessmentId }
			}).then(result => {
				var defer = $q.defer();
				defer.resolve(result.data);
				return defer.promise;
			});
		}

		svc.getCandidate = getCandidate;
		svc.saveCandidate = saveCandidate;
		svc.getOpenAssessments = getOpenAssessments;
		svc.resumeUrl = resumeUrl;
		svc.assessmentUrl = assessmentUrl;
		svc.getQuestions = getQuestions;
		svc.saveQuestions = saveQuestions;
		svc.getAssessmentSummary = getAssessmentSummary;
		return svc;
	}
];