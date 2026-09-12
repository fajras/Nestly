using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nestly.Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameDoctorTestUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "Naida", "Bećirović" });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "TestDoctor", "User" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 13, 15, 2, 46, 287, DateTimeKind.Utc).AddTicks(617));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 16, 15, 2, 46, 287, DateTimeKind.Utc).AddTicks(642));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 18, 15, 2, 46, 287, DateTimeKind.Utc).AddTicks(645));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 19, 15, 2, 46, 287, DateTimeKind.Utc).AddTicks(650));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 15, 2, 46, 287, DateTimeKind.Utc).AddTicks(654));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 23, 15, 2, 46, 287, DateTimeKind.Utc).AddTicks(660));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 25, 15, 2, 46, 287, DateTimeKind.Utc).AddTicks(663));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 28, 15, 2, 46, 287, DateTimeKind.Utc).AddTicks(669));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 30, 15, 2, 46, 287, DateTimeKind.Utc).AddTicks(676));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 1, 15, 2, 46, 287, DateTimeKind.Utc).AddTicks(679));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 3, 15, 2, 46, 287, DateTimeKind.Utc).AddTicks(682));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 5, 15, 2, 46, 287, DateTimeKind.Utc).AddTicks(688));
        }
    }
}
