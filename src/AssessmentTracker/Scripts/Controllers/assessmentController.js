var toastr = require("toastr");
var _ = require("underscore");
module.exports = ["$stateParams", "canidateRepository", "$state", function ($stateParams, canidateRepository, $state) {
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

	ctrl.disableSave = () => {
		return _.any(ctrl.questions, (question) => {
			return !question.score && question.score !== 0;
		});
	}

	ctrl.totalScore = () => {
		var scores = _.filter(_.map(ctrl.questions, (q) => q.score), (num) => num || num === 0);
		if (scores.length === 0) {
			return 0;
		}
		var sum = _.reduce(scores, (sum, num) => sum + num);
		return (sum / (scores.length * 2)) * 100;
	}

	ctrl.save = () => {
		canidateRepository.saveQuestions(id, ctrl.questions).then(() => {
			$state.go("home");
			toastr.success('Successfully Saved!');
		}, () => {
			toastr.error(status, 'Error Saving');
		});
	};
}];