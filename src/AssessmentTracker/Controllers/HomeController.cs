namespace AssessmentTracker.Controllers
{
	using System;
	using System.IO;
	using System.Linq;

	using AssessmentTracker.DataAccess;
	using AssessmentTracker.Models;
	using AssessmentTracker.Services;

	using Microsoft.AspNet.Http;
	using Microsoft.AspNet.Mvc;
	using Microsoft.Net.Http.Headers;

	using Newtonsoft.Json;

	using File = AssessmentTracker.DataAccess.File;

	public class HomeController : Controller
	{
		private readonly AssessmentDbContext assessmentContext;

		public HomeController(AssessmentDbContext assessmentContext)
		{
			this.assessmentContext = assessmentContext;
		}

		public IActionResult Index()
		{
			return this.View();
		}

		[Route("Home/Canidate")]
		[HttpPost]
		public IActionResult PostCanidate()
		{
			var jsonCanidate = this.Request.Form["model"];
			if (string.IsNullOrWhiteSpace(jsonCanidate))
			{
				return this.HttpBadRequest();
			}

      var canidate = JsonConvert.DeserializeObject<Canidate>(jsonCanidate);
			var files = this.Request.Form.Files;

			if (canidate == null || files.Count() < 2)
			{
				return this.HttpBadRequest();
			}

			var assessmentFile = new File
				                     {
					                     FileName =
						                     ContentDispositionHeaderValue.Parse(files[0].ContentDisposition).FileName.Trim('"'),
					                     Contents = files[0].ReadFile()
				                     };
			this.assessmentContext.Files.Add(assessmentFile);

			var resumeFile = new File
				                 {
					                 FileName =
						                 ContentDispositionHeaderValue.Parse(files[1].ContentDisposition).FileName.Trim('"'),
					                 Contents = files[1].ReadFile()
				                 };
			this.assessmentContext.Files.Add(resumeFile);

			var person = new Person { Name = canidate.Name, };
			this.assessmentContext.Persons.Add(person);

			this.assessmentContext.SaveChanges();

			var assessment = new Assessment
				                 {
					                 Person = person,
					                 DateOfSubmission = canidate.DateOfAssessment,
					                 DateOfDeadline = canidate.DateOfDeadline,
					                 Notes = canidate.Notes,
					                 Position = (Positions)Convert.ToInt32(canidate.Position.Value),
					                 AssessmentFileId = assessmentFile.Id,
					                 ResumeFileId = resumeFile.Id,
					                 Active = true
				                 };
      this.assessmentContext.Assessments.Add(assessment);
			this.assessmentContext.SaveChanges();
			return this.Ok();
		}

		[Route("Home/OpenAssessments")]
		[HttpGet]
		public IActionResult GetOpenAssessments()
		{
			var previews =
				this.assessmentContext.Assessments.Where(assessment => assessment.Active)
					.Select(
						assessment =>
						new AssessmentPreview
							{
								AssessmentId = assessment.Id,
								DateDue = assessment.DateOfDeadline,
								DateSubmitted = assessment.DateOfSubmission,
								Name = assessment.Person.Name
							})
					.ToList();

			var openAssessments = new OpenAssessments { AssessmentPreviews = previews };
			return this.Ok(openAssessments);
		}
	}

}
