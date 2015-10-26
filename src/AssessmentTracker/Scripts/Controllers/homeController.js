module.exports = ["$http", "canidateRepository", function ($http, canidateRepository) {
	var ctrl = this;
	canidateRepository.getOpenAssessments().then((data) => {
		ctrl.openAssessments = data;
	});
}];

