var gulp = require("gulp"),
	browserify = require("gulp-browserify"),
	project = require("./project.json"),
	browserSync = require("browser-sync"),
	source = require("vinyl-source-stream"),
	concat = require("gulp-concat"),
	eslint = require("gulp-eslint"),
	gulpDebug = require("gulp-debug"),
	less = require("gulp-less"),
	cssMin = require("gulp-cssmin"),
	uglify = require("gulp-uglify");

var config = {
	port: 50937,
	sitepath: "",
	paths: {
		webroot: "./" + project.webroot + "/",
		html: ["Views/**/*.cshtml",
		"./" + project.webroot + "/templates/**/*.html"],
		js: "App/**/*.js",
		siteLess: "Content/site.less"
	}
};

gulp.task('html-watch', browserSync.reload);
gulp.task('js-watch', ["browserify"], browserSync.reload);
gulp.task("css-watch", ["css"], browserSync.reload);

gulp.task("serve", ["browserify", "css"], function () {
//	browserSync({
//		proxy: {
//			target: "http://localhost:" + config.port,
//			middleware: function (req, res, next) {
//				res.setHeader('Access-Control-Allow-Origin', '*');
//				next();
//			}
//		},
//		baseDir: config.paths.webroot
//
//	});
	gulp.watch(config.paths.js, ["js-watch"]);
	gulp.watch(config.paths.html, ["html-watch"]);
	gulp.watch(config.paths.siteLess, ["css-watch"]);
});

gulp.task("css", function () {
	gulp.src(config.paths.siteLess)
		.pipe(less())
		//.pipe(cssMin())
		.pipe(concat("bundle.min.css"))
		.pipe(gulp.dest("./" + project.webroot + "/css/"));
});

gulp.task("browserify", function () {
	gulp.src("App/app.js")
		.pipe(gulpDebug())
		.pipe(browserify())
//		.pipe(uglify())
		.pipe(concat("bundle.min.js"))
		.pipe(gulp.dest(config.paths.webroot + "/js/"));
});

//gulp.task("lint", function () {
//	return gulp.src(config.paths.js)
//		.pipe(eslint({ config: "./eslint.config.json" }))
//		.pipe(eslint.format())
//		.pipe(eslint.failOnError());
//});

gulp.task("default", ["serve"]);
