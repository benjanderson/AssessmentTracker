using System.ComponentModel.DataAnnotations.Schema;

namespace AssessmentTracker.DataAccess
{
	public class Answers : IEntity
	{
		public int Id { get; set; }

		public int QuestionId { get; set; }

		[ForeignKey("QuestionId")]
		public virtual Question Question { get; set; }

		public int PersonAssessmentId { get; set; }

		[ForeignKey("PersonAssessmentId")]
		public PersonAssessment PersonAssessment { get; set; }

		public float Rating { get; set; }

		public string Comments { get; set; }
	}
}