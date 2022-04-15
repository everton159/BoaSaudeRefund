using Microsoft.EntityFrameworkCore.Migrations;

namespace BoaSaudeRefund.Migrations
{
    public partial class AddCNPJProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CNPJProvider",
                table: "Refund",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CNPJProvider",
                table: "Refund");
        }
    }
}
