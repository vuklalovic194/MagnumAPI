using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magnum_web_application.Migrations
{
    /// <inheritdoc />
    public partial class AddingRankToMemberTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rank",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Members");
        }
    }
}
