module.exports = ["$stateProvider", "$urlRouterProvider", "$locationProvider", function ($stateProvider, $urlRouterProvider, $locationProvider) {
	$urlRouterProvider.otherwise('/home');

	$stateProvider
		.state('home', {
			url: '/home',
			templateUrl: 'templates/home.html',
			controller: "HomeController as Home"
		})
		.state('newCanidate', {
			url: '/canidate/0',
			templateUrl: 'templates/canidate.html',
			controller: "canidateController as canidate"
		})
		.state('editCanidate', {
			url: '/canidate/:canidateId',
			templateUrl: 'templates/canidate.html',
			controller: "canidateController as canidate"
		});
}];