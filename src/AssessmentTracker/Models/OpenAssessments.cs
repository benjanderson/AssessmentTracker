using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessmentTracker.Models
{
	public class AssessmentPreview
	{
		public int AssessmentId { get; set; }

		public string Name { get; set; }

		public DateTime DateSubmitted { get; set; }

		public DateTime DateDue { get; set; }
	}
}
