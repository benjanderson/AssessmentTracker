using System.Collections.Generic;
using System.ComponentModel;

namespace AssessmentTracker.DataAccess
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Assessment : IEntity
	{
		public Assessment()
		{
			this.PersonAssessments = new List<PersonAssessment>();
		}

		public int Id { get; set; }

		public DateTime DateOfSubmission { get; set; }

		public DateTime DateOfDeadline { get; set; }

		public string Notes { get; set; }

		public int PersonId { get; set; }

		public virtual Person Person { get; set; }

		public Positions Position { get; set; }

		public int AssessmentFileId { get; set; }

		[ForeignKey("AssessmentFileId")]
		public virtual DbFile AssessmentDbFile { get; set; }

		public int ResumeFileId { get; set; }

		[ForeignKey("ResumeFileId")]
		public virtual DbFile ResumeDbFile { get; set; }

		[DefaultValue(true)]
		public bool Active { get; set; }

		public ICollection<PersonAssessment> PersonAssessments { get; set; }
  }
}