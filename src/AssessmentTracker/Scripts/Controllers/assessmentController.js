var toastr = require("toastr");
var _ = require("underscore");
module.exports = ["$stateParams", "candidateRepository", "$state", "$scope", function ($stateParams, candidateRepository, $state, $scope) {
	var ctrl = this;
	var id = $stateParams.assessmentId;

	candidateRepository.getCandidate(id).then((data) => {
		ctrl.assessment = data;
		ctrl.resumeUrl = candidateRepository.resumeUrl(data.resumeFileId, data.resumeFileName);
		ctrl.assessmentUrl = candidateRepository.assessmentUrl(data.assessmentFileId, data.assessmentFileName);
	});

	candidateRepository.getQuestions(id).then((data) => {
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
		candidateRepository.saveQuestions(id, ctrl.questions).then(() => {
			$state.go("home");
			toastr.success('Successfully Saved!');
		}, () => {
			toastr.error(status, 'Error Saving');
		});
	};

	$scope.modalVisible = false;
	ctrl.showModal = () => {
		$scope.modalVisible = !$scope.modalVisible;
		candidateRepository.getAssessmentSummary(id).then((result) => {
			ctrl.summary = result;
		});
	};
}];