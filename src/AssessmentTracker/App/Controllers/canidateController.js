var toastr = require("toastr");
module.exports = ["$http", "$stateParams", "$state", "canidateRepository", function ($http, $stateParams, $state, canidateRepository) {
	var ctrl = this;
	var assessment;
	var resume;
	ctrl.showFiles = true;

	$http.get("positions").then((result) => {
		ctrl.positions = result.data;
	});

	if ($stateParams.assessmentId) {
		canidateRepository.getCanidate($stateParams.assessmentId).then((result) => {
			result.dateOfAssessment = new Date(result.dateOfAssessment);
			result.dateOfDeadline = new Date(result.dateOfDeadline);
			ctrl.showFiles = false;
			ctrl.canidate = result;
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
		canidateRepository.saveCanidate(ctrl.canidate, ctrl.assessment, ctrl.resume, $stateParams.assessmentId ? "PUT" : "POST")
			.success(() => {
				$state.go("home");
				toastr.success('Successfully Saved!');
			}).error(() => {
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