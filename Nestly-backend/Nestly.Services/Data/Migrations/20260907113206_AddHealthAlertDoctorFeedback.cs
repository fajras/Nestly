using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nestly.Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHealthAlertDoctorFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DoctorFeedbackAt",
                table: "HealthDeviationAlerts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DoctorFeedbackByDoctorId",
                table: "HealthDeviationAlerts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorFeedbackComment",
                table: "HealthDeviationAlerts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DoctorFeedbackIsAccurate",
                table: "HealthDeviationAlerts",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MlRetrainingWatermarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelGroup = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastTrainedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordCountAtLastTraining = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MlRetrainingWatermarks", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 13, 11, 32, 4, 633, DateTimeKind.Utc).AddTicks(9635));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 16, 11, 32, 4, 633, DateTimeKind.Utc).AddTicks(9648));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 18, 11, 32, 4, 633, DateTimeKind.Utc).AddTicks(9652));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 19, 11, 32, 4, 633, DateTimeKind.Utc).AddTicks(9654));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 11, 32, 4, 633, DateTimeKind.Utc).AddTicks(9656));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 23, 11, 32, 4, 633, DateTimeKind.Utc).AddTicks(9658));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 25, 11, 32, 4, 633, DateTimeKind.Utc).AddTicks(9661));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 28, 11, 32, 4, 633, DateTimeKind.Utc).AddTicks(9663));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 30, 11, 32, 4, 633, DateTimeKind.Utc).AddTicks(9666));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 1, 11, 32, 4, 633, DateTimeKind.Utc).AddTicks(9668));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 3, 11, 32, 4, 633, DateTimeKind.Utc).AddTicks(9670));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 5, 11, 32, 4, 633, DateTimeKind.Utc).AddTicks(9672));

            migrationBuilder.CreateIndex(
                name: "IX_HealthDeviationAlerts_DoctorFeedbackByDoctorId",
                table: "HealthDeviationAlerts",
                column: "DoctorFeedbackByDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_MlRetrainingWatermarks_ModelGroup",
                table: "MlRetrainingWatermarks",
                column: "ModelGroup",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthDeviationAlerts_DoctorProfiles_DoctorFeedbackByDoctorId",
                table: "HealthDeviationAlerts",
                column: "DoctorFeedbackByDoctorId",
                principalTable: "DoctorProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthDeviationAlerts_DoctorProfiles_DoctorFeedbackByDoctorId",
                table: "HealthDeviationAlerts");

            migrationBuilder.DropTable(
                name: "MlRetrainingWatermarks");

            migrationBuilder.DropIndex(
                name: "IX_HealthDeviationAlerts_DoctorFeedbackByDoctorId",
                table: "HealthDeviationAlerts");

            migrationBuilder.DropColumn(
                name: "DoctorFeedbackAt",
                table: "HealthDeviationAlerts");

            migrationBuilder.DropColumn(
                name: "DoctorFeedbackByDoctorId",
                table: "HealthDeviationAlerts");

            migrationBuilder.DropColumn(
                name: "DoctorFeedbackComment",
                table: "HealthDeviationAlerts");

            migrationBuilder.DropColumn(
                name: "DoctorFeedbackIsAccurate",
                table: "HealthDeviationAlerts");

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
        }
    }
}
