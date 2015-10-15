namespace AssessmentTracker.Models
{
	using Microsoft.AspNet.Identity.EntityFramework;
	using Microsoft.Data.Entity;

	public class AssessmentDbContext : DbContext
	{
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}

	public class Person
	{
		public int Id { get; set; }

		public string Name { get; set; }


	}

	public class Assessment
	{
		public int Id { get; set; }
	}
}
