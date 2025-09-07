using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nestly.Services.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FetalDevelopmentWeeks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekNumber = table.Column<short>(type: "smallint", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BabyDevelopment = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    MotherChanges = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FetalDevelopmentWeeks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyAdvices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekNumber = table.Column<short>(type: "smallint", nullable: false),
                    AdviceText = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyAdvices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BabyProfiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BabyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BabyProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BabyProfiles_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPosts_AppUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AppUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MedicationPlans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MedicineName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Dose = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicationPlans_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pregnancies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LmpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pregnancies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pregnancies_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QaQuestions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    AskedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AskedById = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QaQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QaQuestions_AppUsers_AskedById",
                        column: x => x.AskedById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMembers",
                columns: table => new
                {
                    RoomId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMembers", x => new { x.RoomId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ChatMembers_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMembers_ChatRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BabyGrowths",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyId = table.Column<long>(type: "bigint", nullable: false),
                    WeekNumber = table.Column<short>(type: "smallint", nullable: false),
                    WeightKg = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    HeightCm = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    HeadCircumferenceCm = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BabyGrowths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BabyGrowths_BabyProfiles_BabyId",
                        column: x => x.BabyId,
                        principalTable: "BabyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalendarEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarEvents_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CalendarEvents_BabyProfiles_BabyId",
                        column: x => x.BabyId,
                        principalTable: "BabyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiaperLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyId = table.Column<long>(type: "bigint", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangeTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DiaperState = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaperLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiaperLogs_BabyProfiles_BabyId",
                        column: x => x.BabyId,
                        principalTable: "BabyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedingLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyId = table.Column<long>(type: "bigint", nullable: false),
                    FeedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FeedTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    AmountMl = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FoodTypeId = table.Column<long>(type: "bigint", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedingLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedingLogs_BabyProfiles_BabyId",
                        column: x => x.BabyId,
                        principalTable: "BabyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedingLogs_FoodTypes_FoodTypeId",
                        column: x => x.FoodTypeId,
                        principalTable: "FoodTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HealthEntries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyId = table.Column<long>(type: "bigint", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TemperatureC = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: true),
                    Medicines = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DoctorVisit = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthEntries_BabyProfiles_BabyId",
                        column: x => x.BabyId,
                        principalTable: "BabyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealPlans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyId = table.Column<long>(type: "bigint", nullable: false),
                    WeekNumber = table.Column<short>(type: "smallint", nullable: false),
                    FoodItem = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FoodRating = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealPlans_BabyProfiles_BabyId",
                        column: x => x.BabyId,
                        principalTable: "BabyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Milestones",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AchievedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Milestones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Milestones_BabyProfiles_BabyId",
                        column: x => x.BabyId,
                        principalTable: "BabyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SleepLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyId = table.Column<long>(type: "bigint", nullable: false),
                    SleepDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SleepLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SleepLogs_BabyProfiles_BabyId",
                        column: x => x.BabyId,
                        principalTable: "BabyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostCategories",
                columns: table => new
                {
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostCategories", x => new { x.PostId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_BlogPostCategories_BlogCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "BlogCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPostCategories_BlogPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicationIntakeLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<long>(type: "bigint", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IntakeTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Taken = table.Column<bool>(type: "bit", nullable: false),
                    TakenAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationIntakeLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicationIntakeLogs_MedicationPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "MedicationPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicationScheduleTimes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<long>(type: "bigint", nullable: false),
                    IntakeTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationScheduleTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicationScheduleTimes_MedicationPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "MedicationPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QaAnswers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    AnsweredByUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnsweredById = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QaAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QaAnswers_AppUsers_AnsweredById",
                        column: x => x.AnsweredById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QaAnswers_QaQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "QaQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Email",
                table: "AppUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Username",
                table: "AppUsers",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BabyGrowths_BabyId_WeekNumber",
                table: "BabyGrowths",
                columns: new[] { "BabyId", "WeekNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BabyProfiles_UserId",
                table: "BabyProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostCategories_CategoryId",
                table: "BlogPostCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_AuthorId",
                table: "BlogPosts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_BabyId_StartAt",
                table: "CalendarEvents",
                columns: new[] { "BabyId", "StartAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_UserId",
                table: "CalendarEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMembers_UserId",
                table: "ChatMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_RoomId",
                table: "ChatMessages",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_UserId",
                table: "ChatMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiaperLogs_BabyId_ChangeDate",
                table: "DiaperLogs",
                columns: new[] { "BabyId", "ChangeDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FeedingLogs_BabyId",
                table: "FeedingLogs",
                column: "BabyId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedingLogs_FoodTypeId",
                table: "FeedingLogs",
                column: "FoodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FetalDevelopmentWeeks_WeekNumber",
                table: "FetalDevelopmentWeeks",
                column: "WeekNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthEntries_BabyId_EntryDate",
                table: "HealthEntries",
                columns: new[] { "BabyId", "EntryDate" });

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_BabyId_WeekNumber",
                table: "MealPlans",
                columns: new[] { "BabyId", "WeekNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicationIntakeLogs_PlanId",
                table: "MedicationIntakeLogs",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationPlans_UserId_MedicineName",
                table: "MedicationPlans",
                columns: new[] { "UserId", "MedicineName" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicationScheduleTimes_PlanId",
                table: "MedicationScheduleTimes",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Milestones_BabyId_AchievedDate",
                table: "Milestones",
                columns: new[] { "BabyId", "AchievedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Pregnancies_UserId_CreatedAt",
                table: "Pregnancies",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_QaAnswers_AnsweredById",
                table: "QaAnswers",
                column: "AnsweredById");

            migrationBuilder.CreateIndex(
                name: "IX_QaAnswers_QuestionId_CreatedAt",
                table: "QaAnswers",
                columns: new[] { "QuestionId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_QaQuestions_AskedById",
                table: "QaQuestions",
                column: "AskedById");

            migrationBuilder.CreateIndex(
                name: "IX_QaQuestions_AskedByUserId_CreatedAt",
                table: "QaQuestions",
                columns: new[] { "AskedByUserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SleepLogs_BabyId_SleepDate",
                table: "SleepLogs",
                columns: new[] { "BabyId", "SleepDate" });

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyAdvices_WeekNumber",
                table: "WeeklyAdvices",
                column: "WeekNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BabyGrowths");

            migrationBuilder.DropTable(
                name: "BlogPostCategories");

            migrationBuilder.DropTable(
                name: "CalendarEvents");

            migrationBuilder.DropTable(
                name: "ChatMembers");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "DiaperLogs");

            migrationBuilder.DropTable(
                name: "FeedingLogs");

            migrationBuilder.DropTable(
                name: "FetalDevelopmentWeeks");

            migrationBuilder.DropTable(
                name: "HealthEntries");

            migrationBuilder.DropTable(
                name: "MealPlans");

            migrationBuilder.DropTable(
                name: "MedicationIntakeLogs");

            migrationBuilder.DropTable(
                name: "MedicationScheduleTimes");

            migrationBuilder.DropTable(
                name: "Milestones");

            migrationBuilder.DropTable(
                name: "Pregnancies");

            migrationBuilder.DropTable(
                name: "QaAnswers");

            migrationBuilder.DropTable(
                name: "SleepLogs");

            migrationBuilder.DropTable(
                name: "WeeklyAdvices");

            migrationBuilder.DropTable(
                name: "BlogCategories");

            migrationBuilder.DropTable(
                name: "BlogPosts");

            migrationBuilder.DropTable(
                name: "ChatRooms");

            migrationBuilder.DropTable(
                name: "FoodTypes");

            migrationBuilder.DropTable(
                name: "MedicationPlans");

            migrationBuilder.DropTable(
                name: "QaQuestions");

            migrationBuilder.DropTable(
                name: "BabyProfiles");

            migrationBuilder.DropTable(
                name: "AppUsers");
        }
    }
}
