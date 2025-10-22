using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyDealer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTestDriveRecordFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "TestDriveRecords",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedBy",
                table: "TestDriveRecords",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByName",
                table: "TestDriveRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TestDriveRecords",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "TestDriveRecords",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "TestDriveRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAt",
                table: "TestDriveRecords",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "TestDriveRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TestDriveRecords",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "TestDriveRecords");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "TestDriveRecords");

            migrationBuilder.DropColumn(
                name: "ApprovedByName",
                table: "TestDriveRecords");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TestDriveRecords");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TestDriveRecords");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "TestDriveRecords");

            migrationBuilder.DropColumn(
                name: "RejectedAt",
                table: "TestDriveRecords");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "TestDriveRecords");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TestDriveRecords");
        }
    }
}
