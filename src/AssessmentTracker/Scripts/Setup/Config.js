module.exports = ["$stateProvider", "$urlRouterProvider", "$locationProvider", function ($stateProvider, $urlRouterProvider, $locationProvider) {
	$urlRouterProvider.otherwise('/home');

	$stateProvider
		.state('home', {
			url: '/home',
			templateUrl: 'templates/home.html',
			controller: "homeController as home"
		})
		.state('newCanidate', {
			url: '/canidate/0',
			templateUrl: 'templates/canidate.html',
			controller: "canidateController as can"
		})
		.state('editCanidate', {
			url: '/canidate/:assessmentId',
			templateUrl: 'templates/canidate.html',
			controller: "canidateController as can"
		})
		.state("assessment", {
			url: "/assessment/:assessmentId",
			templateUrl: "templates/assessment.html",
			controller: "assessmentController as assess"
		});
}];