using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusBookingAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddOtpCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OtpCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phone = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    AttemptCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpCodes", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 4,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 5,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 6,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 7,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 8,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 9,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 10,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 11,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 12,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 13,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 14,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 15,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 16,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 17,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 18,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 19,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 20,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 21,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 22,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 23,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 24,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 25,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 26,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 27,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 28,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 29,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 30,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 31,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 32,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 33,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 34,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 35,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 36,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 37,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 38,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 39,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 40,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 41,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 42,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 43,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 44,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 45,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 46,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 47,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 48,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 49,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 50,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 51,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 52,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 53,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 54,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 55,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 56,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 57,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 58,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 59,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 60,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 61,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 62,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 63,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 64,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 65,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 66,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 67,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 68,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 69,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 70,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 71,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 72,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 73,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 74,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 75,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 76,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 77,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 78,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 79,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 80,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 81,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 82,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 83,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 84,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 85,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 86,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 87,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 88,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 89,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 90,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 91,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 92,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 93,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 94,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 95,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 96,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 97,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 98,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 99,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 100,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 101,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 102,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 103,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 104,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 105,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 106,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 107,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 108,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 109,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 110,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 111,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 112,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 113,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 114,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 115,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 116,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 117,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 118,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 119,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 120,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 121,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 122,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 123,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 124,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 125,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 126,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 127,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 128,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 129,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 130,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 131,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 132,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 133,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 134,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 135,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 136,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 137,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 138,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 139,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 140,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 141,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 142,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 143,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 144,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 145,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 146,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 147,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 148,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 149,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 150,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 151,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 152,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 153,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 154,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 155,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 156,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 157,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 158,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 159,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 160,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 161,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 162,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 163,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 164,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 165,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 166,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 167,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 168,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 169,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 170,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 171,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 172,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 173,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 174,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 175,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 176,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 177,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 178,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 179,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 180,
                column: "StartTime",
                value: new DateTime(2025, 10, 22, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.CreateIndex(
                name: "IX_OtpCodes_Phone",
                table: "OtpCodes",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_OtpCodes_Phone_ExpiresAt",
                table: "OtpCodes",
                columns: new[] { "Phone", "ExpiresAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OtpCodes");

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 4,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 5,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 6,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 7,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 8,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 9,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 10,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 11,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 12,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 13,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 14,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 15,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 16,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 17,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 18,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 19,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 20,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 21,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 22,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 23,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 24,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 25,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 26,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 27,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 28,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 29,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 30,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 31,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 32,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 33,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 34,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 35,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 36,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 37,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 38,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 39,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 40,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 41,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 42,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 43,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 44,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 45,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 46,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 47,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 48,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 49,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 50,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 51,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 52,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 53,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 54,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 55,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 56,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 57,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 58,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 59,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 60,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 61,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 62,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 63,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 64,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 65,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 66,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 67,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 68,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 69,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 70,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 71,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 72,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 73,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 74,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 75,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 76,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 77,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 78,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 79,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 80,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 81,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 82,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 83,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 84,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 85,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 86,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 87,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 88,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 89,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 90,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 91,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 92,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 93,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 94,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 95,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 96,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 97,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 98,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 99,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 100,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 101,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 102,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 103,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 104,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 105,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 106,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 107,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 108,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 109,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 110,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 111,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 112,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 113,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 114,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 115,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 116,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 117,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 118,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 119,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 120,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 121,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 122,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 123,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 124,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 125,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 126,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 127,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 128,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 129,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 130,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 131,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 132,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 133,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 134,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 135,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 136,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 137,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 138,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 139,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 140,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 141,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 142,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 143,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 144,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 145,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 146,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 147,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 148,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 149,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 150,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 151,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 152,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 153,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 154,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 155,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 156,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 157,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 158,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 159,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 160,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 161,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 162,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 163,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 164,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 165,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 166,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 167,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 168,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 169,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 170,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 171,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 172,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 173,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 174,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 175,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 6, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 176,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 8, 15, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 177,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 178,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 14, 30, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 179,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 18, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 180,
                column: "StartTime",
                value: new DateTime(2025, 10, 21, 19, 45, 0, 0, DateTimeKind.Utc));
        }
    }
}
