using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_and_DataBase.Migrations
{
    public partial class ExportRecieptAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentAccount",
                table: "ExportReciepts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousAccount",
                table: "ExportReciepts",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentAccount",
                table: "ExportReciepts");

            migrationBuilder.DropColumn(
                name: "PreviousAccount",
                table: "ExportReciepts");
        }
    }
}
