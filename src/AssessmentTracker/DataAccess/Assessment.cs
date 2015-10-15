namespace AssessmentTracker.DataAccess
{
	using System;

	public class Assessment : IEntity
	{
		public int Id { get; set; }

		public DateTime DateOfSubmission { get; set; }

		public DateTime DateOfDeadline { get; set; }

		public string Notes { get; set; }

		public int PersonId { get; set; }

		public virtual Person Person { get; set; }

		public Positions Position { get; set; }

		public int AssessmentFileId { get; set; }

		public virtual File AssessmentFile { get; set; }

		public int ResumeFileId { get; set; }

		public virtual File ResumeFile { get; set; }
	}
}