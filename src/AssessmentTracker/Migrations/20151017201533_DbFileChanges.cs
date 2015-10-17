using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.SqlServer.Metadata;

namespace AssessmentTracker.Migrations
{
    public partial class DbFileChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Assessment_File_AssessmentFileId", table: "Assessment");
            migrationBuilder.DropForeignKey(name: "FK_Assessment_File_ResumeFileId", table: "Assessment");
            migrationBuilder.DropTable("File");
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
            migrationBuilder.AddColumn<int>(
                name: "AssessmentDbFileId",
                table: "Assessment",
                isNullable: true);
            migrationBuilder.AddColumn<int>(
                name: "ResumeDbFileId",
                table: "Assessment",
                isNullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_Assessment_DbFile_AssessmentDbFileId",
                table: "Assessment",
                column: "AssessmentDbFileId",
                principalTable: "DbFile",
                principalColumn: "Id");
            migrationBuilder.AddForeignKey(
                name: "FK_Assessment_DbFile_ResumeDbFileId",
                table: "Assessment",
                column: "ResumeDbFileId",
                principalTable: "DbFile",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Assessment_DbFile_AssessmentDbFileId", table: "Assessment");
            migrationBuilder.DropForeignKey(name: "FK_Assessment_DbFile_ResumeDbFileId", table: "Assessment");
            migrationBuilder.DropColumn(name: "AssessmentDbFileId", table: "Assessment");
            migrationBuilder.DropColumn(name: "ResumeDbFileId", table: "Assessment");
            migrationBuilder.DropTable("DbFile");
            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<int>(isNullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerIdentityStrategy.IdentityColumn),
                    Contents = table.Column<byte[]>(isNullable: true),
                    FileName = table.Column<string>(isNullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                });
            migrationBuilder.AddForeignKey(
                name: "FK_Assessment_File_AssessmentFileId",
                table: "Assessment",
                column: "AssessmentFileId",
                principalTable: "File",
                principalColumn: "Id");
            migrationBuilder.AddForeignKey(
                name: "FK_Assessment_File_ResumeFileId",
                table: "Assessment",
                column: "ResumeFileId",
                principalTable: "File",
                principalColumn: "Id");
        }
    }
}
