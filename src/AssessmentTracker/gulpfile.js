var gulp = require("gulp"),
	browserify = require("browserify"),
	gulpBrowserify = require("gulp-browserify"),
	project = require("./project.json"),
	browserSync = require("browser-sync"),
	source = require("vinyl-source-stream"),
	concat = require("gulp-concat"),
	eslint = require("gulp-eslint"),
	gulpDebug = require("gulp-debug"),
	less = require("gulp-less"),
	cssMin = require("gulp-cssmin"),
	minifyify = require("minifyify"),
	sourcemaps = require('gulp-sourcemaps'),
	babelify = require("babelify"),
	gulpIf = require("gulp-if");

var config = {
	release: process.env.NODE_ENV && process.env.NODE_ENV === 'Release',
	port: 50937,
	sitepath: "",
	paths: {
		webroot: "./" + project.webroot + "/",
		html: ["Views/**/*.cshtml",
		"./" + project.webroot + "/templates/**/*.html"],
		js: "Scripts/**/*.js",
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
	return gulp.src(config.paths.siteLess)
		.pipe(gulpIf(!config.release, sourcemaps.init()))
		.pipe(less())
		.pipe(gulpIf(!config.release, sourcemaps.write()))
		.on('error', swallowError)
		.pipe(cssMin())
		.pipe(gulp.dest("./" + project.webroot + "/css/"));
});

gulp.task("browserify", function () {
	return browserify({ debug: !config.release, entries: ["./Scripts/app.js"] })
		.transform("babelify", { presets: ["es2015"] })
		.plugin('minifyify', { map: config.release ? null : 'app.map.json', output: config.release ? null : "./" + project.webroot + "/js/app.map.json" })
		.bundle()
		.on('error', swallowError)
		.pipe(source("app.js"))
		.pipe(gulp.dest(config.paths.webroot + "/js/"));
});

gulp.task("fonts", function () {
	return gulp.src("node_modules/font-awesome/fonts/**.*")
		.pipe(gulp.dest(config.paths.webroot + "/fonts/"));
});

gulp.task("min", function() {

});

gulp.task("clean", function () {

});

gulp.task("default", ["clean", "min", "browserify", "css", "serve", "fonts"], function () {
	if (!config.release) {
		gulp.watch(config.paths.js, ["js-watch"]);
		gulp.watch(config.paths.html, ["html-watch"]);
		gulp.watch(config.paths.siteLess, ["css-watch"]);
	}
});
