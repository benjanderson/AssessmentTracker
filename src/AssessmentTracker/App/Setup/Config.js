module.exports = function ($stateProvider, $urlRouterProvider, $locationProvider) {
	$locationProvider.html5Mode(true);
	$urlRouterProvider.otherwise('/home');

	$stateProvider
    .state('home', {
    	url: '/home',
    	templateUrl: 'home.html',
    	controller: "HomeController as Home"
    })
  .state('NewCanidate', {
  	url: '/NewCanidate',
  	templateUrl: 'NewCanidate.html',
  	controller: "NewCanidateController as NewCanidate"
  });
};