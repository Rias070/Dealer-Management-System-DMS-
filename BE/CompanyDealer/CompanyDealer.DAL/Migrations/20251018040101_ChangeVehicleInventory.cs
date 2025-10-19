using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeVehicleInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Inventories_InventoryId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_InventoryId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Inventories");

            migrationBuilder.CreateTable(
                name: "InventoryVehicles",
                columns: table => new
                {
                    InventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryVehicles", x => new { x.InventoryId, x.VehicleId });
                    table.ForeignKey(
                        name: "FK_InventoryVehicles_Inventories_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryVehicles_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryVehicles_VehicleId",
                table: "InventoryVehicles",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryVehicles");

            migrationBuilder.AddColumn<Guid>(
                name: "InventoryId",
                table: "Vehicles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Inventories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_InventoryId",
                table: "Vehicles",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Inventories_InventoryId",
                table: "Vehicles",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
