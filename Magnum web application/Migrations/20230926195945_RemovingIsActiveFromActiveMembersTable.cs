using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magnum_web_application.Migrations
{
    /// <inheritdoc />
    public partial class RemovingIsActiveFromActiveMembersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ActiveMembers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ActiveMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
