using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magnum_web_application.Migrations
{
    /// <inheritdoc />
    public partial class AddingDateToTrainingSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SessionDate",
                table: "TrainingSessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionDate",
                table: "TrainingSessions");
        }
    }
}
