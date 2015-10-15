using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessmentTracker.Controllers
{
	using Microsoft.AspNet.Mvc;

	using Newtonsoft.Json.Linq;

	public class HomeController : Controller
	{
		public HomeController()
		{
		}

		public IActionResult Index()
		{
			return View();
		}

		[Route("Home/Canidate")]
		[HttpPost]
		public IActionResult PostCanidate()
		{
			var canidate = Newtonsoft.Json.JsonConvert.DeserializeObject<Canidate>(this.Request.Form["model"]);
			var files = this.Request.Form.Files;

			return Ok();
		}
	}

	public class Canidate
	{
		public string Name { get; set; }

		public DateTime DateOfAssessment { get; set; }

		public DateTime DateOfDeadline { get; set; }

		public Option Position { get; set; }

		public string Notes { get; set; }
  }

	public class Option
	{
		public string Text { get; set; }

		public string Value { get; set; }
	}
}
