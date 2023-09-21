using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magnum_web_application.Migrations
{
    /// <inheritdoc />
    public partial class AddingMonthlyToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fees_TrainingSessions_TrainingSessionId",
                table: "Fees");

            migrationBuilder.RenameColumn(
                name: "TrainingSessionId",
                table: "Fees",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Fees_TrainingSessionId",
                table: "Fees",
                newName: "IX_Fees_MemberId");

            migrationBuilder.AddColumn<int>(
                name: "MonthlySessions",
                table: "TrainingSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Fees_Members_MemberId",
                table: "Fees",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fees_Members_MemberId",
                table: "Fees");

            migrationBuilder.DropColumn(
                name: "MonthlySessions",
                table: "TrainingSessions");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "Fees",
                newName: "TrainingSessionId");

            migrationBuilder.RenameIndex(
                name: "IX_Fees_MemberId",
                table: "Fees",
                newName: "IX_Fees_TrainingSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fees_TrainingSessions_TrainingSessionId",
                table: "Fees",
                column: "TrainingSessionId",
                principalTable: "TrainingSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
