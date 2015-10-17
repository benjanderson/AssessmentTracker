namespace AssessmentTracker.Controllers
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;

	using AssessmentTracker.DataAccess;
	using AssessmentTracker.Models;
	using AssessmentTracker.Services;

	using Microsoft.AspNet.Http;
	using Microsoft.AspNet.Mvc;
	using Microsoft.Net.Http.Headers;
	using Microsoft.Data.Entity;
	
	using Newtonsoft.Json;

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

		[Route("assessment")]
		[HttpPost]
		public async Task<IActionResult> PostCanidate()
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

			var assessmentFile = new DbFile
				                     {
					                     FileName =
						                     ContentDispositionHeaderValue.Parse(files[0].ContentDisposition).FileName.Trim('"'),
					                     Contents = files[0].ReadFile(),
                               ContentType = files[0].ContentType
			};
			this.assessmentContext.Files.Add(assessmentFile);

			var resumeFile = new DbFile
				                 {
					                 FileName =
						                 ContentDispositionHeaderValue.Parse(files[1].ContentDisposition).FileName.Trim('"'),
					                 Contents = files[1].ReadFile(),
													 ContentType = files[1].ContentType
				                 };
			this.assessmentContext.Files.Add(resumeFile);

			var person = new Person { Name = canidate.Name, };
			this.assessmentContext.Persons.Add(person);

			await this.assessmentContext.SaveChangesAsync();

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
			await this.assessmentContext.SaveChangesAsync();
			return this.Ok();
		}

		[Route("assessment")]
		[HttpPut]
		public async Task<IActionResult> PutCanidate()
		{
			var jsonCanidate = this.Request.Form["model"];
			if (string.IsNullOrWhiteSpace(jsonCanidate))
			{
				return this.HttpBadRequest();
			}

			var canidate = JsonConvert.DeserializeObject<Canidate>(jsonCanidate);
			var files = this.Request.Form.Files;

			if (canidate == null)
			{
				return this.HttpBadRequest();
			}
			
			var assessment = await this.assessmentContext.Assessments.Include(a => a.Person)
				.FirstOrDefaultAsync(a => a.Id == canidate.Id);

			if (assessment == null)
			{
				return this.HttpBadRequest("No Assessment found with id " + canidate.Id);
			}

			assessment.Person.Name = canidate.Name;
			assessment.DateOfSubmission = canidate.DateOfAssessment;
			assessment.DateOfDeadline = canidate.DateOfDeadline;
			assessment.Notes = canidate.Notes;
			assessment.Position = (Positions)Convert.ToInt32(canidate.Position.Value);
			assessment.Active = canidate.Active;
			await this.assessmentContext.SaveChangesAsync();

			return this.Ok();
		}

		[Route("assessment")]
		[HttpGet]
		public async Task<IActionResult> GetCanidate(int assessmentId)
		{
			var assessment =
				await this.assessmentContext.Assessments.Where(a => a.Id == assessmentId)
					.Select(
						canidate =>
						new Canidate
							{
							  Id = assessmentId,
								DateOfAssessment = canidate.DateOfSubmission,
								DateOfDeadline = canidate.DateOfDeadline,
								Name = canidate.Person.Name,
								Notes = canidate.Notes,
								Active = canidate.Active,
								Position =
									new Option
										{
											Text = canidate.Position.ToString().ToSentenceCase(),
											Value = ((int)canidate.Position).ToString()
										}
							})
					.FirstOrDefaultAsync();

			return this.Json(assessment);
		}

		[Route("positions")]
		[HttpGet]
		public IActionResult GetPositions()
		{
			return
				this.Json(
					Enum.GetValues(typeof(Positions)).Cast<Positions>().Select(p => new { text = p.ToString().ToSentenceCase(), value = (int)p }));
		}

		[Route("openAssessments")]
		[HttpGet]
		public async Task<IActionResult> GetOpenAssessments()
		{
			var previews = await
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
					.ToListAsync();

			return this.Ok(previews);
		}
	}

}
