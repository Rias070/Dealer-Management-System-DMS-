using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ContractField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DealerId",
                table: "Contracts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_DealerId",
                table: "Contracts",
                column: "DealerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Dealers_DealerId",
                table: "Contracts",
                column: "DealerId",
                principalTable: "Dealers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Dealers_DealerId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_DealerId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "DealerId",
                table: "Contracts");
        }
    }
}
