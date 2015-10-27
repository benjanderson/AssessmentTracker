using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssessmentTracker.DataAccess
{
	public class PersonAssessment : IEntity
	{
		public int Id { get; set; }

		public int AssessmentId { get; set; }

		[ForeignKey("AssessmentId")]
		public Assessment Assessment { get; set; }

		public ICollection<Answers> Answers { get; set; }

		public int PersonId { get; set; }

		[ForeignKey("PersonId")]
		public Person Person { get; set; }
	}
}