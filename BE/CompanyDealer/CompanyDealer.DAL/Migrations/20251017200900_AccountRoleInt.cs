using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AccountRoleInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Accounts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Accounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
