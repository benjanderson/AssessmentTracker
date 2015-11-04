module.exports = ["canidateRepository", function (canidateRepository) {
	var ctrl = this;
	canidateRepository.getOpenAssessments().then((data) => {
		ctrl.openAssessments = data;
	});
}];

