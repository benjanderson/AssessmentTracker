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
	uglify = require("gulp-uglify"),
	gulpIf = require("gulp-if");

var config = {
	release: process.env.NODE_ENV && process.env.NODE_ENV !== 'Release',
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

function swallowError(error) {
	console.log(error.toString());
	this.emit('end');
}

gulp.task('html-watch', browserSync.reload);
gulp.task('js-watch', ["browserify"], browserSync.reload);
gulp.task("css-watch", ["css"], browserSync.reload);

gulp.task("serve", function () {
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
});

gulp.task("css", function () {
	gulp.src(config.paths.siteLess)
		.pipe(less())
		.on('error', swallowError)
		.pipe(gulpIf(config.release, cssMin()))
		.pipe(concat("bundle.min.css"))
		.pipe(gulp.dest("./" + project.webroot + "/css/"));
});

gulp.task("browserify", function () {
	gulp.src("App/app.js")
		.pipe(browserify({
			debug: !config.release
		}))
		.on('error', swallowError)
		.pipe(gulpIf(config.release, uglify()))
		.pipe(concat("bundle.min.js"))
		.pipe(gulp.dest(config.paths.webroot + "/js/"));
});


gulp.task("default", ["browserify", "css", "serve"], function() {
	gulp.watch(config.paths.js, ["js-watch"]);
	gulp.watch(config.paths.html, ["html-watch"]);
	gulp.watch(config.paths.siteLess, ["css-watch"]);
});
