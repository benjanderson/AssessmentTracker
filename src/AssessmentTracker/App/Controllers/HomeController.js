module.exports = ["$http", function ($http) {
	var ctrl = this;

	$http.get("/home/openAssessments").then(function openAssessmentResponse(response) {
		ctrl.openAssessments = response.data;
	});
	
}];

