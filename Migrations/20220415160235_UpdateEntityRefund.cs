using Microsoft.EntityFrameworkCore.Migrations;

namespace BoaSaudeRefund.Migrations
{
    public partial class UpdateEntityRefund : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserAnalyst",
                table: "Refund",
                newName: "UserNameAnalyst");

            migrationBuilder.RenameColumn(
                name: "User",
                table: "Refund",
                newName: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserNameAnalyst",
                table: "Refund",
                newName: "UserAnalyst");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Refund",
                newName: "User");
        }
    }
}
