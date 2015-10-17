namespace AssessmentTracker.DataAccess
{
	using Microsoft.Data.Entity;

	public interface IAssessmentDbContext 
	{
		DbSet<Person> Persons { get; set; }

		DbSet<Assessment> Assessments { get; set; }

		DbSet<DbFile> Files { get; set; }

		int SaveChanges();
	}

	public sealed class AssessmentDbContext : DbContext, IAssessmentDbContext
	{
		public AssessmentDbContext()
		{
			this.Database.EnsureCreated();
		}

		public DbSet<Person> Persons { get; set; }

		public DbSet<Assessment> Assessments { get; set; }

		public DbSet<DbFile> Files { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}
