using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magnum_web_application.Migrations
{
    /// <inheritdoc />
    public partial class AddingPublicVipToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VIP",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VIP",
                table: "Members");
        }
    }
}
