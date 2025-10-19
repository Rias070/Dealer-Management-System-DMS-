using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_Accounts_AccountId1",
                table: "Tokens");

            migrationBuilder.DropIndex(
                name: "IX_Tokens_AccountId1",
                table: "Tokens");

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "Tokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId1",
                table: "Tokens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_AccountId1",
                table: "Tokens",
                column: "AccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_Accounts_AccountId1",
                table: "Tokens",
                column: "AccountId1",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
