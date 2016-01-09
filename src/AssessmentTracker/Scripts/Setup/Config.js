module.exports = ["$stateProvider", "$urlRouterProvider", "$locationProvider", function ($stateProvider, $urlRouterProvider, $locationProvider) {
	$urlRouterProvider.otherwise('/home');

	$stateProvider
		.state('home', {
			url: '/home',
			templateUrl: 'templates/home.html',
			controller: "homeController as home"
		})
		.state('newCandidate', {
			url: '/candidate/0',
			templateUrl: 'templates/candidate.html',
			controller: "candidateController as can"
		})
		.state('editCandidate', {
			url: '/candidate/:assessmentId',
			templateUrl: 'templates/candidate.html',
			controller: "candidateController as can"
		})
		.state("assessment", {
			url: "/assessment/:assessmentId",
			templateUrl: "templates/assessment.html",
			controller: "assessmentController as assess"
		});
}];