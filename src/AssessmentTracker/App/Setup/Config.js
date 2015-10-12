module.exports = ["$stateProvider", "$urlRouterProvider", "$locationProvider", function ($stateProvider, $urlRouterProvider, $locationProvider) {
	$urlRouterProvider.otherwise('/home');

	$stateProvider
    .state('home', {
    	url: '/home',
    	templateUrl: 'templates/home.html',
    	controller: "HomeController as Home"
    })
  .state('NewCanidate', {
  	url: '/NewCanidate',
  	templateUrl: 'templates/newCanidate.html',
  	controller: "NewCanidateController as NewCanidate"
  });
}];