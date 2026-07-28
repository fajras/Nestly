using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nestly.Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedingLogAmountUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AmountUnit",
                table: "FeedingLogs",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "ml");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 14, 13, 49, 277, DateTimeKind.Utc).AddTicks(6921));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 6, 14, 13, 49, 277, DateTimeKind.Utc).AddTicks(6932));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 14, 13, 49, 277, DateTimeKind.Utc).AddTicks(6935));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 14, 13, 49, 277, DateTimeKind.Utc).AddTicks(6936));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 11, 14, 13, 49, 277, DateTimeKind.Utc).AddTicks(6938));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 13, 14, 13, 49, 277, DateTimeKind.Utc).AddTicks(6940));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 15, 14, 13, 49, 277, DateTimeKind.Utc).AddTicks(6942));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 18, 14, 13, 49, 277, DateTimeKind.Utc).AddTicks(6943));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 20, 14, 13, 49, 277, DateTimeKind.Utc).AddTicks(6945));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 14, 13, 49, 277, DateTimeKind.Utc).AddTicks(6947));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 24, 14, 13, 49, 277, DateTimeKind.Utc).AddTicks(6948));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 26, 14, 13, 49, 277, DateTimeKind.Utc).AddTicks(6950));

            migrationBuilder.UpdateData(
                table: "Pregnancies",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DueDate", "LmpDate" },
                values: new object[] { new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountUnit",
                table: "FeedingLogs");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 5, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2583));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2590));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 10, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2592));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 11, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2594));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 13, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2596));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 15, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2598));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 17, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2599));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 20, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2601));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 22, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2603));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 24, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2604));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 26, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2606));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 28, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2608));

            migrationBuilder.UpdateData(
                table: "Pregnancies",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DueDate", "LmpDate" },
                values: new object[] { new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
