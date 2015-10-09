/// <binding Clean='clean' />

var gulp = require("gulp"),
	browserify = require("browserify"),
	project = require("./project.json"),
	shell = require("gulp-shell"),
	browserSync = require("browser-sync"),
	source = require("vinyl-source-stream"),
	concat = require("gulp-concat"),
	eslint = require("gulp-eslint"),
	gulpDebug = require("gulp-debug");

var config = {
	port: 14989,
	sitepath: "",
	paths: {
		webroot: "./" + project.webroot + "/",
		html: "Views/**/*.cshtml",
		siteJs: "./" + project.webroot + "/js/site.js",
		js: ["./" + project.webroot + "/**/*.js", "!./" + project.webroot + "/**/*.min.js"],
		cssRoot: "./" + project.webroot + "/css/",
		css: [
			"node_modules/bootstrap/dist/css/bootstrap.min.css",
			"node_modules/bootstrap/dist/css/bootstrap.theme.min.css"
		],
		siteCss: "./" + project.webroot + "/css/site.css"
	}
};

gulp.task("browserify", function () {
	browserify(config.paths.siteJs)
		.transform(reactify)
		.bundle()
		.pipe(source("bundle.min.js"))
		.pipe(gulp.dest(config.paths.webroot + "/js/"));
});

gulp.task('html-watch', browserSync.reload);
gulp.task('js-watch', ["browserify"], browserSync.reload);
gulp.task("css-watch", browserSync.reload);

gulp.task("serve", ["browserify", "css"], function () {
	browserSync({
		proxy: {
			target: "http://localhost:" + config.port,
			middleware: function (req, res, next) {
				res.setHeader('Access-Control-Allow-Origin', '*');
				next();
			}
		},
		baseDir: config.paths.webroot

	});
	gulp.watch(config.paths.js, ["js-watch"]);
	gulp.watch(config.paths.html, ["html-watch"]);
	gulp.watch(config.paths.siteCss, ["css-watch"]);
});

//gulp.task("iis-express", function() {
//	var baseAddress = "\"" + process.env.PROGRAMFILES + "\\IIS Express\\iisexpress\"";
//	var cmd = " /site:\"benjanderson\" /config:\"" + "C:\\Source\\Github\\benjanderson\\.vs\\config\\applicationhost.config" + "\"";
//	gulp.src("")
//		.pipe(shell([baseAddress + cmd]));
//});

gulp.task("css", function () {
	gulp.src(config.paths.css)
		.pipe(concat("bundle.min.css"))
		.pipe(gulp.dest(config.paths.cssRoot));
});

gulp.task("lint", function () {
	return gulp.src(config.paths.js)
		.pipe(eslint({ config: "./eslint.config.json" }))
		.pipe(eslint.format())
		.pipe(eslint.failOnError());
});

gulp.task("default", ["serve"]);
