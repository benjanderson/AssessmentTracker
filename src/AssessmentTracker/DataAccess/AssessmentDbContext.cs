namespace AssessmentTracker.DataAccess
{
	using Microsoft.Data.Entity;

	public interface IAssessmentDbContext 
	{
		DbSet<Person> Persons { get; set; }

		DbSet<Assessment> Assessments { get; set; }

		DbSet<DbFile> Files { get; set; }

		DbSet<PersonAssessment> PersonAssessments { get; set; }

		DbSet<Question> Questions { get; set; }

		DbSet<Answers> Answers { get; set; }

		int SaveChanges();
	}

	public sealed class AssessmentDbContext : DbContext, IAssessmentDbContext
	{
		public DbSet<Person> Persons { get; set; }

		public DbSet<Assessment> Assessments { get; set; }

		public DbSet<DbFile> Files { get; set; }

		public DbSet<PersonAssessment> PersonAssessments { get; set; }

		public DbSet<Question> Questions { get; set; }

		public DbSet<Answers> Answers { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			this.Database.Migrate();
		}
	}
}
