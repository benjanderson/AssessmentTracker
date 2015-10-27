using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.SqlServer.Metadata;

namespace AssessmentTracker.Migrations
{
    public partial class QuestionAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbFile",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    ContentType = table.Column<string>(isNullable: true),
                    Contents = table.Column<byte[]>(isNullable: true),
                    FileName = table.Column<string>(isNullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbFile", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    Name = table.Column<string>(isNullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    Text = table.Column<string>(isNullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "Assessment",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    Active = table.Column<bool>(isNullable: false),
                    AssessmentFileId = table.Column<int>(isNullable: false),
                    DateOfDeadline = table.Column<DateTime>(isNullable: false),
                    DateOfSubmission = table.Column<DateTime>(isNullable: false),
                    Notes = table.Column<string>(isNullable: true),
                    PersonId = table.Column<int>(isNullable: false),
                    Position = table.Column<int>(isNullable: false),
                    ResumeFileId = table.Column<int>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessment_DbFile_AssessmentFileId",
                        column: x => x.AssessmentFileId,
                        principalTable: "DbFile",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Assessment_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Assessment_DbFile_ResumeFileId",
                        column: x => x.ResumeFileId,
                        principalTable: "DbFile",
                        principalColumn: "Id");
                });
            migrationBuilder.CreateTable(
                name: "PersonAssessment",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    AssessmentId = table.Column<int>(isNullable: false),
                    PersonId = table.Column<int>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonAssessment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonAssessment_Assessment_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonAssessment_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                });
            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    Comments = table.Column<string>(isNullable: true),
                    PersonAssessmentId = table.Column<int>(isNullable: false),
                    QuestionId = table.Column<int>(isNullable: false),
                    Rating = table.Column<int>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_PersonAssessment_PersonAssessmentId",
                        column: x => x.PersonAssessmentId,
                        principalTable: "PersonAssessment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answers_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id");
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Answers");
            migrationBuilder.DropTable("PersonAssessment");
            migrationBuilder.DropTable("Question");
            migrationBuilder.DropTable("Assessment");
            migrationBuilder.DropTable("DbFile");
            migrationBuilder.DropTable("Person");
        }
    }
}
