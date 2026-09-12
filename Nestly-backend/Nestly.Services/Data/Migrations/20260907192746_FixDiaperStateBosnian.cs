using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nestly.Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixDiaperStateBosnian : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 13, 19, 27, 42, 725, DateTimeKind.Utc).AddTicks(5777));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 16, 19, 27, 42, 725, DateTimeKind.Utc).AddTicks(5795));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 18, 19, 27, 42, 725, DateTimeKind.Utc).AddTicks(5799));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 19, 19, 27, 42, 725, DateTimeKind.Utc).AddTicks(5802));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 19, 27, 42, 725, DateTimeKind.Utc).AddTicks(5805));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 23, 19, 27, 42, 725, DateTimeKind.Utc).AddTicks(5807));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 25, 19, 27, 42, 725, DateTimeKind.Utc).AddTicks(5810));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 28, 19, 27, 42, 725, DateTimeKind.Utc).AddTicks(5824));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 30, 19, 27, 42, 725, DateTimeKind.Utc).AddTicks(5827));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 1, 19, 27, 42, 725, DateTimeKind.Utc).AddTicks(5832));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 3, 19, 27, 42, 725, DateTimeKind.Utc).AddTicks(5834));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 5, 19, 27, 42, 725, DateTimeKind.Utc).AddTicks(5837));

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5001L,
                column: "DiaperState",
                value: "mokra");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5002L,
                column: "DiaperState",
                value: "stolica");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5003L,
                column: "DiaperState",
                value: "kombinovano");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5004L,
                column: "DiaperState",
                value: "mokra");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5005L,
                column: "DiaperState",
                value: "mokra");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5006L,
                column: "DiaperState",
                value: "stolica");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5007L,
                column: "DiaperState",
                value: "kombinovano");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5008L,
                column: "DiaperState",
                value: "mokra");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5009L,
                column: "DiaperState",
                value: "mokra");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5010L,
                column: "DiaperState",
                value: "stolica");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5011L,
                column: "DiaperState",
                value: "kombinovano");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5012L,
                column: "DiaperState",
                value: "mokra");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5013L,
                column: "DiaperState",
                value: "mokra");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5014L,
                column: "DiaperState",
                value: "stolica");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5015L,
                column: "DiaperState",
                value: "kombinovano");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5016L,
                column: "DiaperState",
                value: "mokra");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5017L,
                column: "DiaperState",
                value: "mokra");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5018L,
                column: "DiaperState",
                value: "stolica");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5019L,
                column: "DiaperState",
                value: "kombinovano");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5020L,
                column: "DiaperState",
                value: "mokra");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5021L,
                column: "DiaperState",
                value: "mokra");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5022L,
                column: "DiaperState",
                value: "stolica");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5023L,
                column: "DiaperState",
                value: "kombinovano");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5024L,
                column: "DiaperState",
                value: "mokra");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5025L,
                column: "DiaperState",
                value: "mokra");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5026L,
                column: "DiaperState",
                value: "stolica");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5027L,
                column: "DiaperState",
                value: "kombinovano");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5028L,
                column: "DiaperState",
                value: "mokra");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 13, 15, 39, 3, 933, DateTimeKind.Utc).AddTicks(5720));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 16, 15, 39, 3, 933, DateTimeKind.Utc).AddTicks(5730));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 18, 15, 39, 3, 933, DateTimeKind.Utc).AddTicks(5733));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 19, 15, 39, 3, 933, DateTimeKind.Utc).AddTicks(5735));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 15, 39, 3, 933, DateTimeKind.Utc).AddTicks(5738));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 23, 15, 39, 3, 933, DateTimeKind.Utc).AddTicks(5740));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 25, 15, 39, 3, 933, DateTimeKind.Utc).AddTicks(5743));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 28, 15, 39, 3, 933, DateTimeKind.Utc).AddTicks(5745));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 30, 15, 39, 3, 933, DateTimeKind.Utc).AddTicks(5748));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 1, 15, 39, 3, 933, DateTimeKind.Utc).AddTicks(5750));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 3, 15, 39, 3, 933, DateTimeKind.Utc).AddTicks(5752));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 5, 15, 39, 3, 933, DateTimeKind.Utc).AddTicks(5754));

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5001L,
                column: "DiaperState",
                value: "Wet");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5002L,
                column: "DiaperState",
                value: "Dirty");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5003L,
                column: "DiaperState",
                value: "Mixed");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5004L,
                column: "DiaperState",
                value: "Wet");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5005L,
                column: "DiaperState",
                value: "Wet");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5006L,
                column: "DiaperState",
                value: "Dirty");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5007L,
                column: "DiaperState",
                value: "Mixed");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5008L,
                column: "DiaperState",
                value: "Wet");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5009L,
                column: "DiaperState",
                value: "Wet");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5010L,
                column: "DiaperState",
                value: "Dirty");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5011L,
                column: "DiaperState",
                value: "Mixed");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5012L,
                column: "DiaperState",
                value: "Wet");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5013L,
                column: "DiaperState",
                value: "Wet");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5014L,
                column: "DiaperState",
                value: "Dirty");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5015L,
                column: "DiaperState",
                value: "Mixed");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5016L,
                column: "DiaperState",
                value: "Wet");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5017L,
                column: "DiaperState",
                value: "Wet");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5018L,
                column: "DiaperState",
                value: "Dirty");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5019L,
                column: "DiaperState",
                value: "Mixed");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5020L,
                column: "DiaperState",
                value: "Wet");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5021L,
                column: "DiaperState",
                value: "Wet");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5022L,
                column: "DiaperState",
                value: "Dirty");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5023L,
                column: "DiaperState",
                value: "Mixed");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5024L,
                column: "DiaperState",
                value: "Wet");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5025L,
                column: "DiaperState",
                value: "Wet");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5026L,
                column: "DiaperState",
                value: "Dirty");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5027L,
                column: "DiaperState",
                value: "Mixed");

            migrationBuilder.UpdateData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5028L,
                column: "DiaperState",
                value: "Wet");
        }
    }
}
