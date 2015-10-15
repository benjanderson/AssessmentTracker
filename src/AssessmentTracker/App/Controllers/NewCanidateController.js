module.exports = ["$http", function ($http) {
	var ctrl = this;
	var assessment;
	var resume;
	ctrl.canidate = {
		name: null,
		position: null,
		dateOfAssessment: new Date()
	};

	ctrl.positions = [
		{ value: 1, text: "Intern" }, { value: 2, text: "Software Engineer" }, { value: 3, text: "Senior Software Engineer" }, { value: 4, text: "Software Tester" }, { value: 5, text: "Other" }
	];

	ctrl.assessmentUpload = function (files) {
		assessment = files[0];
		console.log("assessment");
	}

	ctrl.resumeUpload = function (files) {
		resume = files[0];
		console.log("resume");
	}

	ctrl.save = function() {
		$http({
				method: "POST",
				url: "Home/Canidate",
				headers: { 'Content-Type': undefined },
				transformRequest: function(data) {
					var formData = new FormData();
					//need to convert our json object to a string version of json otherwise
					// the browser will do a 'toString()' on the object which will result 
					// in the value '[Object object]' on the server.
					formData.append("model", angular.toJson(data.model));
					//now add all of the assigned files
					for (var i = 0; i < data.files.length; i++) {
						//add each file to the form data and iteratively name them
						formData.append("file" + i, data.files[i]);
					}
					return formData;
				},
				data: { model: ctrl.canidate, files: [assessment, resume] }
			}).
			success(function(data, status, headers, config) {
				alert("success!");
			}).
			error(function(data, status, headers, config) {
				alert("failed!");
			});
	}
}];