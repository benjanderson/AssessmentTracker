namespace AssessmentTracker.Models
{
	using System.Collections.Generic;

	public class AssessmentScore
	{
		public int AssessmentId { get; set; }

		public List<AssessmentQuestion> Questions { get; set; }
	}
}