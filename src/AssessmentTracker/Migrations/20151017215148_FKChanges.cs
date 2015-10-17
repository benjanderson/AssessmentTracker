using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace AssessmentTracker.Migrations
{
    public partial class FKChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Assessment_DbFile_AssessmentDbFileId", table: "Assessment");
            migrationBuilder.DropForeignKey(name: "FK_Assessment_DbFile_ResumeDbFileId", table: "Assessment");
            migrationBuilder.DropColumn(name: "AssessmentDbFileId", table: "Assessment");
            migrationBuilder.DropColumn(name: "ResumeDbFileId", table: "Assessment");
            migrationBuilder.AddForeignKey(
                name: "FK_Assessment_DbFile_AssessmentFileId",
                table: "Assessment",
                column: "AssessmentFileId",
                principalTable: "DbFile",
                principalColumn: "Id");
            migrationBuilder.AddForeignKey(
                name: "FK_Assessment_DbFile_ResumeFileId",
                table: "Assessment",
                column: "ResumeFileId",
                principalTable: "DbFile",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Assessment_DbFile_AssessmentFileId", table: "Assessment");
            migrationBuilder.DropForeignKey(name: "FK_Assessment_DbFile_ResumeFileId", table: "Assessment");
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
    }
}
