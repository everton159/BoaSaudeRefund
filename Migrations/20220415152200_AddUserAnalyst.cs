using Microsoft.EntityFrameworkCore.Migrations;

namespace BoaSaudeRefund.Migrations
{
    public partial class AddUserAnalyst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Refund",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Refund",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserAnalyst",
                table: "Refund",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Refund");

            migrationBuilder.DropColumn(
                name: "UserAnalyst",
                table: "Refund");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Refund",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
