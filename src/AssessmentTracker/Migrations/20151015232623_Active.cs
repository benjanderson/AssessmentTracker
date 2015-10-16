using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace AssessmentTracker.Migrations
{
    public partial class Active : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Assessment",
                isNullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Active", table: "Assessment");
        }
    }
}
