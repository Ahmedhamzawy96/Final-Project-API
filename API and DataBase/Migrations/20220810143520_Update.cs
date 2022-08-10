using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_and_DataBase.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExportReciepts_Cars_CarBuyID",
                table: "ExportReciepts");

            migrationBuilder.DropForeignKey(
                name: "FK_ExportReciepts_Cars_CarSellID",
                table: "ExportReciepts");

            migrationBuilder.DropIndex(
                name: "IX_ExportReciepts_CarBuyID",
                table: "ExportReciepts");

            migrationBuilder.DropColumn(
                name: "CarBuyID",
                table: "ExportReciepts");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Suppliers",
                newName: "Account");

            migrationBuilder.RenameColumn(
                name: "CarSellID",
                table: "ExportReciepts",
                newName: "CarID");

            migrationBuilder.RenameIndex(
                name: "IX_ExportReciepts_CarSellID",
                table: "ExportReciepts",
                newName: "IX_ExportReciepts_CarID");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Customers",
                newName: "Account");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Cars",
                newName: "Account");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportReciepts_Cars_CarID",
                table: "ExportReciepts",
                column: "CarID",
                principalTable: "Cars",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExportReciepts_Cars_CarID",
                table: "ExportReciepts");

            migrationBuilder.RenameColumn(
                name: "Account",
                table: "Suppliers",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "CarID",
                table: "ExportReciepts",
                newName: "CarSellID");

            migrationBuilder.RenameIndex(
                name: "IX_ExportReciepts_CarID",
                table: "ExportReciepts",
                newName: "IX_ExportReciepts_CarSellID");

            migrationBuilder.RenameColumn(
                name: "Account",
                table: "Customers",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Account",
                table: "Cars",
                newName: "Amount");

            migrationBuilder.AlterColumn<decimal>(
                name: "Type",
                table: "Users",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CarBuyID",
                table: "ExportReciepts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExportReciepts_CarBuyID",
                table: "ExportReciepts",
                column: "CarBuyID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportReciepts_Cars_CarBuyID",
                table: "ExportReciepts",
                column: "CarBuyID",
                principalTable: "Cars",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportReciepts_Cars_CarSellID",
                table: "ExportReciepts",
                column: "CarSellID",
                principalTable: "Cars",
                principalColumn: "ID");
        }
    }
}
