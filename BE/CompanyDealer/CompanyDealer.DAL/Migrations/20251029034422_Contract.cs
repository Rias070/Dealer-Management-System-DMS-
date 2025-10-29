using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Contract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "RestockRequests",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "RestockRequests");
        }
    }
}
