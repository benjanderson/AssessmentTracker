using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssessmentTracker.Models
{

	public class Canidate
	{
		public string Name { get; set; }

		public DateTime DateOfAssessment { get; set; }

		public DateTime DateOfDeadline { get; set; }

		public Option Position { get; set; }

		public string Notes { get; set; }
	}
}
