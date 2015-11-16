namespace AssessmentTracker.Controllers
{
	using System;
	using System.Collections.Generic;
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
		private const int AssessmentIndex = 0;
		private const int ResumeIndex = 1;

		private readonly AssessmentDbContext assessmentContext;

		private string CurrentUserId => "homer.simpson";

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
						                     ContentDispositionHeaderValue.Parse(files[AssessmentIndex].ContentDisposition).FileName.Trim('"'),
					                     Contents = files[AssessmentIndex].ReadFile(),
                               ContentType = files[AssessmentIndex].ContentType
			};
			this.assessmentContext.Files.Add(assessmentFile);

			var resumeFile = new DbFile
				                 {
					                 FileName =
						                 ContentDispositionHeaderValue.Parse(files[ResumeIndex].ContentDisposition).FileName.Trim('"'),
					                 Contents = files[ResumeIndex].ReadFile(),
													 ContentType = files[ResumeIndex].ContentType
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
			
			var assessment = await this.assessmentContext.Assessments
				.Include(a => a.AssessmentDbFile)
				.Include(a => a.ResumeDbFile)
				.Include(a => a.Person)
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

			if (files.Count() == 2)
			{
				assessment.AssessmentDbFile.FileName =
					ContentDispositionHeaderValue.Parse(files[AssessmentIndex].ContentDisposition).FileName.Trim('"');
        assessment.AssessmentDbFile.Contents = files[AssessmentIndex].ReadFile();
				assessment.AssessmentDbFile.ContentType = files[AssessmentIndex].ContentType;

				assessment.ResumeDbFile.FileName = ContentDispositionHeaderValue.Parse(files[ResumeIndex].ContentDisposition).FileName.Trim('"');
				assessment.ResumeDbFile.Contents = files[ResumeIndex].ReadFile();
				assessment.ResumeDbFile.ContentType = files[ResumeIndex].ContentType;
			}

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
										},
								AssessmentFileId = canidate.AssessmentFileId,
								AssessmentFileName = canidate.AssessmentDbFile.FileName,
								ResumeFileId = canidate.ResumeFileId,
								ResumeFileName = canidate.ResumeDbFile.FileName
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

		[Route("questions")]
		[HttpGet]
		public async Task<IActionResult> GetQuestions(int assessmentId)
		{
			List<AssessmentQuestion> questions;
			var previousAssessment =
				await
				this.assessmentContext.PersonAssessments.Where(
					assess => assess.Person.Name == this.CurrentUserId && assess.AssessmentId == assessmentId)
					.Include(assess => assess.Answers)
					.ThenInclude(answer => answer.Question)
					.FirstOrDefaultAsync();


			questions =
					await
					this.assessmentContext.Questions.Select(
						question => new AssessmentQuestion { Id = question.Id, Text = question.Text }).ToListAsync();

			if (previousAssessment != null)
			{
				foreach (var answer in previousAssessment.Answers)
				{
					var targetQuestion = questions.FirstOrDefault(q => q.Id == answer.QuestionId);
					if (targetQuestion != null)
					{
						targetQuestion.Score = answer.Rating;
						targetQuestion.Comments = answer.Comments;
					}
				}
			}
			
			return this.Ok(questions);
		}

		[Route("questions")]
		[HttpPost]
		public async Task<IActionResult> PostQuestions([FromBody]AssessmentScore assessmentScore)
		{
			var person = await this.assessmentContext.Persons.FirstOrDefaultAsync(p => p.Name == this.CurrentUserId);
			if (person == null)
			{
				person = new Person { Name = this.CurrentUserId };
				this.assessmentContext.Persons.Add(person);
				await this.assessmentContext.SaveChangesAsync();
			}

			var previousAssessment =
				await
				this.assessmentContext.PersonAssessments.Where(
					assess => assess.Person.Name == this.CurrentUserId && assess.AssessmentId == assessmentScore.AssessmentId)
					.Include(assess => assess.Answers)
					.Include(assess => assess.Person)
					.FirstOrDefaultAsync();

			if (previousAssessment == null)
			{
				var newAssessment = new PersonAssessment { Person = person, AssessmentId = assessmentScore.AssessmentId };
				this.assessmentContext.PersonAssessments.Add(newAssessment);
				await this.assessmentContext.SaveChangesAsync();

				if (assessmentScore.Questions != null)
				{
					foreach (var score in assessmentScore.Questions)
					{
						if (score.Score.HasValue)
						{
							newAssessment.Answers.Add(
								new Answers
									{
										Rating = (int)score.Score,
										Comments = score.Comments,
										QuestionId = score.Id,
										PersonAssessmentId = newAssessment.Id
									});
						}
					}
				}
				
			}
			else
			{
				if (assessmentScore.Questions != null)
				{
					foreach (var score in assessmentScore.Questions)
					{
						if (score.Score.HasValue)
						{
							var targetQuestion = previousAssessment.Answers.FirstOrDefault(answer => answer.QuestionId == score.Id);
							if (targetQuestion != null)
							{
								targetQuestion.Rating = (int)score.Score;
								targetQuestion.Comments = score.Comments;
							}
							else
							{
								previousAssessment.Answers.Add(
									new Answers { Rating = (int)score.Score, Comments = score.Comments, QuestionId = score.Id });
							}
						}
					}
				}
			}

			await this.assessmentContext.SaveChangesAsync();
			return this.Ok();
		}

		[Route("files/{fileId:int}/{fileName}")]
		public async Task<IActionResult> GetFile(int fileId, string fileName)
		{
			var dbFile = await this.assessmentContext.Files.FirstOrDefaultAsync(file => file.Id == fileId);
			if (dbFile == null)
			{
				return this.HttpBadRequest("No such file exists");
			}
			return this.File(dbFile.Contents, dbFile.ContentType);
		}

		[Route("assessment/summary/")]
		public async Task<IActionResult> GetAssessmentSummary(int assessmentId)
		{
			var dbSummary =
				await
				this.assessmentContext.PersonAssessments.Where(assess => assess.AssessmentId == assessmentId)
					.Include(assess => assess.Person)
					.Include(assess => assess.Answers)
					.ThenInclude(answer => answer.Question )
					.ToListAsync();

			var summary =
				dbSummary.SelectMany(assess => assess.Answers)
					.GroupBy(question => question.QuestionId)
					.Select(
						question =>
						new
							{
								Text = question.First().Question.Text,
								Average = $"Value: {(float)question.Sum(answer => answer.Rating) / (question.Count() * 3):P2}.",
								Answers =
							question.Select(
								answer =>
								new { Rating = answer.Rating, Person = answer.PersonAssessment.Person.Name, Comment = answer.Comments })
							})
					.ToList();

			return this.Ok(summary);
		}
	}
}
