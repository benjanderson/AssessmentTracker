namespace AssessmentTracker.DataAccess
{
	public class DbFile : IEntity
	{
		public int Id { get; set; }

		public string FileName { get; set; }

		public byte[] Contents { get; set; }

		public string ContentType { get; set; }
	}
}