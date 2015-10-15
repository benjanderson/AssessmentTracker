namespace AssessmentTracker.DataAccess
{
	public class File : IEntity
	{
		public int Id { get; set; }

		public string FileName { get; set; }

		public byte[] Contents { get; set; }
	}
}