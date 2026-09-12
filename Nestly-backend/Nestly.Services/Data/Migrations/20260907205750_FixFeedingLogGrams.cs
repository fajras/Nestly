using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nestly.Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixFeedingLogGrams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 13, 20, 57, 47, 343, DateTimeKind.Utc).AddTicks(9249));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 16, 20, 57, 47, 343, DateTimeKind.Utc).AddTicks(9265));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 18, 20, 57, 47, 343, DateTimeKind.Utc).AddTicks(9268));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 19, 20, 57, 47, 343, DateTimeKind.Utc).AddTicks(9270));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 20, 57, 47, 343, DateTimeKind.Utc).AddTicks(9273));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 23, 20, 57, 47, 343, DateTimeKind.Utc).AddTicks(9276));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 25, 20, 57, 47, 343, DateTimeKind.Utc).AddTicks(9279));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 28, 20, 57, 47, 343, DateTimeKind.Utc).AddTicks(9281));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 30, 20, 57, 47, 343, DateTimeKind.Utc).AddTicks(9284));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 1, 20, 57, 47, 343, DateTimeKind.Utc).AddTicks(9286));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 3, 20, 57, 47, 343, DateTimeKind.Utc).AddTicks(9289));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 5, 20, 57, 47, 343, DateTimeKind.Utc).AddTicks(9291));

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5001L,
                column: "AmountMl",
                value: 47m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5002L,
                column: "AmountMl",
                value: 54m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5003L,
                column: "AmountMl",
                value: 61m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5004L,
                column: "AmountMl",
                value: 68m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5013L,
                column: "AmountMl",
                value: 75m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5014L,
                column: "AmountMl",
                value: 82m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5015L,
                column: "AmountMl",
                value: 89m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5016L,
                column: "AmountMl",
                value: 46m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5017L,
                column: "AmountMl",
                value: 53m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5018L,
                column: "AmountMl",
                value: 60m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5019L,
                column: "AmountMl",
                value: 67m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5020L,
                column: "AmountMl",
                value: 74m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5021L,
                column: "AmountMl",
                value: 81m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5022L,
                column: "AmountMl",
                value: 88m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5023L,
                column: "AmountMl",
                value: 45m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5024L,
                column: "AmountMl",
                value: 52m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5025L,
                column: "AmountMl",
                value: 59m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5026L,
                column: "AmountMl",
                value: 66m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5027L,
                column: "AmountMl",
                value: 73m);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5028L,
                column: "AmountMl",
                value: 80m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5001L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5002L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5003L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5004L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5013L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5014L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5015L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5016L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5017L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5018L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5019L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5020L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5021L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5022L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5023L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5024L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5025L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5026L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5027L,
                column: "AmountMl",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5028L,
                column: "AmountMl",
                value: null);
        }
    }
}
