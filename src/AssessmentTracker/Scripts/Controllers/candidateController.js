var toastr = require("toastr");
module.exports = ["$http", "$stateParams", "$state", "candidateRepository", function ($http, $stateParams, $state, candidateRepository) {
	var ctrl = this;
	var assessment;
	var resume;
	ctrl.showFiles = true;

	$http.get("positions").then((result) => {
		ctrl.positions = result.data;
	});

	if ($stateParams.assessmentId) {
		candidateRepository.getCandidate($stateParams.assessmentId).then((result) => {
		    if (result.dateOfAssessment) {
		        result.dateOfAssessment = new Date(result.dateOfAssessment);
		    }
		    if (result.dateOfDeadline) {
		        result.dateOfDeadline = new Date(result.dateOfDeadline);
		    }
		    ctrl.showFiles = false;
			ctrl.candidate = result;
			ctrl.assessmentUrl = "files/" + ctrl.candidate.assessmentFileId + "/" + encodeURI(ctrl.candidate.assessmentFileName);
			ctrl.resumeUrl = "files/" + ctrl.candidate.resumeFileId + "/" + encodeURI(ctrl.candidate.resumeFileName);
		});
	} else {
		ctrl.candidate = {
			name: null,
			position: null,
			dateOfAssessment: new Date(),
			active: true
		};
	}

	function saveCandidate() {
		candidateRepository.saveCandidate(ctrl.candidate, ctrl.assessment, ctrl.resume, $stateParams.assessmentId ? "PUT" : "POST")
			.success(() => {
				$state.go("home");
				toastr.success('Successfully Saved!');
			}).error(() => {
				toastr.error(status, 'Error Saving');
			});
	}

	function resumeUpload(files) {
		ctrl.resume = files[0];
	}

	function assessmentUpload(files) {
		ctrl.assessment = files[0];
	}

	function replaceFiles() {
		ctrl.showFiles = true;
	}

	ctrl.save = saveCandidate;
	ctrl.assessmentUpload = assessmentUpload;
	ctrl.resumeUpload = resumeUpload;
	ctrl.replaceFiles = replaceFiles;
}];