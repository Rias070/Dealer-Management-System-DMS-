using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UserPass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Accounts");
        }
    }
}
