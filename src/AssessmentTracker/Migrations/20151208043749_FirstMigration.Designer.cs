using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using AssessmentTracker.DataAccess;

namespace AssessmentTracker.Migrations
{
    [DbContext(typeof(AssessmentDbContext))]
    [Migration("20151208043749_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AssessmentTracker.DataAccess.Answers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comments");

                    b.Property<int>("PersonAssessmentId");

                    b.Property<int>("QuestionId");

                    b.Property<int>("Rating");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("AssessmentTracker.DataAccess.Assessment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("AssessmentFileId");

                    b.Property<DateTime>("DateOfDeadline");

                    b.Property<DateTime>("DateOfSubmission");

                    b.Property<string>("Notes");

                    b.Property<int>("PersonId");

                    b.Property<int>("Position");

                    b.Property<int>("ResumeFileId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("AssessmentTracker.DataAccess.DbFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentType");

                    b.Property<byte[]>("Contents");

                    b.Property<string>("FileName");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("AssessmentTracker.DataAccess.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("AssessmentTracker.DataAccess.PersonAssessment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AssessmentId");

                    b.Property<int>("PersonId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("AssessmentTracker.DataAccess.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Text");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("AssessmentTracker.DataAccess.Answers", b =>
                {
                    b.HasOne("AssessmentTracker.DataAccess.PersonAssessment")
                        .WithMany()
                        .HasForeignKey("PersonAssessmentId");

                    b.HasOne("AssessmentTracker.DataAccess.Question")
                        .WithMany()
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("AssessmentTracker.DataAccess.Assessment", b =>
                {
                    b.HasOne("AssessmentTracker.DataAccess.DbFile")
                        .WithMany()
                        .HasForeignKey("AssessmentFileId");

                    b.HasOne("AssessmentTracker.DataAccess.Person")
                        .WithMany()
                        .HasForeignKey("PersonId");

                    b.HasOne("AssessmentTracker.DataAccess.DbFile")
                        .WithMany()
                        .HasForeignKey("ResumeFileId");
                });

            modelBuilder.Entity("AssessmentTracker.DataAccess.PersonAssessment", b =>
                {
                    b.HasOne("AssessmentTracker.DataAccess.Assessment")
                        .WithMany()
                        .HasForeignKey("AssessmentId");

                    b.HasOne("AssessmentTracker.DataAccess.Person")
                        .WithMany()
                        .HasForeignKey("PersonId");
                });
        }
    }
}
