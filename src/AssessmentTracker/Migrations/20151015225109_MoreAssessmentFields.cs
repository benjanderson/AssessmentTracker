using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace AssessmentTracker.Migrations
{
    public partial class MoreAssessmentFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfDeadline",
                table: "Assessment",
                isNullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfSubmission",
                table: "Assessment",
                isNullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Assessment",
                isNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "DateOfDeadline", table: "Assessment");
            migrationBuilder.DropColumn(name: "DateOfSubmission", table: "Assessment");
            migrationBuilder.DropColumn(name: "Notes", table: "Assessment");
        }
    }
}
