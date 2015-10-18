var toastr = require("toastr");
module.exports = ["$http", "$stateParams", "$state", function ($http, $stateParams, $state) {
	var ctrl = this;
	var assessment;
	var resume;
	ctrl.showFiles = true;

	$http.get("positions").then((result) => {
		ctrl.positions = result.data;
	});

	if ($stateParams.assessmentId) {
		$http.get("assessment", {
			params: { assessmentId: $stateParams.assessmentId }
		}).then((result) => {
			result.data.dateOfAssessment = new Date(result.data.dateOfAssessment);
			result.data.dateOfDeadline = new Date(result.data.dateOfDeadline);
			ctrl.showFiles = false;
			ctrl.canidate = result.data;
			ctrl.assessmentUrl = "files/" + ctrl.canidate.assessmentFileId + "/" + encodeURI(ctrl.canidate.assessmentFileName);
			ctrl.resumeUrl = "files/" + ctrl.canidate.resumeFileId + "/" + encodeURI(ctrl.canidate.resumeFileName);
		});
	} else {
		ctrl.canidate = {
			name: null,
			position: null,
			dateOfAssessment: new Date()
		};
	}

	function saveCanidate() {
		$http({
			method: $stateParams.assessmentId ? "PUT" : "POST",
			url: "assessment",
			headers: { 'Content-Type': undefined },
			transformRequest: function (data) {
				var formData = new FormData();
				formData.append("model", angular.toJson(data.model));
				for (var i = 0; i < data.files.length; i++) {
					if (data.files[i]) {
						formData.append("file" + i, data.files[i]);
					}
				}
				return formData;
			},
			data: { model: ctrl.canidate, files: [assessment, resume] }
		}).
			success(function (data, status, headers, config) {
				$state.go("home");
				toastr.success('Successfully Saved!');
			}).
			error(function (data, status, headers, config) {
				toastr.error(status, 'Error Saving');
			});
	}

	function resumeUpload(files) {
		resume = files[0];
	}

	function assessmentUpload(files) {
		assessment = files[0];
	}

	function replaceFiles() {
		ctrl.showFiles = true;
	}

	ctrl.save = saveCanidate;
	ctrl.assessmentUpload = assessmentUpload;
	ctrl.resumeUpload = resumeUpload;
	ctrl.replaceFiles = replaceFiles;
}];