namespace AssessmentTracker.DataAccess
{
	public class Question : IEntity
	{
		public int Id { get; set; }

		public string Text { get; set; }
	}
}