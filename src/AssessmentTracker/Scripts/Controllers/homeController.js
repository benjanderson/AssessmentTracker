module.exports = ["candidateRepository", function (candidateRepository) {
	var ctrl = this;
	candidateRepository.getOpenAssessments().then((data) => {
		ctrl.openAssessments = data;
	});
}];

