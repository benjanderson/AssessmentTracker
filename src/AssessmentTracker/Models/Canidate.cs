using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessmentTracker.Models
{

	public class Candidate
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public DateTime DateOfAssessment { get; set; }

		public DateTime DateOfDeadline { get; set; }

		public Option Position { get; set; }

		public string Notes { get; set; }

		public bool Active { get; set; }

		public int? AssessmentFileId { get; set; }

		public string AssessmentFileName { get; set; }

		public int? ResumeFileId { get; set; }

		public string ResumeFileName { get; set; }
	}
}
