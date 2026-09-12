using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nestly.Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameParentTestUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "Amela", "Hasić" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "FirstName", "LastName" },
                values: new object[] { "TestParent", "User" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 13, 14, 16, 34, 316, DateTimeKind.Utc).AddTicks(1808));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 16, 14, 16, 34, 316, DateTimeKind.Utc).AddTicks(1820));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 18, 14, 16, 34, 316, DateTimeKind.Utc).AddTicks(1822));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 19, 14, 16, 34, 316, DateTimeKind.Utc).AddTicks(1825));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 14, 16, 34, 316, DateTimeKind.Utc).AddTicks(1827));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 23, 14, 16, 34, 316, DateTimeKind.Utc).AddTicks(1830));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 25, 14, 16, 34, 316, DateTimeKind.Utc).AddTicks(1832));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 28, 14, 16, 34, 316, DateTimeKind.Utc).AddTicks(1835));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 30, 14, 16, 34, 316, DateTimeKind.Utc).AddTicks(1837));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 1, 14, 16, 34, 316, DateTimeKind.Utc).AddTicks(1839));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 3, 14, 16, 34, 316, DateTimeKind.Utc).AddTicks(1845));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 5, 14, 16, 34, 316, DateTimeKind.Utc).AddTicks(1847));
        }
    }
}
