using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_and_DataBase.Migrations
{
    public partial class UserTransactionRelationShip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserName",
                table: "Transactions",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_UserName",
                table: "Transactions",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_UserName",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_UserName",
                table: "Transactions");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
