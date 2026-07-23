using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nestly.Services.Migrations.NestlyDb
{
    /// <inheritdoc />
    public partial class AddHealthDeviationAlerts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HealthDeviationAlerts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyId = table.Column<long>(type: "bigint", nullable: false),
                    ParameterType = table.Column<int>(type: "int", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Recommendation = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DetectedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthDeviationAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthDeviationAlerts_BabyProfiles_BabyId",
                        column: x => x.BabyId,
                        principalTable: "BabyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthDeviationAlerts_BabyId_ParameterType_IsResolved",
                table: "HealthDeviationAlerts",
                columns: new[] { "BabyId", "ParameterType", "IsResolved" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthDeviationAlerts");
        }
    }
}
