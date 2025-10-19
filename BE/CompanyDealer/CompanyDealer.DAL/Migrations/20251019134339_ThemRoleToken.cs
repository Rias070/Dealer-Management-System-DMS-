using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ThemRoleToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRole_Accounts_AccountsId",
                table: "AccountRole");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountRole_Role_RolesId",
                table: "AccountRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.RenameColumn(
                name: "RolesId",
                table: "AccountRole",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "AccountsId",
                table: "AccountRole",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountRole_RolesId",
                table: "AccountRole",
                newName: "IX_AccountRole_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId1 = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tokens_Accounts_AccountId1",
                        column: x => x.AccountId1,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_AccountId",
                table: "Tokens",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_AccountId1",
                table: "Tokens",
                column: "AccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRole_Account_AccountId",
                table: "AccountRole",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRole_Role_RoleId",
                table: "AccountRole",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountRole_Account_AccountId",
                table: "AccountRole");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountRole_Role_RoleId",
                table: "AccountRole");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "AccountRole",
                newName: "RolesId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "AccountRole",
                newName: "AccountsId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountRole_RoleId",
                table: "AccountRole",
                newName: "IX_AccountRole_RolesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRole_Accounts_AccountsId",
                table: "AccountRole",
                column: "AccountsId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountRole_Role_RolesId",
                table: "AccountRole",
                column: "RolesId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
