var $ = jQuery = require('jquery');
require("bootstrap");
var angular = require("angular");
var app = angular.module("assessment", [require("angular-ui-router"), require("angular-sanitize")]);
require("./Services/");
require("./Setup/");
require("./Directives/");
require("./Controllers/");
