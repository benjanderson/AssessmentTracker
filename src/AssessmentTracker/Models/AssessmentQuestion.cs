namespace AssessmentTracker.Models
{
	public class AssessmentQuestion
	{
		public string Text { get; set; }

		public int Id { get; set; }

		public float? Score { get; set; }

		public string Comments { get; set; }
	}
}