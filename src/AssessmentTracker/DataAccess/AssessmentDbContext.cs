namespace AssessmentTracker.DataAccess
{
	using Microsoft.Data.Entity;

	public interface IAssessmentDbContext 
	{
		DbSet<Person> Persons { get; set; }

		DbSet<Assessment> Assessments { get; set; }

		DbSet<File> Files { get; set; }

		int SaveChanges();
	}

	public class AssessmentDbContext : DbContext, IAssessmentDbContext
	{
		public DbSet<Person> Persons { get; set; }

		public DbSet<Assessment> Assessments { get; set; }

		public DbSet<File> Files { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}
