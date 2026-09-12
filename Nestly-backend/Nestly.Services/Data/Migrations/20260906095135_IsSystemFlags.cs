using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nestly.Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class IsSystemFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystemRole",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystemPost",
                table: "BlogPosts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystemCategory",
                table: "BlogCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "BlogCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsSystemCategory",
                value: true);

            migrationBuilder.UpdateData(
                table: "BlogCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsSystemCategory",
                value: true);

            migrationBuilder.UpdateData(
                table: "BlogCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsSystemCategory",
                value: true);

            migrationBuilder.UpdateData(
                table: "BlogCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsSystemCategory",
                value: true);

            migrationBuilder.UpdateData(
                table: "BlogCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsSystemCategory",
                value: true);

            migrationBuilder.UpdateData(
                table: "BlogCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsSystemCategory",
                value: true);

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "IsSystemPost" },
                values: new object[] { new DateTime(2026, 8, 12, 9, 51, 34, 807, DateTimeKind.Utc).AddTicks(4529), true });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreatedAt", "IsSystemPost" },
                values: new object[] { new DateTime(2026, 8, 15, 9, 51, 34, 807, DateTimeKind.Utc).AddTicks(4544), true });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreatedAt", "IsSystemPost" },
                values: new object[] { new DateTime(2026, 8, 17, 9, 51, 34, 807, DateTimeKind.Utc).AddTicks(4547), true });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "CreatedAt", "IsSystemPost" },
                values: new object[] { new DateTime(2026, 8, 18, 9, 51, 34, 807, DateTimeKind.Utc).AddTicks(4549), true });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "CreatedAt", "IsSystemPost" },
                values: new object[] { new DateTime(2026, 8, 20, 9, 51, 34, 807, DateTimeKind.Utc).AddTicks(4552), true });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "CreatedAt", "IsSystemPost" },
                values: new object[] { new DateTime(2026, 8, 22, 9, 51, 34, 807, DateTimeKind.Utc).AddTicks(4554), true });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "CreatedAt", "IsSystemPost" },
                values: new object[] { new DateTime(2026, 8, 24, 9, 51, 34, 807, DateTimeKind.Utc).AddTicks(4557), true });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "CreatedAt", "IsSystemPost" },
                values: new object[] { new DateTime(2026, 8, 27, 9, 51, 34, 807, DateTimeKind.Utc).AddTicks(4559), true });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "CreatedAt", "IsSystemPost" },
                values: new object[] { new DateTime(2026, 8, 29, 9, 51, 34, 807, DateTimeKind.Utc).AddTicks(4562), true });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "CreatedAt", "IsSystemPost" },
                values: new object[] { new DateTime(2026, 8, 31, 9, 51, 34, 807, DateTimeKind.Utc).AddTicks(4564), true });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "CreatedAt", "IsSystemPost" },
                values: new object[] { new DateTime(2026, 9, 2, 9, 51, 34, 807, DateTimeKind.Utc).AddTicks(4567), true });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "CreatedAt", "IsSystemPost" },
                values: new object[] { new DateTime(2026, 9, 4, 9, 51, 34, 807, DateTimeKind.Utc).AddTicks(4570), true });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "IsSystemRole",
                value: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "IsSystemRole",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystemRole",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsSystemPost",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "IsSystemCategory",
                table: "BlogCategories");

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
        }
    }
}
