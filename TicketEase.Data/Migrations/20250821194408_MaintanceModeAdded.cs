using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketEase.Data.Migrations
{
    /// <inheritdoc />
    public partial class MaintanceModeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaintenanceMode = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "MaintenanceMode", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2025, 8, 21, 22, 44, 8, 72, DateTimeKind.Local).AddTicks(6298), false, false, null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 21, 22, 44, 8, 226, DateTimeKind.Local).AddTicks(2051), "$2a$11$47FzyOsIE.WgwZxEnUaII.HQOKtgDGM8G8C2xi6Vno2SelojK3.ne" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 21, 21, 33, 0, 349, DateTimeKind.Local).AddTicks(2203), "$2a$11$DIEZ9FC5FDTKK4u9LE0sF.ztRiNSYSFXTCuXQBecdXB1punNi/JlO" });
        }
    }
}
