using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nestly.Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToLeafEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SymptomDiaries_ParentProfileId_Date",
                table: "SymptomDiaries");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "SymptomDiaries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SymptomDiaries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "SleepLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SleepLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "QaQuestions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "QaQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Pregnancies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Pregnancies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Milestones",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Milestones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "MealPlans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MealPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "HealthEntries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HealthEntries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "FeedingLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FeedingLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "DiaperLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DiaperLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "CalendarEvents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CalendarEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 12, 10, 20, 57, 333, DateTimeKind.Utc).AddTicks(507));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 15, 10, 20, 57, 333, DateTimeKind.Utc).AddTicks(520));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 17, 10, 20, 57, 333, DateTimeKind.Utc).AddTicks(524));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 18, 10, 20, 57, 333, DateTimeKind.Utc).AddTicks(701));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 20, 10, 20, 57, 333, DateTimeKind.Utc).AddTicks(736));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 22, 10, 20, 57, 333, DateTimeKind.Utc).AddTicks(767));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 24, 10, 20, 57, 333, DateTimeKind.Utc).AddTicks(791));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 27, 10, 20, 57, 333, DateTimeKind.Utc).AddTicks(805));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 29, 10, 20, 57, 333, DateTimeKind.Utc).AddTicks(808));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 31, 10, 20, 57, 333, DateTimeKind.Utc).AddTicks(811));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 2, 10, 20, 57, 333, DateTimeKind.Utc).AddTicks(814));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 4, 10, 20, 57, 333, DateTimeKind.Utc).AddTicks(816));

            migrationBuilder.UpdateData(
                table: "Pregnancies",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DeletedAt", "IsDeleted" },
                values: new object[] { null, false });

            migrationBuilder.CreateIndex(
                name: "IX_SymptomDiaries_ParentProfileId_Date",
                table: "SymptomDiaries",
                columns: new[] { "ParentProfileId", "Date" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SymptomDiaries_ParentProfileId_Date",
                table: "SymptomDiaries");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "SymptomDiaries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SymptomDiaries");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "SleepLogs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SleepLogs");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "QaQuestions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "QaQuestions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Pregnancies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Pregnancies");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "HealthEntries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HealthEntries");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "FeedingLogs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FeedingLogs");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "DiaperLogs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DiaperLogs");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CalendarEvents");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CalendarEvents");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 12, 10, 10, 30, 453, DateTimeKind.Utc).AddTicks(6015));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 15, 10, 10, 30, 453, DateTimeKind.Utc).AddTicks(6051));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 17, 10, 10, 30, 453, DateTimeKind.Utc).AddTicks(6061));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 18, 10, 10, 30, 453, DateTimeKind.Utc).AddTicks(6070));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 20, 10, 10, 30, 453, DateTimeKind.Utc).AddTicks(6078));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 22, 10, 10, 30, 453, DateTimeKind.Utc).AddTicks(6087));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 24, 10, 10, 30, 453, DateTimeKind.Utc).AddTicks(6095));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 27, 10, 10, 30, 453, DateTimeKind.Utc).AddTicks(6104));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 29, 10, 10, 30, 453, DateTimeKind.Utc).AddTicks(6112));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 31, 10, 10, 30, 453, DateTimeKind.Utc).AddTicks(6120));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 2, 10, 10, 30, 453, DateTimeKind.Utc).AddTicks(6129));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 4, 10, 10, 30, 453, DateTimeKind.Utc).AddTicks(6138));

            migrationBuilder.CreateIndex(
                name: "IX_SymptomDiaries_ParentProfileId_Date",
                table: "SymptomDiaries",
                columns: new[] { "ParentProfileId", "Date" },
                unique: true);
        }
    }
}
