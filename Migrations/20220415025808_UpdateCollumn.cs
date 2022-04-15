using Microsoft.EntityFrameworkCore.Migrations;

namespace BoaSaudeRefund.Migrations
{
    public partial class UpdateCollumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "Refund",
                newName: "NFeLink");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NFeLink",
                table: "Refund",
                newName: "Reason");
        }
    }
}
