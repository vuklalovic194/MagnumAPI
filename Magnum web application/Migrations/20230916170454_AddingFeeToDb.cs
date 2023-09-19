using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magnum_web_application.Migrations
{
    /// <inheritdoc />
    public partial class AddingFeeToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Fees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TrainingSessionId",
                table: "Fees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fees_TrainingSessionId",
                table: "Fees",
                column: "TrainingSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fees_TrainingSessions_TrainingSessionId",
                table: "Fees",
                column: "TrainingSessionId",
                principalTable: "TrainingSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fees_TrainingSessions_TrainingSessionId",
                table: "Fees");

            migrationBuilder.DropIndex(
                name: "IX_Fees_TrainingSessionId",
                table: "Fees");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Fees");

            migrationBuilder.DropColumn(
                name: "TrainingSessionId",
                table: "Fees");
        }
    }
}
