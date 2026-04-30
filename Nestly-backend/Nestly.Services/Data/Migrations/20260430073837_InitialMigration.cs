using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nestly.Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostInteractions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    SpentSeconds = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostInteractions", x => x.Id);
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecommendationModelStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeightsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendationModelStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
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
                name: "MealRecommendations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekNumber = table.Column<short>(type: "smallint", nullable: false),
                    FoodTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealRecommendations_FoodTypes_FoodTypeId",
                        column: x => x.FoodTypeId,
                        principalTable: "FoodTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityUserId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUsers_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoctorProfiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorProfiles_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParentProfiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentProfiles_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Phase = table.Column<int>(type: "int", nullable: false),
                    WeekFrom = table.Column<int>(type: "int", nullable: true),
                    WeekTo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPosts_DoctorProfiles_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "DoctorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ChatConversations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User1Id = table.Column<long>(type: "bigint", nullable: false),
                    User2Id = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    ParentProfileId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatConversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatConversations_AppUsers_User1Id",
                        column: x => x.User1Id,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatConversations_AppUsers_User2Id",
                        column: x => x.User2Id,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatConversations_ParentProfiles_ParentProfileId",
                        column: x => x.ParentProfileId,
                        principalTable: "ParentProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MedicationPlans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentProfileId = table.Column<long>(type: "bigint", nullable: false),
                    MedicineName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Dose = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicationPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicationPlans_ParentProfiles_ParentProfileId",
                        column: x => x.ParentProfileId,
                        principalTable: "ParentProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pregnancies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentProfileId = table.Column<long>(type: "bigint", nullable: false),
                    LmpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CycleLengthDays = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pregnancies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pregnancies_ParentProfiles_ParentProfileId",
                        column: x => x.ParentProfileId,
                        principalTable: "ParentProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QaQuestions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AskedById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QaQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QaQuestions_ParentProfiles_AskedById",
                        column: x => x.AskedById,
                        principalTable: "ParentProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SymptomDiaries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentProfileId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Nausea = table.Column<int>(type: "int", nullable: true),
                    Fatigue = table.Column<int>(type: "int", nullable: true),
                    Headache = table.Column<int>(type: "int", nullable: true),
                    Heartburn = table.Column<int>(type: "int", nullable: true),
                    LegSwelling = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomDiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymptomDiaries_ParentProfiles_ParentProfileId",
                        column: x => x.ParentProfileId,
                        principalTable: "ParentProfiles",
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
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConversationId = table.Column<long>(type: "bigint", nullable: false),
                    SenderId = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_AppUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatConversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "ChatConversations",
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
                    TakenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReminderSent = table.Column<bool>(type: "bit", nullable: false)
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
                name: "BabyProfiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentProfileId = table.Column<long>(type: "bigint", nullable: false),
                    BabyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PregnancyId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BabyProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BabyProfiles_ParentProfiles_ParentProfileId",
                        column: x => x.ParentProfileId,
                        principalTable: "ParentProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BabyProfiles_Pregnancies_PregnancyId",
                        column: x => x.PregnancyId,
                        principalTable: "Pregnancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "QaAnswers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnsweredById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QaAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QaAnswers_DoctorProfiles_AnsweredById",
                        column: x => x.AnsweredById,
                        principalTable: "DoctorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_QaAnswers_QaQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "QaQuestions",
                        principalColumn: "Id");
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
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reminder24hSent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarEvents_BabyProfiles_BabyId",
                        column: x => x.BabyId,
                        principalTable: "BabyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalendarEvents_ParentProfiles_UserId",
                        column: x => x.UserId,
                        principalTable: "ParentProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiaperLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyId = table.Column<long>(type: "bigint", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "date", nullable: false),
                    ChangeTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DiaperState = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
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
                    AmountMl = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    FoodTypeId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "HealthEntries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyId = table.Column<long>(type: "bigint", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "date", nullable: false),
                    TemperatureC = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: true),
                    Medicines = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DoctorVisit = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
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
                    FoodTypeId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<short>(type: "smallint", nullable: true),
                    TriedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
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
                    table.ForeignKey(
                        name: "FK_MealPlans_FoodTypes_FoodTypeId",
                        column: x => x.FoodTypeId,
                        principalTable: "FoodTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Milestones",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BabyId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AchievedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    SleepDate = table.Column<DateTime>(type: "date", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
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

            migrationBuilder.InsertData(
                table: "BlogCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Trudnoća i zdravlje" },
                    { 2, "Njega bebe" },
                    { 3, "Ishrana i recepti" },
                    { 4, "Savjeti roditelja" },
                    { 5, "Psihološko zdravlje" },
                    { 6, "Razvoj djeteta" }
                });

            migrationBuilder.InsertData(
                table: "FetalDevelopmentWeeks",
                columns: new[] { "Id", "BabyDevelopment", "ImageUrl", "MotherChanges", "WeekNumber" },
                values: new object[,]
                {
                    { 1, "Trudnoća se računa od posljednje menstruacije. Oplodnja se još nije dogodila; tijelo se priprema za ovulaciju i zadebljava sluznicu materice.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development1.png", "Još nema specifičnih simptoma trudnoće. Moguće su uobičajene PMS promjene raspoloženja, nadutost i osjetljivost dojki.", (short)1 },
                    { 2, "Blizu sredine ciklusa dešava se ovulacija. Ako spermij oplodi jajnu ćeliju, nastaje zigota koja kreće ka materici.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development1.png", "Još nema trudnoćom uzrokovanih promjena. Bazalna temperatura može porasti nakon ovulacije; neki primijete pojačan cervikalni sekret.", (short)2 },
                    { 3, "Oplođena jajna ćelija (zigota) se dijeli i formira blastocistu. Počinje implantacija u sluznicu materice.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development1.png", "Moguće je blago tačkasto krvarenje usljed implantacije. Hormoni hCG i progesteron polako rastu i mogu izazvati umor.", (short)3 },
                    { 4, "Blastocista se čvrsto ugnijezdila. Formiraju se amnion i žumančana vreća; embrij je veličine sjemenke maka.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development4.png", "Test na trudnoću može biti pozitivan. Česti rani znaci su umor, osjetljive dojke i blago kašnjenje menstruacije.", (short)4 },
                    { 5, "Formira se srčana cijev i neuralna cijev iz koje nastaju mozak i kičma. Embrij je dug oko 2 mm.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development5.png", "Mučnine i češće mokrenje su uobičajeni. Hormonske promjene mogu izazvati promjene raspoloženja i osjetljivost mirisa.", (short)5 },
                    { 6, "Srce počinje kucati i moguće je vidjeti aktivnost na UZ. Pojavljuju se pupoljci ruku i nogu; formiraju se oči i uši.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development6.png", "Mučnina ujutro može biti izraženija. Umor, napetost dojki i učestalo mokrenje su česti.", (short)6 },
                    { 7, "Mozak se ubrzano razvija; srce ima četiri šupljine. Lice dobija prepoznatljive crte, a udovi se izdužuju.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development7.png", "Zbog hormona moguća su pojačana salivacija i gađenje prema mirisima. Umor i mučnine obično traju.", (short)7 },
                    { 8, "Prsti su još spojeni opnama; formiraju se pluća, jetra i probavni trakt. Embrij se sve više pokreće.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development8.png", "Mučnine i promjene raspoloženja su česte. Moguće nadimanje, blagi grčevi i osjetljivost dojki.", (short)8 },
                    { 9, "Nestaje embrionalni ‘rep’; formiraju se kapci, vrh nosa i ušni listići. Fetus dobija ljudskiji izgled.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development9.png", "Žgaravica i promjene apetita se mogu pojaviti. Umor je i dalje izražen, a mokrenje češće.", (short)9 },
                    { 10, "Embrij postaje fetus; svi glavni organi su formirani i sazrijevaju. Počinju pokreti koje još ne osjećaš.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development10.png", "Mučnine mogu početi popuštati. Stomak se lagano zaokružuje kako materica raste iznad stidne kosti.", (short)10 },
                    { 11, "Formiraju se nokti i zubi unutar desni. Vanjske genitalije se razvijaju, iako je spol još teško vidjeti UZ-om.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development11.png", "Energija se postepeno vraća. Moguće su nadutost i promjene u probavi.", (short)11 },
                    { 12, "Fetus vježba refleksne pokrete; bubrezi počinju stvarati urin. Proporcije glave i tijela se izjednačavaju.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development12.png", "Mučnine često slabe krajem prvog trimestra. Možda primijetiš manje umora i stabilnije raspoloženje.", (short)12 },
                    { 13, "Kosti počinju očvrsnuti (osifikacija). Fetus može sisati palac i gutati plodovu vodu.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development13.png", "Početak drugog trimestra često donosi više energije. Moguća je žgaravica i začepljen nos.", (short)13 },
                    { 14, "Na koži se pojavljuje lanugo (fina dlaka). Jetra i slezena rade na stvaranju krvnih ćelija.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development14.png", "Raspoloženje se stabilizuje. Možda primijetiš promjene kože i sjajniju kosu.", (short)14 },
                    { 15, "Na UZ se jasnije vide kosti i nos. Formira se uzorak kose na vlasištu.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development15.png", "Bolovi u leđima mogu započeti. Povećava se apetit; neke trudnice osjećaju vrtoglavice.", (short)15 },
                    { 16, "Oči se pomjeraju; uši su funkcionalnije. Počinje razvijanje mišićnog tonusa.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development16.png", "Moguće je osjetiti prve lagane pokrete (posebno kod višerotki). Trbuh se vidljivije zaokružuje.", (short)16 },
                    { 17, "Počinje taloženje masnog tkiva i razvija se koža. Skelet dodatno čvrsti.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development17.png", "Grčevi u nogama i bol u leđima su češći. Možda primijetiš povećan apetit.", (short)17 },
                    { 18, "Uši su na mjestu; fetus čuje zvukove. Pokreti postaju izraženiji i koordinisaniji.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development18.png", "Mogu se javiti vrtoglavica pri naglom ustajanju. Veća težina utiče na držanje.", (short)18 },
                    { 19, "Kožu prekriva vernix (zaštitni sloj). Nervni sistem sazrijeva; osjet dodira je bolji.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development19.png", "Koža trbuha se rasteže i može svrbiti. Strije se ponekad pojavljuju.", (short)19 },
                    { 20, "Vrijeme za detaljan UZ pregled anatomije. Beba se snažnije kreće; spol je obično vidljiv.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development20.png", "‘Quickening’ – pokrete jasno osjećaš. Žgaravica i bol u leđima mogu biti češći.", (short)20 },
                    { 21, "Fetus guta plodovu vodu i stvara mekonij u crijevima. Pojačana kontrola pokreta.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development21.png", "Oticanje stopala i gležnjeva je moguće. Apetit raste; potreba za odmorom veća.", (short)21 },
                    { 22, "Formiraju se obrve i trepavice; osjeti zvuk i vibracije. Proporcije tijela postaju skladnije.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development22.png", "Pritisak materice može uzrokovati bol u karlici. Zadržavanje tečnosti i strije su mogući.", (short)22 },
                    { 23, "Pluća započinju produkciju surfaktanta, ključnog za disanje nakon rođenja. Koža je još naborana.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development23.png", "Kratak dah pri naporu i umor su češći. Moguće su noćne grčeve nogu.", (short)23 },
                    { 24, "Grananje disajnih puteva napreduje; mozak se brzo razvija. Beba reaguje na dodir i zvuk.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development24.png", "Žgaravica i otoci zglobova su češći. Braxton–Hicks kontrakcije se mogu pojaviti.", (short)24 },
                    { 25, "Kičma jača; mišići se razvijaju. Pokreti su ritmičniji, a beba može reagovati na svjetlo.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development25.png", "Nesanica i češće noćno mokrenje. Umor i bol u leđima su izraženiji.", (short)25 },
                    { 26, "Alveole u plućima se formiraju; nervni sistem sazrijeva. Mogu se bilježiti ciklusi sna.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development26.png", "Povremene Braxton–Hicks kontrakcije su normalne. Može se javiti otežano disanje pri naporu.", (short)26 },
                    { 27, "Počinje treći trimestar. Mozak dobija sve složenije veze; pokreti su snažni.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development27.png", "Žgaravica, bol u leđima i umor postaju učestaliji. Apetit može varirati.", (short)27 },
                    { 28, "Oči se otvaraju; beba trepće i reaguje na svjetlo. Rast potkožnog masnog tkiva ubrzava.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development28.png", "Otoci nogu su češći; moguća nelagoda pri spavanju. Kratkoća daha pri hodu.", (short)28 },
                    { 29, "Brže taloženje masti pomaže termoregulaciji nakon rođenja. Mišići jačaju.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development29.png", "Grčevi u listovima i hemoroidi mogu smetati. Braxton–Hicks kontrakcije češće.", (short)29 },
                    { 30, "Mozak razvija brazde i vijuge; pokreti postaju glatkiji. Beba zauzima više prostora.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development30.png", "Učestalije kontrakcije bezbolne prirode. Umor i bol u krstima su izraženiji.", (short)30 },
                    { 31, "Rasporedi spavanja i budnosti su uočljiviji. Pluća i dalje sazrijevaju.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development31.png", "Kratak dah i potreba za češćim odmorom. Nelagoda pri sjedenju i spavanju.", (short)31 },
                    { 32, "Nokti i kosa su duži; beba dobija na težini. Verniks i lanugo i dalje prisutni.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development32.png", "Češće mokrenje zbog pritiska na bešiku. Žgaravica i noćne nelagode su učestale.", (short)32 },
                    { 33, "Majčina antitijela prelaze na bebu i jačaju imunitet. Tonus mišića je bolji.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development33.png", "Otoci ruku i nogu, trnjenje prstiju i nelagoda u karlici su mogući.", (short)33 },
                    { 34, "Pluća i nervni sistem znatno zreliji. Beba vježba disajne pokrete.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development34.png", "Pritisak u karlici i češće kontrakcije. Umor i nesanica se pojačavaju.", (short)34 },
                    { 35, "Stisak šake je snažan; beba dobija na težini. Prostor u materici je sve manji.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development35.png", "Kiselina u želucu i zadihanost su česte. Teže je pronaći udoban položaj za spavanje.", (short)35 },
                    { 36, "Većina beba je okrenuta glavom prema dolje. Pokreti su rjeđi zbog manjka prostora, ali i dalje primjetni.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development36.png", "‘Lightening’ – spuštanje bebe može olakšati disanje, ali pojačati pritisak na bešiku.", (short)36 },
                    { 37, "Rani termin (early term). Organi su funkcionalni, pluća blizu pune zrelosti.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development37.png", "Lažne kontrakcije češće i pravilnije. Povećan vaginalni iscjedak i mogući znakovi skorog poroda.", (short)37 },
                    { 38, "Završno sazrijevanje pluća i nervnog sistema. Beba skladišti energiju i masnoću.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development38.png", "Pritisak nisko u karlici, umor i nesanica. Mogući su znakovi početka poroda.", (short)38 },
                    { 39, "Puni termin (full term). Beba je spremna za rođenje, nastavlja dobivati na težini.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development39.png", "Kontrakcije postaju pravilnije i jače. Moguća pojava ‘bloody show’ i pucanje vodenjaka.", (short)39 },
                    { 40, "Termin; većina organa je potpuno funkcionalna. Beba je spremna za vanjski svijet.", "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development40.png", "Porod može početi u bilo kojem trenutku. Kontrakcije se intenziviraju i postaju učestalije.", (short)40 }
                });

            migrationBuilder.InsertData(
                table: "FoodTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Rižine pahuljice" },
                    { 2, "Zobene pahuljice" },
                    { 3, "Kukuruzne pahuljice (bebe)" },
                    { 4, "Pšenična kaša (bebe)" },
                    { 10, "Mrkva" },
                    { 11, "Tikvica" },
                    { 12, "Batat (slatki krompir)" },
                    { 13, "Bundeva" },
                    { 14, "Brokula" },
                    { 15, "Karfiol" },
                    { 16, "Grašak" },
                    { 17, "Špinat termički obrađen" },
                    { 20, "Jabuka" },
                    { 21, "Kruška" },
                    { 22, "Banana" },
                    { 23, "Breskva" },
                    { 24, "Šljiva" },
                    { 25, "Borovnica" },
                    { 26, "Jagoda" },
                    { 27, "Citrusi (npr. narandža)" },
                    { 30, "Piletina" },
                    { 31, "Puretina" },
                    { 32, "Govedina" },
                    { 33, "Jaja (dobro termički)" },
                    { 34, "Jogurt (punomasni, obični)" },
                    { 35, "Svježi sir (cottage)" },
                    { 36, "Sir blagog okusa" },
                    { 40, "Leća" },
                    { 41, "Grah (crni, crveni)" },
                    { 42, "Slanutak" },
                    { 45, "Kikiriki maslac (razrijeđen)" },
                    { 46, "Badem maslac (razrijeđen)" },
                    { 47, "Orah (mljeven/maslac)" },
                    { 50, "Bijela riba" },
                    { 51, "Losos" },
                    { 52, "Tunjevina (povremeno)" },
                    { 60, "Integralni hljeb (meke kriške)" },
                    { 61, "Tjestenina male forme" },
                    { 62, "Kinoa" },
                    { 63, "Smeđa riža" },
                    { 70, "Krastavac (meke štapiće)" },
                    { 71, "Paradajz kuhan" },
                    { 72, "Kukuruz" },
                    { 80, "Grožđe (četvrtine)" },
                    { 81, "Malina" },
                    { 90, "Med" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Parent" },
                    { 2L, "Doctor" }
                });

            migrationBuilder.InsertData(
                table: "WeeklyAdvices",
                columns: new[] { "Id", "AdviceText", "WeekNumber" },
                values: new object[,]
                {
                    { 1, "Prva sedmica trudnoće računa se od prvog dana posljednje menstruacije. Iako se oplodnja još nije dogodila, tvoje tijelo se već priprema za stvaranje novog života. Počni uzimati folnu kiselinu i jedi hranu bogatu željezom i kalcijem. Pokušaj izbjegavati stres i osiguraj dovoljno sna. Ako planiraš trudnoću, sada je savršeno vrijeme da uvedeš zdrave navike koje će trajati kroz naredne mjesece.", (short)1 },
                    { 2, "Ovo je tjedan kada tvoje tijelo ovulira i priprema se za moguću oplodnju. Održavaj uravnoteženu prehranu, povećaj unos voća, povrća i vode. Izbjegavaj alkohol, cigarete i brzu hranu jer mogu utjecati na kvalitetu jajne stanice. Odmaraj se dovoljno i pokušaj smanjiti stres kroz šetnju ili lagano istezanje. Tvoje tijelo radi važan posao i zaslužuje pažnju i brigu.", (short)2 },
                    { 3, "U ovoj sedmici oplodnja se vjerojatno dogodila, i embrion započinje svoje putovanje prema maternici. Moguće je da još ne znaš da si trudna, ali promjene već počinju. Održavaj hidraciju i izbjegavaj lijekove bez konsultacije s doktorom. Folna kiselina sada ima ključnu ulogu u razvoju nervnog sistema bebe. Iako još ne vidiš promjene, tvoje tijelo već radi čuda.", (short)3 },
                    { 4, "Čestitamo! Tvoj test na trudnoću može pokazati pozitivan rezultat. Hormoni trudnoće počinju naglo rasti, pa možeš osjećati blagu mučninu, osjetljivost dojki i umor. Jednostavni, česti obroci pomoći će ti da se osjećaš bolje. Izbjegavaj kofein i počni planirati svoj prvi prenatalni pregled. Uživaj u ovom posebnom trenutku — tek počinje jedno divno putovanje.", (short)4 },
                    { 5, "Tvoje tijelo sada stvara osnovu za posteljicu, a embrion raste brže nego ikad. Možda osjećaš mučninu, česte promjene raspoloženja ili blagu vrtoglavicu. Odmaraj se i jedi male, nutritivno bogate obroke. Ako ti neki mirisi smetaju, pokušaj izbjegavati jake arome. Ovo je vrijeme kada tvoje tijelo uči kako podržati novi život — budi nježna prema sebi.", (short)5 },
                    { 6, "U šestoj sedmici beba je veličine zrna graška i počinju se formirati crte lica. Hormoni trudnoće mogu uzrokovati umor i promjene apetita. Spavaj kad god osjetiš potrebu i unosi dovoljno tekućine. Izbjegavaj sirove namirnice i brini o higijeni hrane. Lagane šetnje na svježem zraku mogu pomoći da se osjećaš bolje i odmornije.", (short)6 },
                    { 7, "Srce tvoje bebe kuca stabilnim ritmom, a razvoj organa se ubrzava. Možda ćeš osjetiti češće mokrenje i promjene raspoloženja. Jedi često, ali u manjim porcijama i drži grickalice pri ruci. Unosi dovoljno kalcija i magnezija kroz mliječne proizvode ili zamjene. Ako ti se pojavi mučnina, pokušaj piti male gutljaje vode tokom dana.", (short)7 },
                    { 8, "Vrijeme je za tvoj prvi pregled kod ginekologa! Doktor će potvrditi trudnoću i napraviti osnovne testove. Tvoje tijelo prolazi kroz hormonalne promjene, pa su emotivne oscilacije normalne. Nastavi s unosom folne kiseline i zdravom prehranom. Počni razmišljati o dnevnim ritualima opuštanja — tvoj mir sada znači i mir tvoje bebe.", (short)8 },
                    { 9, "Bebini prsti i nožni prsti sada postaju vidljivi, a glava joj se oblikuje. Tvoje tijelo se širi, pa možeš osjetiti nelagodu u grudima ili blage grčeve. Lagano istezanje i topla kupka mogu pomoći. Ne zaboravi piti dovoljno vode i jesti vlaknaste namirnice za bolju probavu. Emotivne promjene su sasvim prirodne — daj sebi dozvolu da osjećaš sve.", (short)9 },
                    { 10, "Tvoje dijete je sada veličine šljive i svi osnovni organi su formirani. Možda primjećuješ promjene u kosi i koži zbog hormona. Nastavi s balansiranom ishranom i izbjegavaj previše slatkog. Kratke šetnje i svjež zrak pomoći će ti da se osjećaš energičnije. Uživaj u svakoj maloj promjeni jer tvoje tijelo radi nevjerovatan posao.", (short)10 },
                    { 11, "Mučnine bi sada mogle početi slabiti, a energija se polako vraća. Beba je duga oko 4 cm i kreće se unutar maternice. U prehranu uvedi više svježeg povrća, voća i integralnih žitarica. Izbjegavaj duže stajanje i odmori noge kad god možeš. Ako planiraš put, obavezno se posavjetuj s ljekarom.", (short)11 },
                    { 12, "Završava prvo tromjesečje – čestitamo! Rizik od pobačaja sada je značajno manji. Mnoge žene osjećaju olakšanje i stabilnije raspoloženje. Nastavi s blagim fizičkim aktivnostima poput joge ili laganih šetnji. Tvoje tijelo sada sjaji i pokazuje prve znakove trudnoće.", (short)12 },
                    { 13, "Ulaziš u drugo tromjesečje – najugodniji dio trudnoće. Beba raste brzo, a ti se vjerojatno osjećaš energičnije. Počni planirati zdravije obroke bogate proteinima i vlaknima. Hidratacija je i dalje važna, pa uvijek imaj bocu vode uz sebe. Uživaj u ovom razdoblju dok se tvoje tijelo prilagođava s lakoćom.", (short)13 },
                    { 14, "Bebino lice se formira, a počinje i razvoj fine kose na tijelu. Možda primjećuješ povećan apetit — iskoristi to da unosiš više zdravih namirnica. Ako imaš problema sa spavanjem, pokušaj spavati na lijevom boku. Emocionalna stabilnost se vraća, pa je ovo idealno vrijeme za blagu fizičku aktivnost. Prati svoje tijelo, ono zna šta mu treba.", (short)14 },
                    { 15, "Tvoje tijelo se sada prilagođava rastu maternice. Možda primjećuješ tamnije linije na stomaku – to je normalna pigmentacija. Beba reaguje na zvukove, pa joj možeš pričati ili pustiti laganu muziku. U prehranu dodaj namirnice bogate željezom i vitaminom D. Nastavi se kretati, ali izbjegavaj teške vježbe.", (short)15 },
                    { 16, "Beba sada teži oko 100 grama i kreće se aktivno. Možda osjetiš prve lagane pokrete koji podsjećaju na leptiriće. Uživaj u tom osjećaju povezanosti s bebom. Ako se javlja bol u leđima, koristi jastuke za potporu i spavaj na boku. Slušaj svoje tijelo i odmori kada ti zatreba.", (short)16 },
                    { 17, "Tvoje srce pumpa više krvi kako bi podržalo bebu, pa se možeš osjećati umorno. Jedi male, nutritivne obroke i izbjegavaj duga stajanja. Beba počinje čuti tvoj glas, pa s njom slobodno razgovaraj. Uključi partnera u pripreme i dijelite trenutke zajedno. Osjećaj umora je normalan – odmori su sada tvoja obaveza.", (short)17 },
                    { 18, "Vrijeme je za detaljan ultrazvuk! Ljekar će provjeriti razvoj organa tvoje bebe. Možda osjetiš bol u leđima zbog promjene držanja, pa pazi na ergonomiju. Počni razmišljati o trudničkim vježbama koje jačaju mišiće zdjelice. Uživaj u svakom pokretu koji osjetiš – tvoja beba raste i jača svakog dana.", (short)18 },
                    { 19, "Tvoja beba sada ima razvijene organe i počinje akumulirati masno tkivo. Možda se javlja žgaravica, pa jedi manje obroke češće. Pij dovoljno tekućine, ali izbjegavaj gazirane napitke. Masaža nogu može pomoći kod oticanja. Nastavi redovno šetati i održavaj pozitivan duh.", (short)19 },
                    { 20, "Polovina trudnoće – bravo! Beba sada teži oko 300 grama i ti si vjerovatno već osjetila prve jače pokrete. Prati držanje tijela i koristi jastuk za potporu. Ovo je pravo vrijeme da se pripremiš na promjene rasporeda sna. Uživaj u ovom posebnom periodu, jer svaki dan tvoje dijete postaje jače.", (short)20 },
                    { 21, "Bebine kosti postaju čvršće, a pokreti energičniji. Osjećaj težine u nogama može se pojačati – zato odmaraj s nogama podignutim. Jedi dovoljno kalcija i bjelančevina. Hidratacija i lagane šetnje pomažu cirkulaciji. Održavaj vedar duh i povezanost s bebom.", (short)21 },
                    { 22, "Beba već može čuti zvukove iz okoline i reagirati na glasove. Ako osjećaš bol u leđima, istezanje i topla kupka mogu pomoći. U ovom periodu možeš početi birati ime za bebu – uživaj u tom procesu. Jedi hranu bogatu vlaknima i pij dovoljno vode. Emocionalna stabilnost sada ti pomaže da uživaš u trudnoći punim plućima.", (short)22 },
                    { 23, "Tvoje tijelo nosi dodatnu težinu, pa pazi na pravilno držanje. Možeš osjetiti žgaravicu ili otečene zglobove, što je uobičajeno. Lagane šetnje i masaže pomažu u opuštanju. Beba već ima razvijen ritam sna i budnosti. Prati signale svog tijela i ne forsiraj se.", (short)23 },
                    { 24, "Ulaziš u šesti mjesec trudnoće i beba počinje prepoznavati tvoj glas. Održavaj unos tekućine i hranu bogatu proteinima. Ako se javlja grč u nogama, dodaj više magnezija u prehranu. Pokušaj spavati na lijevom boku radi bolje cirkulacije. Osloni se na podršku partnera i porodice.", (short)24 },
                    { 25, "Bebin sluh se poboljšava, pa joj možeš pjevati i pričati. Povećava se potreba za željezom – unosi zeleno povrće i orahe. Ako imaš problema sa spavanjem, kreiraj mirnu večernju rutinu. Započni s pripremama za dolazak bebe polako i bez stresa. Svaka tvoja emocija utiče i na nju, zato se fokusiraj na mir i radost.", (short)25 },
                    { 26, "Tvoje tijelo sada osjeća težinu maternice i promjene držanja. Pazi na stopala i nosi udobnu obuću. U prehranu uvedi više vlakana kako bi spriječila zatvor. Opuštanje uz muziku i disanje pomoći će kod napetosti. Svaki dan približava te trenutku susreta s bebom.", (short)26 },
                    { 27, "Treće tromjesečje počinje! Beba brzo dobija na težini i ti možeš osjećati češću potrebu za odmorom. Redovno se proteži i mijenjaj položaj tijela. Jedi lagano i često kako bi spriječila žgaravicu. Tvoj trudnički sjaj je sada u punom sjaju – ponosi se sobom.", (short)27 },
                    { 28, "Vrijeme je za kontrolu nivoa šećera u krvi. Ako osjećaš umor, ne oklijevaj da odmoriš. Pij dovoljno vode i jedi sezonsko voće. Beba sada prepoznaje svjetlost i reaguje na dodir kroz stomak. Opusti se uz laganu muziku ili meditaciju.", (short)28 },
                    { 29, "Tvoje tijelo se priprema za porod, pa možeš osjetiti blage kontrakcije. Povećaj unos proteina i kalcija. Masaža leđa može pomoći kod nelagode. Razgovaraj s partnerom o planovima za dolazak bebe. Svaki pokret tvoje bebe sada je znak njenog zdravog razvoja.", (short)29 },
                    { 30, "Trbuh je sve izraženiji, a beba zauzima više prostora. Diši polako i duboko da olakšaš pritisak na pluća. U prehranu dodaj više vlakana i tekućine. Počni pripremati torbu za porod i osnovne stvari za bebu. Uživaj u svakom pokretu i povezanosti koju osjećate.", (short)30 },
                    { 31, "Beba sada prepoznaje tvoj glas i reaguje na emocije. Moguća je nesanica zbog veličine stomaka – koristi jastuke za udobnost. Jedi manje obroke, ali češće. Pokušaj svaki dan šetati i istezati se. Održavaj kontakt s doktorom i ne zanemaruj znakove umora.", (short)31 },
                    { 32, "Beba brzo dobija na težini, a tvoje tijelo se priprema za porod. Odmaraj često i pazi na unos soli kako bi spriječila oticanje. Diši polako i koristi vježbe disanja. Ako još nisi, počni planirati porodilište. Uživaj u svakom trenutku trudnoće – kraj se bliži!", (short)32 },
                    { 33, "Maternica se dodatno širi, pa možeš osjećati pritisak na leđa i noge. Lagano istezanje i masaža pomažu u opuštanju. Jedi lagane obroke i izbjegavaj teške začine. Tvoja beba sada vježba disanje u stomaku. Polako pripremaj svoj dom za njen dolazak.", (short)33 },
                    { 34, "Beba zauzima gotovo cijelu maternicu i njeni pokreti su sada snažniji nego ikad. Možda se pojavi nesanica jer je sve teže pronaći udoban položaj za spavanje. Pokušaj spavati na lijevom boku i koristi jastuke za potporu stomaku i leđima. U ovoj fazi često se javlja umor, pa slušaj svoje tijelo i odmori kad god osjetiš potrebu. Uživaj u svakom trenutku trudnoće i mirno se pripremaj za dolazak svoje bebe.", (short)34 },
                    { 35, "Beba sada teži oko 2,5 kilograma i priprema se za završne faze razvoja. Tvoje tijelo se može osjećati teže i umornije, pa se više odmaraj. Ako imaš problema sa spavanjem, pokušaj spavati na lijevom boku i koristi dodatne jastuke. Pripremi torbu za porod i provjeri plan transporta do bolnice. Ovo je dobro vrijeme da se posvetiš sebi i mirno dočekaš posljednje sedmice trudnoće.", (short)35 },
                    { 36, "Tvoja beba je gotovo spremna za susret s tobom. Možda primjećuješ češće kontrakcije i osjećaj težine u karlici. Diši duboko i pokušaj ostati aktivna kroz lagane šetnje. Jedi lagano i izbjegavaj masnu hranu kako bi smanjila žgaravicu. Posljednji pregledi su važni, pa ih redovno obavljaj i slušaj savjete svog ljekara.", (short)36 },
                    { 37, "Zvanično si u terminskoj trudnoći – beba može doći bilo kada! Ako osjećaš pritisak u donjem dijelu stomaka, to znači da se spušta u položaj za porod. Pripremi torbu, dokumente i neophodne stvari za bolnicu. Pokušaj ostati smirena i odmaraj više tokom dana. Svaka kontrakcija sada te korak po korak vodi prema najljepšem trenutku – rođenju tvoje bebe.", (short)37 },
                    { 38, "Tvoje tijelo se potpuno priprema za porod, a beba sada vježba disanje i sisanje u stomaku. Možeš osjetiti bol u leđima i umor – to je normalno. Slušaj svoje tijelo i ne forsiraj se. Pokušaj se opustiti uz topli tuš, laganu muziku ili meditaciju. Tvoj mir sada znači i mir tvoje bebe koja uskoro stiže.", (short)38 },
                    { 39, "Sve je spremno za porođaj! Beba je gotovo iste veličine kao pri rođenju i zauzima svoj položaj. Možda osjećaš učestale Braxton-Hicks kontrakcije, znak da se tvoje tijelo priprema. Pij dovoljno vode i jedi lagano kako bi imala snage za porođaj. Emocije su sada jake – strpljenje i smirenost su tvoja najveća snaga.", (short)39 },
                    { 40, "Stigla si do kraja trudnoće – čestitamo! Svaki dan sada može biti onaj kada ćeš upoznati svoju bebu. Odmori se, jedi lagano i pokušaj se opustiti što više možeš. Ako još ne osjećaš kontrakcije, to je u redu – svaka trudnoća ima svoj ritam. Uživaj u posljednjim trenucima trudnoće i pripremi se za najposebniji susret u svom životu.", (short)40 }
                });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "DateOfBirth", "Email", "FirstName", "Gender", "IdentityUserId", "LastName", "PhoneNumber", "RoleId" },
                values: new object[,]
                {
                    { 1L, new DateTime(1998, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "parent@nestly.com", "TestParent", "Female", "b5b77b5d-65b6-4f32-93f4-3f76b14e6f3c", "User", "+38761000000", 1L },
                    { 2L, new DateTime(1990, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "doctor@nestly.com", "TestDoctor", "Female", "work7b5d-65b6-4f32-93f4-126sko5e6f3c", "User", "+38762000000", 2L }
                });

            migrationBuilder.InsertData(
                table: "MealRecommendations",
                columns: new[] { "Id", "FoodTypeId", "WeekNumber" },
                values: new object[,]
                {
                    { 1L, 1, (short)24 },
                    { 2L, 10, (short)24 },
                    { 3L, 11, (short)25 },
                    { 4L, 20, (short)26 },
                    { 5L, 21, (short)26 },
                    { 6L, 22, (short)27 },
                    { 7L, 2, (short)27 },
                    { 8L, 12, (short)28 },
                    { 9L, 13, (short)28 },
                    { 10L, 14, (short)29 },
                    { 11L, 15, (short)29 },
                    { 12L, 34, (short)30 },
                    { 13L, 33, (short)31 },
                    { 14L, 45, (short)32 },
                    { 15L, 30, (short)32 },
                    { 16L, 31, (short)34 },
                    { 17L, 40, (short)35 },
                    { 18L, 42, (short)36 },
                    { 19L, 16, (short)36 },
                    { 20L, 61, (short)37 },
                    { 21L, 23, (short)38 },
                    { 22L, 24, (short)39 },
                    { 23L, 32, (short)40 },
                    { 24L, 35, (short)42 },
                    { 25L, 25, (short)44 },
                    { 26L, 27, (short)52 },
                    { 27L, 71, (short)52 },
                    { 28L, 50, (short)54 },
                    { 29L, 60, (short)56 },
                    { 30L, 26, (short)58 },
                    { 31L, 46, (short)60 },
                    { 32L, 41, (short)62 },
                    { 33L, 62, (short)64 },
                    { 34L, 51, (short)68 },
                    { 35L, 63, (short)70 },
                    { 36L, 70, (short)72 },
                    { 37L, 36, (short)76 },
                    { 38L, 72, (short)80 },
                    { 39L, 80, (short)82 },
                    { 40L, 90, (short)90 },
                    { 41L, 47, (short)92 },
                    { 42L, 52, (short)100 },
                    { 43L, 81, (short)104 }
                });

            migrationBuilder.InsertData(
                table: "DoctorProfiles",
                columns: new[] { "Id", "UserId" },
                values: new object[] { 1L, 2L });

            migrationBuilder.InsertData(
                table: "ParentProfiles",
                columns: new[] { "Id", "UserId" },
                values: new object[] { 1L, 1L });

            migrationBuilder.InsertData(
                table: "BlogPosts",
                columns: new[] { "Id", "AuthorId", "Content", "CreatedAt", "ImageUrl", "Phase", "Title", "UpdatedAt", "WeekFrom", "WeekTo" },
                values: new object[,]
                {
                    { 1L, 1L, "Umor i izražena pospanost često su prvi znak trudnoće, čak i prije izostanka menstruacije. Mnoge žene primijete pojačanu osjetljivost ili bol u grudima, što je posljedica hormonalnih promjena. Rani hormonski disbalans može uzrokovati promjene raspoloženja, razdražljivost ili neočekivane emotivne reakcije. Kod nekih se javlja pojačan ili potpuno promijenjen apetit, kao i iznenadna odbojnost prema mirisu ili ukusu hrane koju su ranije voljele. Metalni ukus u ustima je još jedan čest, ali manje poznat simptom ranih sedmica trudnoće. Blage grčeve u donjem dijelu stomaka žene često pogrešno povežu s dolaskom menstruacije, iako mogu biti znak implantacije. Osjetljivost na mirise može biti toliko izražena da parfemi, hrana ili miris kuće postaju neugodni. Mučnina se ne javlja uvijek samo ujutro, već može trajati tokom cijelog dana ili u talasima. Pojačana potreba za mokrenjem može se pojaviti rano zbog promjena u cirkulaciji i hormona. Važno je osluškivati svoje tijelo i, uz kombinaciju više simptoma, uraditi test na trudnoću i javiti se ljekaru radi potvrde. Rano prepoznavanje simptoma omogućava pravovremenu brigu o ishrani, suplementima i zdravlju mame i bebe.", new DateTime(2026, 4, 5, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2583), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage1.png", 1, "Prvi simptomi trudnoće koje možda niste očekivali", null, 1, 12 },
                    { 2L, 1L, "Prvi prenatalni pregled je ključan korak u praćenju trudnoće i obično se zakazuje između 8. i 10. sedmice. Prije pregleda zapišite datum posljednje menstruacije jer će ljekar na osnovu toga procijeniti gestacijsku dob. Sastavite listu lijekova, suplemenata i hroničnih stanja koja imate kako bi ljekar procijenio sigurnost terapije. Pripremite pitanja o prehrani, dozvoljenoj fizičkoj aktivnosti, putovanjima i simptomima koji vas brinu. Na pregledu se obično rade laboratorijske analize krvi i urina kako bi se provjerilo opće stanje organizma i vrijednosti važnih parametara. Ljekar može uraditi i ultrazvuk kako bi provjerio razvoj ploda i prisustvo otkucaja srca. Nemojte se ustručavati pričati o mučnini, umoru, strahovima ili emocionalnim promjenama, jer sve to spada u važan dio anamneze. Preporučuje se da sa sobom povedete partnera ili blisku osobu koja vam pruža podršku i može zapamtiti informacije umjesto vas. Zapišite preporuke koje dobijete, poput folne kiseline, unosa tečnosti i izbjegavanja određenih namirnica. Redovni prenatalni pregledi kasnije će pomoći da se potencijalni problemi otkriju na vrijeme i trudnoća prati sigurno. Ovaj prvi korak postavlja temelje povjerenja između vas i vašeg doktora, što je od velikog značaja za cijeli period trudnoće.", new DateTime(2026, 4, 8, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2590), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage2.png", 1, "Kako se pripremiti za prvi prenatalni pregled", null, 1, 12 },
                    { 3L, 1L, "Koža novorođenčeta je tanka, osjetljiva i još uvijek prilagođava vanjskom okruženju. Kupanje bebe 2 do 3 puta sedmično u mlakoj vodi sasvim je dovoljno, dok je svakodnevno umivanje lica i pregiba blago vodom preporučljivo. Birajte blage, bezmirisne preparate bez agresivnih hemikalija, parabena i jakih mirisa. Nakon kupanja kožu samo lagano tapkajte mekanim peškirom umjesto trljanja kako biste izbjegli iritaciju. Posebnu pažnju obratite na pregibe vrata, pazuha, iza ušiju i pelensku regiju, gdje se znoj i vlaga zadržavaju. Za pelensku regiju koristite zaštitne kreme koje stvaraju barijeru, naročito ako primijetite crvenilo. Izbjegavajte pretjeranu upotrebu pudera ili parfemisanih proizvoda jer mogu začepiti pore i nadražiti kožu. Ako se pojave suhe mrlje ili blagi osip, često je dovoljno blago hidratantno ulje ili krema preporučena od pedijatra. Uvijek oblačite bebu u pamučnu odjeću koja diše i ne grebe kožu. Ako primijetite jače crvenilo, mjehuriće, žutilo ili bebu koja je izrazito nemirna na dodir, obavezno se javite pedijatru. Njega kože novorođenčeta je jednostavna kada se vodi računa o čistoći, blagim preparatima i izbjegavanju nepotrebnih proizvoda.", new DateTime(2026, 4, 10, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2592), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage3.png", 2, "Kako njegovati kožu novorođenčeta", null, 1, 16 },
                    { 4L, 1L, "Kolike su čest razlog zabrinutosti roditelja u prvim mjesecima života bebe. Tipično se javljaju kao epizode intenzivnog plača koje traju više od tri sata dnevno, najmanje tri dana u sedmici. Beba često privlači nožice prema stomaku, stišće šačice i izgleda kao da je u grču. Plač se najčešće pojačava u popodnevnim ili večernjim satima, i teško ga je smiriti uobičajenim metodama. Uzrok kolika nije u potpunosti razjašnjen, ali se povezuje s nezrelim probavnim sistemom i osjetljivošću na stimulaciju. Pomažu blago nošenje bebe uspravno, kontakt koža na kožu i nježno ljuljanje. Masaža stomaka kružnim pokretima u smjeru kazaljke na satu može olakšati otpuštanje gasova. Provjerite odgovara li bebi mlijeko, položaj pri hranjenju i tempo hranjenja. Ako beba slabo napreduje, povraća, ima temperaturu ili promjene u stolici, odmah potražite ljekarsku pomoć jer to nije tipično za obične kolike. Iako su iscrpljujuće, kolike obično prolaze spontano do trećeg ili četvrtog mjeseca života. Roditeljima je važno znati da kolike nisu odraz njihove greške niti loše brige o bebi.", new DateTime(2026, 4, 11, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2594), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage4.png", 2, "Kako prepoznati kolike kod beba", null, 1, 12 },
                    { 5L, 1L, "Prvo tromjesečje je period intenzivnog razvoja embriona, pa kvalitet ishrane ima veliku važnost. Folna kiselina je ključna za razvoj nervnog sistema bebe, pa u ishranu uvrstite zeleno lisnato povrće, citrusno voće i integralne žitarice. Namirnice bogate željezom, poput crvenog mesa u umjerenim količinama, jaja, leće i špinata, pomažu u sprječavanju anemije. Proteini iz jaja, piletine, ribe s niskim udjelom žive i mliječnih proizvoda važni su za rast tkiva. Ako imate mučnine, birajte manje, ali češće obroke, suhe krekere, banane i blage juhe. Hidratacija je jednako važna – pijte vodu, biljne čajeve koji su dozvoljeni u trudnoći i izbjegavajte zaslađene napitke. Smanjite unos kofeina i izbjegavajte alkohol potpuno. Neoprano povrće, sirova jaja, nepasterizirani sirevi i nedovoljno termički obrađeno meso mogu biti izvor infekcija koje su rizične u trudnoći. Ako ne možete unijeti dovoljno nutrijenata hranom, sa ginekologom dogovorite adekvatan prenatalni suplement. Slušajte svoje tijelo, ali pokušajte svaku žudnju uklopiti u što zdraviji izbor. Male promjene u prehrani već u prvom tromjesečju dugoročno doprinose zdravlju i mame i bebe.", new DateTime(2026, 4, 13, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2596), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage5.png", 1, "Namirnice koje pomažu u prvom tromjesečju", null, 1, 12 },
                    { 6L, 1L, "Period dojenja zahtijeva dodatnu energiju i nutrijente kako bi se podržala proizvodnja mlijeka i oporavak tijela. Preporučuje se raznovrsna ishrana koja uključuje svježe voće, povrće, integralne žitarice i kvalitetne proteine. Ribe bogate omega-3 masnim kiselinama, poput lososa i sardine, doprinose razvoju bebinog mozga i nervnog sistema. Mlijeko, jogurt, sir i druge namirnice bogate kalcijem pomažu očuvanju zdravlja kostiju majke. Važno je unositi dovoljno tečnosti – voda, blagi čajevi i supice su dobar izbor. Neke bebe mogu reagovati na određene namirnice (npr. vrlo začinjenu hranu ili velike količine kofeina), pa pratite kako se beba ponaša nakon podoja. Umjesto striktnih dijeta, fokus stavite na balansirane obroke raspoređene tokom dana. Užine poput orašastih plodova, svježeg voća, integralnih krekera i humusa mogu pomoći da zadržite energiju. Preskakanje obroka može utjecati na nivo energije i raspoloženje, pa planirajte jednostavne, ali nutritivno bogate kombinacije. Ako imate dileme o određenim namirnicama ili ste vegan/vegetarijanac, konsultujte nutricionistu ili ljekara. Briga o vlastitoj ishrani je ujedno i briga o kvalitetu vremena koje provodite sa svojom bebom.", new DateTime(2026, 4, 15, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2598), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage6.png", 2, "Zdravi obroci za dojilje", null, 1, 12 },
                    { 7L, 1L, "Prvi dani s bebom donose mješavinu sreće, umora, straha i ogromne odgovornosti. Normalno je osjećati nesigurnost, posebno ako vam je ovo prvo dijete. Pokušajte spavati kad god i koliko god beba spava, čak i ako to znači kraće drijemke tokom dana. Prihvatite pomoć porodice i prijatelja za kućne obaveze, kuhanje ili nabavku. Ne ustručavajte se govoriti partneru kako se osjećate i šta vam treba. Postepeno upoznajete ritam svoje bebe – način plača, signale gladi, umora ili nelagode. Nemojte se porediti s idealiziranim prikazima majčinstva na društvenim mrežama. Ako osjećate izraženu tugu, bezvoljnost ili se teško povezujete s bebom, razgovarajte s patronažnom sestrom ili ljekarom. Male svakodnevne rutine, poput kupanja, maženja i kontakta koža na kožu, jačaju vašu povezanost. Zapamtite da ne postoji savršena mama; postoji dovoljno dobra mama koja voli, brine se i uči iz dana u dan. Dajte sebi vremena da se prilagodite novoj ulozi i budite nježni prema sebi.", new DateTime(2026, 4, 17, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2599), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage7.png", 2, "Kako se prilagoditi prvim danima s bebom", null, 1, 12 },
                    { 8L, 1L, "Nakon poroda, partner ima ključnu ulogu u pružanju emocionalne i praktične podrške. Preuzimanje dijela kućnih obaveza pomaže majci da se odmori i oporavi fizički. Partner može učestvovati u kupanju bebe, presvlačenju i uspavljivanju, čime gradi snažnu povezanost s djetetom. Otvorena komunikacija o umoru, strahovima i očekivanjima sprečava nagomilavanje tenzija. Važno je priznati da su promjene intenzivne i za partnera, ali da zajednički pristup olakšava period prilagodbe. Podrška u dojenju može biti jednostavna kao donošenje vode, jastuka ili stvaranje mirne atmosfere. Partner treba prepoznati znakove iscrpljenosti ili postporođajne depresije i ohrabriti majku da potraži pomoć. Vrijeme jedan-na-jedan s bebom osnažuje samopouzdanje partnera u brizi za dijete. Zajedničke odluke o rutini spavanja, posjetama i obavezama smanjuju nesporazume. Uloga partnera nije samo pomoć, već ravnopravan dio roditeljskog tima koji čuva dobrobit cijele porodice.", new DateTime(2026, 4, 20, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2601), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage8.png", 2, "Uloga partnera nakon poroda", null, 1, 12 },
                    { 9L, 1L, "Baby blues je česta pojava u prvim danima nakon poroda i pogađa veliki broj majki. Manifestuje se kao plačljivost, nagle promjene raspoloženja, osjetljivost i osjećaj preopterećenosti. Ovi simptomi obično počinju nekoliko dana nakon poroda i prolaze unutar dvije sedmice. Postporođajna depresija je ozbiljnije stanje koje traje duže i može uključivati osjećaj bezvrijednosti, beznađa ili gubitak interesa za svakodnevne aktivnosti. Majka može imati poteškoće u povezivanju s bebom, osjećaj krivnje ili strah da nije dovoljno dobra. Ponekad su prisutne smetnje sna i apetita koje nisu samo posljedica zahtjeva oko bebe. Ako ovi osjećaji traju duže od dvije sedmice ili se pojačavaju, važno je potražiti stručnu pomoć. Razgovor s partnerom, porodicom i medicinskim osobljem prvi je korak ka podršci. Postoji efikasan tretman kroz psihoterapiju, podršku i, po potrebi, medikamentoznu terapiju. Traženje pomoći nije znak slabosti, nego hrabrosti i brige za sebe i svoju porodicu. Svaka majka zaslužuje podršku u ovom osjetljivom periodu, bez osude.", new DateTime(2026, 4, 22, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2603), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage9.png", 2, "Baby blues ili postporođajna depresija?", null, 1, 12 },
                    { 10L, 1L, "Trudnoća je predivan, ali često i stresan period zbog fizičkih promjena i briga o zdravlju bebe. Jednostavne vježbe dubokog disanja pomažu u smanjenju napetosti i smirivanju nervnog sistema. Kratke vođene meditacije ili molitva mogu pružiti osjećaj sigurnosti i fokusa. Prenatalna joga, prilagođena trudnicama, jača tijelo i poboljšava fleksibilnost bez pretjeranog opterećenja. Šetnje na svježem zraku doprinose boljoj cirkulaciji, snu i raspoloženju. Topla kupka (ne prevruća) može ublažiti bol u leđima i opustiti mišiće. Važno je smanjiti izloženost negativnim vijestima i komentarima koji pojačavaju strah. Razgovor s partnerom ili bliskom osobom o brigama često donosi olakšanje. Organizacija dana s malim ritualima opuštanja pomaže u stvaranju osjećaja kontrole. Ako se anksioznost pojačava, ometa san ili svakodnevno funkcionisanje, razgovarajte s ljekarom ili psihologom. Briga o mentalnom zdravlju u trudnoći jednako je važna kao i briga o fizičkom zdravlju.", new DateTime(2026, 4, 24, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2604), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage10.png", 1, "Tehnike opuštanja u trudnoći", null, 13, 27 },
                    { 11L, 1L, "Većina beba počinje samostalno sjediti između šestog i osmog mjeseca života. Prije toga prolaze kroz faze jačanja mišića vrata, leđa i core-a, posebno kroz igru na stomaku. Ponudite bebi siguran prostor na podu, umjesto da je predugo držite u ljuljama ili ležaljkama. Možete je blago poduprijeti jastucima sa strane dok uči balansirati. Nemojte forsirati sjedenje stavljanjem bebe u položaj za koji još nema snage. Svaka beba ima svoj ritam razvoja i poređenje s drugima može stvoriti nepotreban stres. Ako beba ne pokazuje pokušaje podizanja gornjeg dijela tijela ili kontrole glave nakon nekoliko mjeseci, konsultujte pedijatra. Podstičite igru s igračkama ispred bebe kako bi se prirodno naginjala naprijed i aktivirala mišiće. Pohvalite svaki mali napredak jer pozitivna interakcija jača bebino samopouzdanje. Sjedanje je važna prekretnica koja otvara nove mogućnosti istraživanja svijeta oko sebe. Uz strpljenje i sigurnu okolinu, beba će do ovog koraka doći u svoje vrijeme.", new DateTime(2026, 4, 26, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2606), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage11.png", 2, "Kada beba počinje sjediti?", null, 13, 24 },
                    { 12L, 1L, "Razvoj govora počinje mnogo prije nego što dijete izgovori prve riječi. Već od rođenja, pričajte s bebom, imenujte predmete i opisujte šta radite tokom dana. Čitanje slikovnica, čak i vrlo jednostavnih, pomaže razvoju rječnika i pažnje. Pjevanje pjesmica i brojalica uči dijete ritmu, ponavljanju i novim riječima. Reagujte na bebine glasove, osmijeh i gestove kao da vodite pravi razgovor. Izbjegavajte prekomjernu upotrebu ekrana, posebno u najranijoj dobi, jer utiče na kvalitet interakcije. Postavljajte jednostavna pitanja poput 'Gdje je lopta?' i ohrabrite dijete da pokaže ili izgovori. Ne ispravljajte grubo pogrešan izgovor, već ponovite riječ ispravno i prirodno u rečenici. Ako dijete do 18 mjeseci ne izgovara nijednu riječ ili vrlo malo razumije, posavjetujte se s pedijatrom ili logopedom. Svako dijete napreduje svojim tempom, ali bogata, topla komunikacija uvijek je najbolji podsticaj. Roditeljska blizina, strpljenje i igra ključni su saveznici u razvoju govora.", new DateTime(2026, 4, 28, 7, 38, 37, 203, DateTimeKind.Utc).AddTicks(2608), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage12.png", 2, "Podsticanje govora kod mališana", null, 13, 24 }
                });

            migrationBuilder.InsertData(
                table: "Pregnancies",
                columns: new[] { "Id", "CycleLengthDays", "DueDate", "LmpDate", "ParentProfileId" },
                values: new object[] { 1L, 28, new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L });

            migrationBuilder.InsertData(
                table: "BabyProfiles",
                columns: new[] { "Id", "BabyName", "BirthDate", "Gender", "ParentProfileId", "PregnancyId" },
                values: new object[] { 1L, "Emma", new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Female", 1L, 1L });

            migrationBuilder.InsertData(
                table: "BlogPostCategories",
                columns: new[] { "CategoryId", "PostId" },
                values: new object[,]
                {
                    { 1, 1L },
                    { 1, 2L },
                    { 2, 3L },
                    { 2, 4L },
                    { 3, 5L },
                    { 3, 6L },
                    { 4, 7L },
                    { 4, 8L },
                    { 5, 9L },
                    { 5, 10L },
                    { 6, 11L },
                    { 6, 12L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Email",
                table: "AppUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_IdentityUserId",
                table: "AppUsers",
                column: "IdentityUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_RoleId",
                table: "AppUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_BabyGrowths_BabyId_WeekNumber",
                table: "BabyGrowths",
                columns: new[] { "BabyId", "WeekNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BabyProfiles_ParentProfileId_BirthDate",
                table: "BabyProfiles",
                columns: new[] { "ParentProfileId", "BirthDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BabyProfiles_PregnancyId",
                table: "BabyProfiles",
                column: "PregnancyId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogCategories_Name",
                table: "BlogCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostCategories_CategoryId",
                table: "BlogPostCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostInteractions_UserId_PostId",
                table: "BlogPostInteractions",
                columns: new[] { "UserId", "PostId" });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_AuthorId",
                table: "BlogPosts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_CreatedAt",
                table: "BlogPosts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_BabyId_StartAt",
                table: "CalendarEvents",
                columns: new[] { "BabyId", "StartAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_UserId",
                table: "CalendarEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatConversations_ParentProfileId",
                table: "ChatConversations",
                column: "ParentProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatConversations_User1Id_User2Id",
                table: "ChatConversations",
                columns: new[] { "User1Id", "User2Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatConversations_User2Id",
                table: "ChatConversations",
                column: "User2Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ConversationId",
                table: "ChatMessages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SenderId",
                table: "ChatMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_DiaperLogs_BabyId_ChangeDate",
                table: "DiaperLogs",
                columns: new[] { "BabyId", "ChangeDate" });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorProfiles_UserId",
                table: "DoctorProfiles",
                column: "UserId",
                unique: true);

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
                name: "IX_FoodTypes_Name",
                table: "FoodTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthEntries_BabyId_EntryDate",
                table: "HealthEntries",
                columns: new[] { "BabyId", "EntryDate" });

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_BabyId_FoodTypeId_TriedAt",
                table: "MealPlans",
                columns: new[] { "BabyId", "FoodTypeId", "TriedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_FoodTypeId",
                table: "MealPlans",
                column: "FoodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MealRecommendations_FoodTypeId",
                table: "MealRecommendations",
                column: "FoodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MealRecommendations_WeekNumber_FoodTypeId",
                table: "MealRecommendations",
                columns: new[] { "WeekNumber", "FoodTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicationIntakeLogs_PlanId",
                table: "MedicationIntakeLogs",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicationPlans_ParentProfileId_MedicineName",
                table: "MedicationPlans",
                columns: new[] { "ParentProfileId", "MedicineName" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicationScheduleTimes_PlanId",
                table: "MedicationScheduleTimes",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Milestones_BabyId",
                table: "Milestones",
                column: "BabyId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentProfiles_UserId",
                table: "ParentProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pregnancies_ParentProfileId_LmpDate",
                table: "Pregnancies",
                columns: new[] { "ParentProfileId", "LmpDate" });

            migrationBuilder.CreateIndex(
                name: "IX_QaAnswers_AnsweredById",
                table: "QaAnswers",
                column: "AnsweredById");

            migrationBuilder.CreateIndex(
                name: "IX_QaAnswers_QuestionId",
                table: "QaAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QaQuestions_AskedById",
                table: "QaQuestions",
                column: "AskedById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SleepLogs_BabyId_SleepDate",
                table: "SleepLogs",
                columns: new[] { "BabyId", "SleepDate" });

            migrationBuilder.CreateIndex(
                name: "IX_SymptomDiaries_ParentProfileId_Date",
                table: "SymptomDiaries",
                columns: new[] { "ParentProfileId", "Date" },
                unique: true);

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
                name: "BlogPostInteractions");

            migrationBuilder.DropTable(
                name: "CalendarEvents");

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
                name: "MealRecommendations");

            migrationBuilder.DropTable(
                name: "MedicationIntakeLogs");

            migrationBuilder.DropTable(
                name: "MedicationScheduleTimes");

            migrationBuilder.DropTable(
                name: "Milestones");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "QaAnswers");

            migrationBuilder.DropTable(
                name: "RecommendationModelStates");

            migrationBuilder.DropTable(
                name: "SleepLogs");

            migrationBuilder.DropTable(
                name: "SymptomDiaries");

            migrationBuilder.DropTable(
                name: "WeeklyAdvices");

            migrationBuilder.DropTable(
                name: "BlogCategories");

            migrationBuilder.DropTable(
                name: "BlogPosts");

            migrationBuilder.DropTable(
                name: "ChatConversations");

            migrationBuilder.DropTable(
                name: "FoodTypes");

            migrationBuilder.DropTable(
                name: "MedicationPlans");

            migrationBuilder.DropTable(
                name: "QaQuestions");

            migrationBuilder.DropTable(
                name: "BabyProfiles");

            migrationBuilder.DropTable(
                name: "DoctorProfiles");

            migrationBuilder.DropTable(
                name: "Pregnancies");

            migrationBuilder.DropTable(
                name: "ParentProfiles");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
