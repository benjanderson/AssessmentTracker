var toastr = require("toastr");
module.exports = ["$stateParams", "canidateRepository", function ($stateParams, canidateRepository) {
	var ctrl = this;
	var id = $stateParams.assessmentId;

	canidateRepository.getCanidate(id).then((data) => {
		ctrl.assessment = data;
		ctrl.resumeUrl = canidateRepository.resumeUrl(data.resumeFileId, data.resumeFileName);
		ctrl.assessmentUrl = canidateRepository.assessmentUrl(data.assessmentFileId, data.assessmentFileName);
	});

		canidateRepository.getQuestions(id).then((data) => {
			ctrl.questions = data;
		});
	}
];