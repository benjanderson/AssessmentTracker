var toastr = require("toastr");
module.exports = ["$http", "$stateParams", "$state", "canidateRepository", function ($http, $stateParams, $state, canidateRepository) {
		var ctrl = this;

		canidateRepository.getCanidate($stateParams.assessmentId).then((data) => {
			ctrl.assessment = data;
		});
		
	}
];