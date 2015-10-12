module.exports = ["$scope", function ($scope) {
	this.positions = [
		{ value: 1, text: "Intern" }, { value: 2, text: "Software Engineer" }, { value: 3, text: "Senior Software Engineer" }, { value: 4, text: "Software Tester" }, { value: 5, text: "Other" }
	];
	this.canidate = {
		name: null,//"Ben Anderson",
		position: this.positions[2]
	};
}];