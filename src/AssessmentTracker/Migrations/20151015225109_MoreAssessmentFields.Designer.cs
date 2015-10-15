using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using AssessmentTracker.DataAccess;
using Microsoft.Data.Entity.SqlServer.Metadata;

namespace AssessmentTracker.Migrations
{
    [DbContext(typeof(AssessmentDbContext))]
    partial class MoreAssessmentFields
    {
        public override string Id
        {
            get { return "20151015225109_MoreAssessmentFields"; }
        }

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta7-15540")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn);

            modelBuilder.Entity("AssessmentTracker.DataAccess.Assessment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AssessmentFileId");

                    b.Property<DateTime>("DateOfDeadline");

                    b.Property<DateTime>("DateOfSubmission");

                    b.Property<string>("Notes");

                    b.Property<int>("PersonId");

                    b.Property<int>("Position");

                    b.Property<int>("ResumeFileId");

                    b.Key("Id");
                });

            modelBuilder.Entity("AssessmentTracker.DataAccess.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Contents");

                    b.Property<string>("FileName");

                    b.Key("Id");
                });

            modelBuilder.Entity("AssessmentTracker.DataAccess.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Key("Id");
                });

            modelBuilder.Entity("AssessmentTracker.DataAccess.Assessment", b =>
                {
                    b.Reference("AssessmentTracker.DataAccess.File")
                        .InverseCollection()
                        .ForeignKey("AssessmentFileId");

                    b.Reference("AssessmentTracker.DataAccess.Person")
                        .InverseCollection()
                        .ForeignKey("PersonId");

                    b.Reference("AssessmentTracker.DataAccess.File")
                        .InverseCollection()
                        .ForeignKey("ResumeFileId");
                });
        }
    }
}
