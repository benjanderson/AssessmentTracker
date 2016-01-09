using System.Globalization;

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

    using Microsoft.AspNet.Authorization;
    using Microsoft.AspNet.Http;
    using Microsoft.AspNet.Mvc;
    using Microsoft.Net.Http.Headers;
    using Microsoft.Data.Entity;

    using Newtonsoft.Json;

    [Authorize]
    public class HomeController : Controller
    {
        private const int AssessmentIndex = 0;
        private const int ResumeIndex = 1;

        private readonly AssessmentDbContext assessmentContext;

        private string CurrentUserId => this.User.Identity.Name;

        public HomeController(AssessmentDbContext assessmentContext)
        {
            this.assessmentContext = assessmentContext;
        }

        [Route("Home/Index")]
        [Route("")]
        public IActionResult Index()
        {
            return this.View();
        }

        [Route("Home/Error")]
        [AllowAnonymous]
        public IActionResult Error(string message)
        {
            this.ViewBag.Message = message;
            return this.View();
        }

        [Route("assessment")]
        [HttpPost]
        public async Task<IActionResult> PostCandidate()
        {
            var jsonCandidate = this.Request.Form["model"];
            if (string.IsNullOrWhiteSpace(jsonCandidate))
            {
                return this.HttpBadRequest("Invalid Candidate Object");
            }

            var candidate = JsonConvert.DeserializeObject<Candidate>(jsonCandidate);
            if (candidate == null)
            {
                return this.HttpBadRequest("Invalid Candidate Object");
            }

            var files = this.Request.Form.Files;

            DbFile assessmentFile = null;
            DbFile resumeFile = null;
            if (files.Count() == 2)
            {
                assessmentFile = new DbFile
                {
                    FileName =
                        ContentDispositionHeaderValue.Parse(files[AssessmentIndex].ContentDisposition)
                            .FileName.Trim('"'),
                    Contents = files[AssessmentIndex].ReadFile(),
                    ContentType = files[AssessmentIndex].ContentType
                };
                this.assessmentContext.Files.Add(assessmentFile);

                resumeFile = new DbFile
                {
                    FileName =
                        ContentDispositionHeaderValue.Parse(files[ResumeIndex].ContentDisposition).FileName.Trim('"'),
                    Contents = files[ResumeIndex].ReadFile(),
                    ContentType = files[ResumeIndex].ContentType
                };
                this.assessmentContext.Files.Add(resumeFile);
            }

            var person = new Person { Name = candidate.Name, };
            this.assessmentContext.Persons.Add(person);

            await this.assessmentContext.SaveChangesAsync();

            var assessment = new Assessment
            {
                Person = person,
                DateOfSubmission = candidate.DateOfAssessment,
                DateOfDeadline = candidate.DateOfDeadline,
                Notes = candidate.Notes,
                Position = (Positions)Convert.ToInt32(candidate.Position.Value),
                AssessmentFileId = assessmentFile?.Id,
                ResumeFileId = resumeFile?.Id,
                Active = true
            };
            this.assessmentContext.Assessments.Add(assessment);
            await this.assessmentContext.SaveChangesAsync();
            return this.Ok();
        }

        [Route("assessment")]
        [HttpPut]
        public async Task<IActionResult> PutCandidate()
        {
            var jsonCandidate = this.Request.Form["model"];
            if (string.IsNullOrWhiteSpace(jsonCandidate))
            {
                return this.HttpBadRequest("Invalid Candidate Object");
            }

            var candidate = JsonConvert.DeserializeObject<Candidate>(jsonCandidate);
            if (candidate == null)
            {
                return this.HttpBadRequest("Invalid Candidate Object");
            }

            var files = this.Request.Form.Files;

            var assessment = await this.assessmentContext.Assessments
                .Include(a => a.AssessmentDbFile)
                .Include(a => a.ResumeDbFile)
                .Include(a => a.Person)
                .FirstOrDefaultAsync(a => a.Id == candidate.Id);

            if (assessment == null)
            {
                return this.HttpBadRequest("No Assessment found with id " + candidate.Id);
            }

            assessment.Person.Name = candidate.Name;
            assessment.DateOfSubmission = candidate.DateOfAssessment;
            assessment.DateOfDeadline = candidate.DateOfDeadline;
            assessment.Notes = candidate.Notes;
            assessment.Position = (Positions)Convert.ToInt32(candidate.Position.Value);
            assessment.Active = candidate.Active;

            if (files.Count() == 2)
            {
                var assessmentFile = new
                {
                    fileName = ContentDispositionHeaderValue.Parse(files[AssessmentIndex].ContentDisposition).FileName.Trim('"'),
                    Contents = files[AssessmentIndex].ReadFile(),
                    files[AssessmentIndex].ContentType
                };

                if (assessment.AssessmentDbFile != null)
                {
                    assessment.AssessmentDbFile.FileName = assessmentFile.fileName;
                    assessment.AssessmentDbFile.Contents = assessmentFile.Contents;
                    assessment.AssessmentDbFile.ContentType = assessmentFile.ContentType;
                }
                else
                {
                    assessment.AssessmentDbFile = new DbFile
                    {
                        FileName = assessmentFile.fileName,
                        Contents = assessmentFile.Contents,
                        ContentType = assessmentFile.ContentType
                    };
                }

                var resumeFile = new 
                {
                    fileName = ContentDispositionHeaderValue.Parse(files[ResumeIndex].ContentDisposition).FileName.Trim('"'),
                    Contents = files[ResumeIndex].ReadFile(),
                    files[ResumeIndex].ContentType
                };

                if (assessment.ResumeDbFile != null)
                {
                    assessment.ResumeDbFile.FileName = resumeFile.fileName;
                    assessment.ResumeDbFile.Contents = resumeFile.Contents;
                    assessment.ResumeDbFile.ContentType = resumeFile.ContentType;
                }
                else
                {
                    assessment.ResumeDbFile = new DbFile
                    {
                        FileName = resumeFile.fileName,
                        Contents = resumeFile.Contents,
                        ContentType = resumeFile.ContentType
                    };
                }
            }

            await this.assessmentContext.SaveChangesAsync();

            return this.Ok();
        }

        [Route("assessment")]
        [HttpGet]
        public async Task<IActionResult> GetCandidate(int assessmentId)
        {
            var assessment =
                await this.assessmentContext.Assessments.Where(a => a.Id == assessmentId)
                    .Select(
                        candidate =>
                        new Candidate
                        {
                            Id = assessmentId,
                            DateOfAssessment = candidate.DateOfSubmission,
                            DateOfDeadline = candidate.DateOfDeadline,
                            Name = candidate.Person.Name,
                            Notes = candidate.Notes,
                            Active = candidate.Active,
                            Position =
                                    new Option
                                    {
                                        Text = candidate.Position.ToString().ToSentenceCase(),
                                        Value = ((int)candidate.Position).ToString(CultureInfo.InvariantCulture)
                                    }
                        })
                    .FirstOrDefaultAsync();

            if (assessment == null)
            {
                return this.HttpNotFound("Assessment not found");
            }

            var files =
                await this.assessmentContext.Assessments.Where(a => a.Id == assessmentId)
                    .Select(
                        candidate =>
                        new 
                        {
                            candidate.AssessmentFileId,
                            AssessmentFileName = candidate.AssessmentDbFile.FileName,
                            candidate.ResumeFileId,
                            ResumeFileName = candidate.ResumeDbFile.FileName
                        })
                    .FirstOrDefaultAsync();

            if (files != null)
            {
                assessment.AssessmentFileId = files.AssessmentFileId;
                assessment.AssessmentFileName = files.AssessmentFileName;
                assessment.ResumeFileId = files.ResumeFileId;
                assessment.ResumeFileName = files.ResumeFileName;
            }

            return this.Json(assessment);
        }

        [Route("positions")]
        [HttpGet]
        public IActionResult GetPositions()
        {
            return
                this.Json(
                    Enum.GetValues(typeof(Positions)).Cast<Positions>().Select(p => new { text = p.ToString().ToSentenceCase(), value = (float)p }));
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
                                    Rating = (float)score.Score,
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
                                targetQuestion.Rating = (float)score.Score;
                                targetQuestion.Comments = score.Comments;
                            }
                            else
                            {
                                previousAssessment.Answers.Add(
                                    new Answers { Rating = (float)score.Score, Comments = score.Comments, QuestionId = score.Id });
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
                    .ThenInclude(answer => answer.Question)
                    .ToListAsync();

            var questions =
                dbSummary.SelectMany(assess => assess.Answers)
                    .GroupBy(question => question.QuestionId)
                    .Select(
                        question =>
                        new
                        {
                            Text = question.First().Question.Text,
                            Average = question.Average(a => a.Rating),
                            Answers =
                            question.Select(
                                answer =>
                                new { Rating = answer.Rating, Person = answer.PersonAssessment.Person.Name, Comment = answer.Comments })
                        })
                    .ToList();

            return this.Ok(new
            {
                Questions = questions,
                TotalAverage = $"{questions.Average(q => q.Average / 2):P}"
            });
        }
    }
}
