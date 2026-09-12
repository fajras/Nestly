using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nestly.Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDemoSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "DateOfBirth", "Email", "FirstName", "Gender", "IdentityUserId", "LastName", "PhoneNumber", "RoleId" },
                values: new object[,]
                {
                    { 3L, new DateTime(1996, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "amina.hodzic@nestly.com", "Amina", "Female", "8339ef91-9659-4986-b918-af66726adf19", "Hodžić", "+3876100001", 1L },
                    { 4L, new DateTime(1993, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "lejla.kovacevic@nestly.com", "Lejla", "Female", "aa140327-579a-4a94-b635-b4d9a915d279", "Kovačević", "+3876100002", 1L },
                    { 5L, new DateTime(1991, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "ajla.delic@nestly.com", "Ajla", "Female", "d771c068-ae58-4a8c-abc1-88ea3418bcc3", "Delić", "+3876100003", 1L },
                    { 6L, new DateTime(1994, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "emina.softic@nestly.com", "Emina", "Female", "27cc0316-257a-442d-bbe5-46656ec343e0", "Softić", "+3876100004", 1L },
                    { 7L, new DateTime(1995, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "selma.begic@nestly.com", "Selma", "Female", "2d30b0f8-e94e-4a85-be12-bb2bb528b70b", "Begić", "+3876100005", 1L },
                    { 8L, new DateTime(1992, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "amra.halilovic@nestly.com", "Amra", "Female", "d66502ad-89df-4de1-90ab-61c153a26d12", "Halilović", "+3876100006", 1L },
                    { 9L, new DateTime(1990, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "merisa.karic@nestly.com", "Merisa", "Female", "e02fd7d5-9498-4439-877b-1170b9ee6f4e", "Karić", "+3876100007", 1L },
                    { 10L, new DateTime(1989, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "dzenita.mujic@nestly.com", "Dženita", "Female", "13edc40b-a767-49d0-a194-610e85ae84d0", "Mujić", "+3876100008", 1L },
                    { 11L, new DateTime(1988, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "amila.zukic@nestly.com", "Amila", "Female", "323c2855-426d-4726-9520-bdd2da76a5d5", "Zukić", "+3876100009", 1L },
                    { 12L, new DateTime(1987, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "nadira.sehic@nestly.com", "Nadira", "Female", "465ced2a-f9e9-4f23-acbb-5a11658737b2", "Šehić", "+3876100010", 1L }
                });

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

            migrationBuilder.InsertData(
                table: "CalendarEvents",
                columns: new[] { "Id", "BabyId", "DeletedAt", "Description", "IsDeleted", "Reminder24hSent", "StartAt", "Title", "UserId" },
                values: new object[] { 5001L, 1L, null, "Redovni sistematski pregled za bebu Emma.", false, false, new DateTime(2026, 9, 11, 10, 0, 0, 0, DateTimeKind.Unspecified), "Kontrola kod pedijatra", 1L });

            migrationBuilder.InsertData(
                table: "ChatConversations",
                columns: new[] { "Id", "CreatedAt", "ParentProfileId", "User1Id", "User2Id" },
                values: new object[] { 5001L, new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1L, 2L });

            migrationBuilder.InsertData(
                table: "DiaperLogs",
                columns: new[] { "Id", "BabyId", "ChangeDate", "ChangeTime", "DeletedAt", "DiaperState", "IsDeleted", "Notes" },
                values: new object[,]
                {
                    { 5001L, 1L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "Wet", false, null },
                    { 5002L, 1L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, "Dirty", false, null },
                    { 5003L, 1L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0), null, "Mixed", false, null },
                    { 5004L, 1L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 30, 0, 0), null, "Wet", false, null }
                });

            migrationBuilder.InsertData(
                table: "FeedingLogs",
                columns: new[] { "Id", "AmountMl", "AmountUnit", "BabyId", "DeletedAt", "FeedDate", "FeedTime", "FoodTypeId", "IsDeleted", "Notes" },
                values: new object[,]
                {
                    { 5001L, null, "g", 1L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0), 11, false, null },
                    { 5002L, null, "g", 1L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 30, 0, 0), 22, false, null },
                    { 5003L, null, "g", 1L, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 15, 0, 0), 16, false, null },
                    { 5004L, null, "g", 1L, null, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 20, false, null }
                });

            migrationBuilder.InsertData(
                table: "HealthEntries",
                columns: new[] { "Id", "BabyId", "DeletedAt", "DoctorVisit", "EntryDate", "IsDeleted", "Medicines", "TemperatureC" },
                values: new object[,]
                {
                    { 5001L, 1L, null, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 36.6m },
                    { 5002L, 1L, null, "Redovna kontrola kod pedijatra", new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Vitamin D kapi", 36.8m }
                });

            migrationBuilder.InsertData(
                table: "MealPlans",
                columns: new[] { "Id", "BabyId", "DeletedAt", "FoodTypeId", "IsDeleted", "Rating", "TriedAt" },
                values: new object[,]
                {
                    { 5001L, 1L, null, 11, false, (short)3, new DateTime(2026, 9, 4, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 5002L, 1L, null, 16, false, (short)5, new DateTime(2026, 8, 31, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 5003L, 1L, null, 21, false, (short)3, new DateTime(2026, 8, 27, 12, 30, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Milestones",
                columns: new[] { "Id", "AchievedDate", "BabyId", "CreatedAt", "DeletedAt", "IsDeleted", "Notes", "Title" },
                values: new object[,]
                {
                    { 5001L, new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Prevrće se sa stomaka na leđa" },
                    { 5002L, new DateTime(2025, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, new DateTime(2025, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Guguče i smije se naglas" }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "CreatedAt", "IsRead", "Message", "Title", "UserId" },
                values: new object[,]
                {
                    { 5001, new DateTime(2026, 9, 1, 9, 30, 0, 0, DateTimeKind.Unspecified), false, "Doktor je odgovorio na vaše pitanje.", "Odgovoreno pitanje", 3L },
                    { 5002, new DateTime(2026, 9, 7, 9, 30, 0, 0, DateTimeKind.Unspecified), false, "Vaš termin pregleda je za 2 dana.", "Podsjetnik za termin", 6L },
                    { 5003, new DateTime(2026, 9, 5, 9, 30, 0, 0, DateTimeKind.Unspecified), true, "Imate novu poruku od doktora.", "Nova poruka", 9L },
                    { 5004, new DateTime(2026, 9, 5, 9, 30, 0, 0, DateTimeKind.Unspecified), false, "Doktor je odgovorio na vaše pitanje.", "Odgovoreno pitanje", 10L },
                    { 5005, new DateTime(2026, 9, 4, 9, 30, 0, 0, DateTimeKind.Unspecified), true, "Dostupan je novi savjet za trenutnu sedmicu trudnoće.", "Novi savjet dostupan", 1L }
                });

            migrationBuilder.InsertData(
                table: "QaQuestions",
                columns: new[] { "Id", "AskedById", "CreatedAt", "DeletedAt", "IsDeleted", "QuestionText" },
                values: new object[] { 5012L, 1L, new DateTime(2026, 8, 28, 9, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Kada uvesti čvrstu hranu bebi?" });

            migrationBuilder.InsertData(
                table: "SleepLogs",
                columns: new[] { "Id", "BabyId", "DeletedAt", "EndTime", "IsDeleted", "SleepDate", "StartTime" },
                values: new object[,]
                {
                    { 5001L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5002L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5003L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "ChatConversations",
                columns: new[] { "Id", "CreatedAt", "ParentProfileId", "User1Id", "User2Id" },
                values: new object[,]
                {
                    { 5002L, new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3L, 2L },
                    { 5003L, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9L, 2L }
                });

            migrationBuilder.InsertData(
                table: "ChatMessages",
                columns: new[] { "Id", "Content", "ConversationId", "CreatedAt", "SenderId" },
                values: new object[,]
                {
                    { 5001L, "Poštovana doktorice, imam pitanje vezano za ishranu bebe.", 5001L, new DateTime(2026, 9, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), 1L },
                    { 5002L, "Izvolite, slobodno pitajte.", 5001L, new DateTime(2026, 9, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), 2L },
                    { 5003L, "Da li Emma može jesti jaja u ovoj fazi?", 5001L, new DateTime(2026, 9, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), 1L },
                    { 5004L, "Da, dobro termički obrađena jaja su u redu od 8. mjeseca.", 5001L, new DateTime(2026, 9, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 2L }
                });

            migrationBuilder.InsertData(
                table: "ParentProfiles",
                columns: new[] { "Id", "UserId" },
                values: new object[,]
                {
                    { 2L, 3L },
                    { 3L, 4L },
                    { 4L, 5L },
                    { 5L, 6L },
                    { 6L, 7L },
                    { 7L, 8L },
                    { 8L, 9L },
                    { 9L, 10L },
                    { 10L, 11L },
                    { 11L, 12L }
                });

            migrationBuilder.InsertData(
                table: "QaAnswers",
                columns: new[] { "Id", "AnswerText", "AnsweredById", "CreatedAt", "QuestionId" },
                values: new object[] { 5008L, "Preporuka je oko 6. mjeseca života, kada beba pokazuje znakove spremnosti poput samostalnog sjedenja i interesa za hranu.", 1L, new DateTime(2026, 8, 29, 14, 0, 0, 0, DateTimeKind.Unspecified), 5012L });

            migrationBuilder.InsertData(
                table: "BabyProfiles",
                columns: new[] { "Id", "BabyName", "BirthDate", "Gender", "ParentProfileId", "PregnancyId" },
                values: new object[,]
                {
                    { 2L, "Faris", new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", 6L, null },
                    { 3L, "Amar", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", 7L, null },
                    { 4L, "Lamija", new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Female", 8L, null },
                    { 5L, "Hana", new DateTime(2025, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Female", 9L, null },
                    { 6L, "Adnan", new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", 10L, null },
                    { 7L, "Tarik", new DateTime(2024, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", 11L, null }
                });

            migrationBuilder.InsertData(
                table: "ChatMessages",
                columns: new[] { "Id", "Content", "ConversationId", "CreatedAt", "SenderId" },
                values: new object[,]
                {
                    { 5005L, "Zdravo doktorice, imam mučninu svako jutro, je li to zabrinjavajuće?", 5002L, new DateTime(2026, 9, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), 3L },
                    { 5006L, "Nije, to je uobičajeno u prvom trimestru. Javite se ako povraćanje bude jako učestalo.", 5002L, new DateTime(2026, 9, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), 2L },
                    { 5007L, "Hvala vam puno!", 5002L, new DateTime(2026, 9, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), 3L },
                    { 5008L, "Poštovani, Lamija odbija čvrstu hranu zadnjih dana.", 5003L, new DateTime(2026, 9, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), 9L },
                    { 5009L, "To je uobičajena faza, nastavite nuditi raznovrsnu hranu bez pritiska.", 5003L, new DateTime(2026, 9, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), 2L },
                    { 5010L, "U redu, hvala na savjetu.", 5003L, new DateTime(2026, 9, 6, 10, 0, 0, 0, DateTimeKind.Unspecified), 9L }
                });

            migrationBuilder.InsertData(
                table: "MedicationPlans",
                columns: new[] { "Id", "Dose", "EndDate", "MedicineName", "ParentProfileId", "StartDate" },
                values: new object[,]
                {
                    { 5001L, "1 tableta", new DateTime(2026, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Prenatalni vitamini (Vitamin D + Folna kiselina)", 2L, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5002L, "1 tableta", new DateTime(2026, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Prenatalni vitamini (Vitamin D + Folna kiselina)", 3L, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5003L, "1 tableta", new DateTime(2026, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Prenatalni vitamini (Vitamin D + Folna kiselina)", 4L, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5004L, "1 tableta", new DateTime(2026, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Prenatalni vitamini (Vitamin D + Folna kiselina)", 5L, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5005L, "1 tableta", new DateTime(2026, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Prenatalni vitamini (Vitamin D + Folna kiselina)", 11L, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Pregnancies",
                columns: new[] { "Id", "CycleLengthDays", "DeletedAt", "DueDate", "IsDeleted", "LmpDate", "ParentProfileId" },
                values: new object[,]
                {
                    { 2L, 28, null, new DateTime(2027, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L },
                    { 3L, 28, null, new DateTime(2027, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2026, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L },
                    { 4L, 28, null, new DateTime(2026, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4L },
                    { 5L, 28, null, new DateTime(2026, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2025, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 5L },
                    { 6L, 28, null, new DateTime(2027, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2026, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 11L }
                });

            migrationBuilder.InsertData(
                table: "QaQuestions",
                columns: new[] { "Id", "AskedById", "CreatedAt", "DeletedAt", "IsDeleted", "QuestionText" },
                values: new object[,]
                {
                    { 5001L, 2L, new DateTime(2026, 9, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Da li je normalno imati jutarnju mučninu u 10. sedmici trudnoće?" },
                    { 5002L, 3L, new DateTime(2026, 9, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Koliko je vode potrebno piti dnevno u drugom trimestru?" },
                    { 5003L, 4L, new DateTime(2026, 9, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Osjećam bolove u leđima u 36. sedmici, je li to normalno?" },
                    { 5004L, 5L, new DateTime(2026, 9, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Kada tačno trebam ići u bolnicu kada počnu trudovi?" },
                    { 5005L, 6L, new DateTime(2026, 9, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Beba puno spava, da li je to normalno u prvom mjesecu?" },
                    { 5006L, 7L, new DateTime(2026, 9, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Kada mogu početi uvoditi kašice bebi?" },
                    { 5007L, 8L, new DateTime(2026, 8, 31, 9, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Koliko obroka dnevno je preporučeno za bebu od 8 mjeseci?" },
                    { 5008L, 9L, new DateTime(2026, 9, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Beba od 13 mjeseci još ne hoda sama, da li da se zabrinem?" },
                    { 5009L, 10L, new DateTime(2026, 9, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Koliko sati sna je normalno za dijete od 19 mjeseci?" },
                    { 5010L, 11L, new DateTime(2026, 8, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Kako pripremiti starije dijete na dolazak bebe?" },
                    { 5011L, 2L, new DateTime(2026, 9, 7, 9, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Koje namirnice je najbolje izbjegavati tokom trudnoće?" }
                });

            migrationBuilder.InsertData(
                table: "SymptomDiaries",
                columns: new[] { "Id", "Date", "DeletedAt", "Fatigue", "Headache", "Heartburn", "IsDeleted", "LegSwelling", "Nausea", "ParentProfileId" },
                values: new object[,]
                {
                    { 5001L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 2L },
                    { 5002L, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 3, 2L },
                    { 5003L, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, null, false, null, 4, 2L },
                    { 5004L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, null, null, false, null, 5, 3L },
                    { 5005L, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 3L },
                    { 5006L, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 3L },
                    { 5007L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 3, 4L },
                    { 5008L, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, null, false, null, 4, 4L },
                    { 5009L, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, null, null, false, null, 5, 4L },
                    { 5010L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 5L },
                    { 5011L, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 5L },
                    { 5012L, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 3, 5L },
                    { 5013L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, null, false, null, 4, 11L },
                    { 5014L, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, null, null, false, null, 5, 11L },
                    { 5015L, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 11L }
                });

            migrationBuilder.InsertData(
                table: "BabyGrowths",
                columns: new[] { "Id", "BabyId", "HeadCircumferenceCm", "HeightCm", "WeekNumber", "WeightKg" },
                values: new object[,]
                {
                    { 5001L, 2L, 34.62m, 50.55m, (short)1, 3.58m },
                    { 5002L, 2L, 34.74m, 51.1m, (short)2, 3.76m },
                    { 5003L, 2L, 34.98m, 52.2m, (short)4, 4.12m },
                    { 5004L, 3L, 34.62m, 50.55m, (short)1, 3.58m },
                    { 5005L, 3L, 35.46m, 54.4m, (short)8, 4.84m },
                    { 5006L, 3L, 36.54m, 59.35m, (short)17, 6.46m },
                    { 5007L, 4L, 34.62m, 50.55m, (short)1, 3.58m },
                    { 5008L, 4L, 36.54m, 59.35m, (short)17, 6.46m },
                    { 5009L, 4L, 38.7m, 69.25m, (short)35, 9.7m },
                    { 5010L, 5L, 34.62m, 50.55m, (short)1, 3.58m },
                    { 5011L, 5L, 37.86m, 65.4m, (short)28, 8.44m },
                    { 5012L, 5L, 41.34m, 81.35m, (short)57, 13.66m },
                    { 5013L, 6L, 34.62m, 50.55m, (short)1, 3.58m },
                    { 5014L, 6L, 39.42m, 72.55m, (short)41, 10.78m },
                    { 5015L, 6L, 44.34m, 95.1m, (short)82, 18.16m },
                    { 5016L, 7L, 34.62m, 50.55m, (short)1, 3.58m },
                    { 5017L, 7L, 42.3m, 85.75m, (short)65, 15.1m },
                    { 5018L, 7L, 50.1m, 121.5m, (short)130, 26.8m }
                });

            migrationBuilder.InsertData(
                table: "CalendarEvents",
                columns: new[] { "Id", "BabyId", "DeletedAt", "Description", "IsDeleted", "Reminder24hSent", "StartAt", "Title", "UserId" },
                values: new object[,]
                {
                    { 5002L, 2L, null, "Redovni sistematski pregled za bebu Faris.", false, false, new DateTime(2026, 9, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), "Kontrola kod pedijatra", 6L },
                    { 5003L, 3L, null, "Redovni sistematski pregled za bebu Amar.", false, false, new DateTime(2026, 9, 13, 10, 0, 0, 0, DateTimeKind.Unspecified), "Kontrola kod pedijatra", 7L },
                    { 5004L, 4L, null, "Redovni sistematski pregled za bebu Lamija.", false, false, new DateTime(2026, 9, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), "Kontrola kod pedijatra", 8L },
                    { 5005L, 5L, null, "Redovni sistematski pregled za bebu Hana.", false, false, new DateTime(2026, 9, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "Kontrola kod pedijatra", 9L },
                    { 5006L, 6L, null, "Redovni sistematski pregled za bebu Adnan.", false, false, new DateTime(2026, 9, 11, 10, 0, 0, 0, DateTimeKind.Unspecified), "Kontrola kod pedijatra", 10L },
                    { 5007L, 7L, null, "Redovni sistematski pregled za bebu Tarik.", false, false, new DateTime(2026, 9, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), "Kontrola kod pedijatra", 11L }
                });

            migrationBuilder.InsertData(
                table: "DiaperLogs",
                columns: new[] { "Id", "BabyId", "ChangeDate", "ChangeTime", "DeletedAt", "DiaperState", "IsDeleted", "Notes" },
                values: new object[,]
                {
                    { 5005L, 2L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "Wet", false, null },
                    { 5006L, 2L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, "Dirty", false, null },
                    { 5007L, 2L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0), null, "Mixed", false, null },
                    { 5008L, 2L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 30, 0, 0), null, "Wet", false, null },
                    { 5009L, 3L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "Wet", false, null },
                    { 5010L, 3L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, "Dirty", false, null },
                    { 5011L, 3L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0), null, "Mixed", false, null },
                    { 5012L, 3L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 30, 0, 0), null, "Wet", false, null },
                    { 5013L, 4L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "Wet", false, null },
                    { 5014L, 4L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, "Dirty", false, null },
                    { 5015L, 4L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0), null, "Mixed", false, null },
                    { 5016L, 4L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 30, 0, 0), null, "Wet", false, null },
                    { 5017L, 5L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "Wet", false, null },
                    { 5018L, 5L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, "Dirty", false, null },
                    { 5019L, 5L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0), null, "Mixed", false, null },
                    { 5020L, 5L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 30, 0, 0), null, "Wet", false, null },
                    { 5021L, 6L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "Wet", false, null },
                    { 5022L, 6L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, "Dirty", false, null },
                    { 5023L, 6L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0), null, "Mixed", false, null },
                    { 5024L, 6L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 30, 0, 0), null, "Wet", false, null },
                    { 5025L, 7L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "Wet", false, null },
                    { 5026L, 7L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, "Dirty", false, null },
                    { 5027L, 7L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0), null, "Mixed", false, null },
                    { 5028L, 7L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 30, 0, 0), null, "Wet", false, null }
                });

            migrationBuilder.InsertData(
                table: "FeedingLogs",
                columns: new[] { "Id", "AmountMl", "AmountUnit", "BabyId", "DeletedAt", "FeedDate", "FeedTime", "FoodTypeId", "IsDeleted", "Notes" },
                values: new object[,]
                {
                    { 5005L, 130m, "ml", 2L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0), null, false, null },
                    { 5006L, 140m, "ml", 2L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 30, 0, 0), null, false, null },
                    { 5007L, 150m, "ml", 2L, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 15, 0, 0), null, false, null },
                    { 5008L, 120m, "ml", 2L, null, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), null, false, null },
                    { 5009L, 130m, "ml", 3L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0), null, false, null },
                    { 5010L, 140m, "ml", 3L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 30, 0, 0), null, false, null },
                    { 5011L, 150m, "ml", 3L, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 15, 0, 0), null, false, null },
                    { 5012L, 120m, "ml", 3L, null, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), null, false, null },
                    { 5013L, null, "g", 4L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0), 11, false, null },
                    { 5014L, null, "g", 4L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 30, 0, 0), 22, false, null },
                    { 5015L, null, "g", 4L, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 15, 0, 0), 16, false, null },
                    { 5016L, null, "g", 4L, null, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 20, false, null },
                    { 5017L, null, "g", 5L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0), 11, false, null },
                    { 5018L, null, "g", 5L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 30, 0, 0), 22, false, null },
                    { 5019L, null, "g", 5L, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 15, 0, 0), 16, false, null },
                    { 5020L, null, "g", 5L, null, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 20, false, null },
                    { 5021L, null, "g", 6L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0), 11, false, null },
                    { 5022L, null, "g", 6L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 30, 0, 0), 22, false, null },
                    { 5023L, null, "g", 6L, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 15, 0, 0), 16, false, null },
                    { 5024L, null, "g", 6L, null, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 20, false, null },
                    { 5025L, null, "g", 7L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0), 11, false, null },
                    { 5026L, null, "g", 7L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 30, 0, 0), 22, false, null },
                    { 5027L, null, "g", 7L, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 15, 0, 0), 16, false, null },
                    { 5028L, null, "g", 7L, null, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 20, false, null }
                });

            migrationBuilder.InsertData(
                table: "HealthDeviationAlerts",
                columns: new[] { "Id", "BabyId", "DetectedAt", "DoctorFeedbackAt", "DoctorFeedbackByDoctorId", "DoctorFeedbackComment", "DoctorFeedbackIsAccurate", "IsResolved", "Message", "ParameterType", "PeriodFrom", "PeriodTo", "Recommendation", "ResolvedAt", "Severity", "Title" },
                values: new object[,]
                {
                    { 5001L, 3L, new DateTime(2026, 9, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 4, 11, 0, 0, 0, DateTimeKind.Unspecified), 1L, "Potvrđeno na pregledu, preporučena prilagodba ishrane.", true, true, "Prirast težine bebe Amar u zadnje dvije sedmice je ispod očekivane krivulje rasta za uzrast.", 1, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Preporučuje se kontrola kod pedijatra i praćenje unosa hrane u narednih 7 dana.", new DateTime(2026, 9, 4, 11, 0, 0, 0, DateTimeKind.Unspecified), 2, "Usporen rast težine" },
                    { 5002L, 5L, new DateTime(2026, 9, 6, 8, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, false, "Beba Hana bilježi kraće noćno spavanje u odnosu na prosjek za njen uzrast tokom zadnjih 5 dana.", 3, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pratite rutinu prije spavanja i javite se doktoru ako se obrazac ne popravi u narednih sedmicu dana.", null, 1, "Promjena obrasca spavanja" }
                });

            migrationBuilder.InsertData(
                table: "HealthEntries",
                columns: new[] { "Id", "BabyId", "DeletedAt", "DoctorVisit", "EntryDate", "IsDeleted", "Medicines", "TemperatureC" },
                values: new object[,]
                {
                    { 5003L, 2L, null, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 36.6m },
                    { 5004L, 2L, null, "Redovna kontrola kod pedijatra", new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Vitamin D kapi", 36.8m },
                    { 5005L, 3L, null, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 36.6m },
                    { 5006L, 3L, null, "Redovna kontrola kod pedijatra", new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Vitamin D kapi", 36.8m },
                    { 5007L, 4L, null, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 36.6m },
                    { 5008L, 4L, null, "Redovna kontrola kod pedijatra", new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Vitamin D kapi", 36.8m },
                    { 5009L, 5L, null, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 36.6m },
                    { 5010L, 5L, null, "Redovna kontrola kod pedijatra", new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Vitamin D kapi", 36.8m },
                    { 5011L, 6L, null, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 36.6m },
                    { 5012L, 6L, null, "Redovna kontrola kod pedijatra", new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Vitamin D kapi", 36.8m },
                    { 5013L, 7L, null, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 36.6m },
                    { 5014L, 7L, null, "Redovna kontrola kod pedijatra", new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Vitamin D kapi", 36.8m }
                });

            migrationBuilder.InsertData(
                table: "MealPlans",
                columns: new[] { "Id", "BabyId", "DeletedAt", "FoodTypeId", "IsDeleted", "Rating", "TriedAt" },
                values: new object[,]
                {
                    { 5004L, 4L, null, 20, false, (short)2, new DateTime(2026, 9, 4, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 5005L, 4L, null, 22, false, (short)4, new DateTime(2026, 8, 31, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 5006L, 4L, null, 34, false, (short)2, new DateTime(2026, 8, 27, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 5007L, 5L, null, 30, false, (short)5, new DateTime(2026, 9, 4, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 5008L, 5L, null, 40, false, (short)3, new DateTime(2026, 8, 31, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 5009L, 5L, null, 11, false, (short)5, new DateTime(2026, 8, 27, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 5010L, 6L, null, 10, false, (short)4, new DateTime(2026, 9, 4, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 5011L, 6L, null, 13, false, (short)2, new DateTime(2026, 8, 31, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 5012L, 6L, null, 20, false, (short)4, new DateTime(2026, 8, 27, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 5013L, 7L, null, 16, false, (short)3, new DateTime(2026, 9, 4, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 5014L, 7L, null, 21, false, (short)5, new DateTime(2026, 8, 31, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 5015L, 7L, null, 30, false, (short)3, new DateTime(2026, 8, 27, 12, 30, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "MedicationIntakeLogs",
                columns: new[] { "Id", "IntakeTime", "PlanId", "ReminderSent", "ScheduledDate", "Taken", "TakenAt" },
                values: new object[,]
                {
                    { 5001L, new TimeSpan(0, 8, 0, 0, 0), 5001L, true, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 5002L, new TimeSpan(0, 8, 0, 0, 0), 5001L, true, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 9, 6, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 5003L, new TimeSpan(0, 8, 0, 0, 0), 5001L, true, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 9, 5, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 5004L, new TimeSpan(0, 8, 0, 0, 0), 5001L, true, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 5005L, new TimeSpan(0, 8, 0, 0, 0), 5002L, true, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 5006L, new TimeSpan(0, 8, 0, 0, 0), 5002L, true, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 9, 6, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 5007L, new TimeSpan(0, 8, 0, 0, 0), 5002L, true, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 9, 5, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 5008L, new TimeSpan(0, 8, 0, 0, 0), 5002L, true, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 5009L, new TimeSpan(0, 8, 0, 0, 0), 5003L, true, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 5010L, new TimeSpan(0, 8, 0, 0, 0), 5003L, true, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 9, 6, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 5011L, new TimeSpan(0, 8, 0, 0, 0), 5003L, true, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 9, 5, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 5012L, new TimeSpan(0, 8, 0, 0, 0), 5003L, true, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 5013L, new TimeSpan(0, 8, 0, 0, 0), 5004L, true, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 5014L, new TimeSpan(0, 8, 0, 0, 0), 5004L, true, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 9, 6, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 5015L, new TimeSpan(0, 8, 0, 0, 0), 5004L, true, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 9, 5, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 5016L, new TimeSpan(0, 8, 0, 0, 0), 5004L, true, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 5017L, new TimeSpan(0, 8, 0, 0, 0), 5005L, true, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 5018L, new TimeSpan(0, 8, 0, 0, 0), 5005L, true, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 9, 6, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 5019L, new TimeSpan(0, 8, 0, 0, 0), 5005L, true, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 9, 5, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 5020L, new TimeSpan(0, 8, 0, 0, 0), 5005L, true, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null }
                });

            migrationBuilder.InsertData(
                table: "MedicationScheduleTimes",
                columns: new[] { "Id", "IntakeTime", "PlanId" },
                values: new object[,]
                {
                    { 5001L, new TimeSpan(0, 8, 0, 0, 0), 5001L },
                    { 5002L, new TimeSpan(0, 8, 0, 0, 0), 5002L },
                    { 5003L, new TimeSpan(0, 8, 0, 0, 0), 5003L },
                    { 5004L, new TimeSpan(0, 8, 0, 0, 0), 5004L },
                    { 5005L, new TimeSpan(0, 8, 0, 0, 0), 5005L }
                });

            migrationBuilder.InsertData(
                table: "Milestones",
                columns: new[] { "Id", "AchievedDate", "BabyId", "CreatedAt", "DeletedAt", "IsDeleted", "Notes", "Title" },
                values: new object[,]
                {
                    { 5003L, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Prvi osmijeh" },
                    { 5004L, new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, new DateTime(2026, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Podiže glavu dok leži na stomaku" },
                    { 5005L, new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Prati predmete pogledom" },
                    { 5006L, new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 4L, new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Samostalno sjedi" },
                    { 5007L, new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 4L, new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Počinje puzati" },
                    { 5008L, new DateTime(2025, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 5L, new DateTime(2025, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Prve samostalne riječi (mama, tata)" },
                    { 5009L, new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 5L, new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Stoji uz pridržavanje" },
                    { 5010L, new DateTime(2025, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 6L, new DateTime(2025, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Prvi samostalni koraci" },
                    { 5011L, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 6L, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Jede kašikom uz pomoć" },
                    { 5012L, new DateTime(2024, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 7L, new DateTime(2024, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Trči i penje se po namještaju" },
                    { 5013L, new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 7L, new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Govori kratke rečenice" }
                });

            migrationBuilder.InsertData(
                table: "QaAnswers",
                columns: new[] { "Id", "AnswerText", "AnsweredById", "CreatedAt", "QuestionId" },
                values: new object[,]
                {
                    { 5001L, "Da, jutarnja mučnina je vrlo česta u prvom trimestru i obično prolazi do 12-14. sedmice. Pijte dosta tečnosti i jedite manje, češće obroke.", 1L, new DateTime(2026, 9, 2, 14, 0, 0, 0, DateTimeKind.Unspecified), 5001L },
                    { 5002L, "Preporučuje se oko 2 do 2.5 litre tečnosti dnevno, ovisno o aktivnosti i tjelesnoj masi.", 1L, new DateTime(2026, 9, 4, 14, 0, 0, 0, DateTimeKind.Unspecified), 5002L },
                    { 5003L, "Kada kontrakcije postanu redovne (svakih 5 minuta, traju oko 60 sekundi) u trajanju od sat vremena, ili ako pukne vodenjak, vrijeme je za bolnicu.", 1L, new DateTime(2026, 9, 5, 14, 0, 0, 0, DateTimeKind.Unspecified), 5004L },
                    { 5004L, "Da, novorođenčad spava 16-18 sati dnevno u kratkim intervalima, to je potpuno normalno.", 1L, new DateTime(2026, 9, 3, 14, 0, 0, 0, DateTimeKind.Unspecified), 5005L },
                    { 5005L, "Obično 3 glavna obroka plus 1-2 manje užine, uz nastavak dojenja/formule po potrebi.", 1L, new DateTime(2026, 9, 1, 14, 0, 0, 0, DateTimeKind.Unspecified), 5007L },
                    { 5006L, "Raspon je širok, mnoga djeca prohodaju između 12. i 18. mjeseca. Ako postoji napredak (stajanje, hodanje uz pridržavanje), nema razloga za brigu.", 1L, new DateTime(2026, 9, 6, 14, 0, 0, 0, DateTimeKind.Unspecified), 5008L },
                    { 5007L, "Uključite ga u pripreme, govorite pozitivno o bebi i zadržite što više uobičajenu rutinu kako bi se osjećalo sigurno.", 1L, new DateTime(2026, 8, 31, 14, 0, 0, 0, DateTimeKind.Unspecified), 5010L }
                });

            migrationBuilder.InsertData(
                table: "SleepLogs",
                columns: new[] { "Id", "BabyId", "DeletedAt", "EndTime", "IsDeleted", "SleepDate", "StartTime" },
                values: new object[,]
                {
                    { 5004L, 2L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5005L, 2L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5006L, 2L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5007L, 3L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5008L, 3L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5009L, 3L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5010L, 4L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5011L, 4L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5012L, 4L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5013L, 5L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5014L, 5L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5015L, 5L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5016L, 6L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5017L, 6L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5018L, 6L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5019L, 7L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5020L, 7L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5021L, 7L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5005L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5006L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5007L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5008L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5009L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5010L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5011L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5012L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5013L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5014L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5015L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5016L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5017L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 5018L);

            migrationBuilder.DeleteData(
                table: "CalendarEvents",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "CalendarEvents",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "CalendarEvents",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "CalendarEvents",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "CalendarEvents",
                keyColumn: "Id",
                keyValue: 5005L);

            migrationBuilder.DeleteData(
                table: "CalendarEvents",
                keyColumn: "Id",
                keyValue: 5006L);

            migrationBuilder.DeleteData(
                table: "CalendarEvents",
                keyColumn: "Id",
                keyValue: 5007L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 5005L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 5006L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 5007L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 5008L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 5009L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 5010L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5005L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5006L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5007L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5008L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5009L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5010L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5011L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5012L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5013L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5014L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5015L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5016L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5017L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5018L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5019L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5020L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5021L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5022L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5023L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5024L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5025L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5026L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5027L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 5028L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5005L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5006L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5007L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5008L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5009L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5010L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5011L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5012L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5013L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5014L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5015L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5016L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5017L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5018L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5019L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5020L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5021L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5022L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5023L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5024L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5025L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5026L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5027L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 5028L);

            migrationBuilder.DeleteData(
                table: "HealthDeviationAlerts",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "HealthDeviationAlerts",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5005L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5006L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5007L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5008L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5009L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5010L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5011L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5012L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5013L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5014L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 5005L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 5006L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 5007L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 5008L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 5009L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 5010L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 5011L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 5012L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 5013L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 5014L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 5015L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5005L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5006L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5007L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5008L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5009L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5010L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5011L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5012L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5013L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5014L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5015L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5016L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5017L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5018L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5019L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 5020L);

            migrationBuilder.DeleteData(
                table: "MedicationScheduleTimes",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "MedicationScheduleTimes",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "MedicationScheduleTimes",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "MedicationScheduleTimes",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "MedicationScheduleTimes",
                keyColumn: "Id",
                keyValue: 5005L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 5005L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 5006L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 5007L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 5008L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 5009L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 5010L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 5011L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 5012L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 5013L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 5001);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 5002);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 5003);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 5004);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 5005);

            migrationBuilder.DeleteData(
                table: "Pregnancies",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Pregnancies",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Pregnancies",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Pregnancies",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Pregnancies",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "QaAnswers",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "QaAnswers",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "QaAnswers",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "QaAnswers",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "QaAnswers",
                keyColumn: "Id",
                keyValue: 5005L);

            migrationBuilder.DeleteData(
                table: "QaAnswers",
                keyColumn: "Id",
                keyValue: 5006L);

            migrationBuilder.DeleteData(
                table: "QaAnswers",
                keyColumn: "Id",
                keyValue: 5007L);

            migrationBuilder.DeleteData(
                table: "QaAnswers",
                keyColumn: "Id",
                keyValue: 5008L);

            migrationBuilder.DeleteData(
                table: "QaQuestions",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "QaQuestions",
                keyColumn: "Id",
                keyValue: 5006L);

            migrationBuilder.DeleteData(
                table: "QaQuestions",
                keyColumn: "Id",
                keyValue: 5009L);

            migrationBuilder.DeleteData(
                table: "QaQuestions",
                keyColumn: "Id",
                keyValue: 5011L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5005L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5006L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5007L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5008L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5009L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5010L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5011L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5012L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5013L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5014L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5015L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5016L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5017L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5018L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5019L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5020L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 5021L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 5005L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 5006L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 5007L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 5008L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 5009L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 5010L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 5011L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 5012L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 5013L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 5014L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 5015L);

            migrationBuilder.DeleteData(
                table: "BabyProfiles",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "BabyProfiles",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "BabyProfiles",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "BabyProfiles",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "BabyProfiles",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "BabyProfiles",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "ChatConversations",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "ChatConversations",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "ChatConversations",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "MedicationPlans",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "MedicationPlans",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "MedicationPlans",
                keyColumn: "Id",
                keyValue: 5003L);

            migrationBuilder.DeleteData(
                table: "MedicationPlans",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "MedicationPlans",
                keyColumn: "Id",
                keyValue: 5005L);

            migrationBuilder.DeleteData(
                table: "QaQuestions",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "QaQuestions",
                keyColumn: "Id",
                keyValue: 5002L);

            migrationBuilder.DeleteData(
                table: "QaQuestions",
                keyColumn: "Id",
                keyValue: 5004L);

            migrationBuilder.DeleteData(
                table: "QaQuestions",
                keyColumn: "Id",
                keyValue: 5005L);

            migrationBuilder.DeleteData(
                table: "QaQuestions",
                keyColumn: "Id",
                keyValue: 5007L);

            migrationBuilder.DeleteData(
                table: "QaQuestions",
                keyColumn: "Id",
                keyValue: 5008L);

            migrationBuilder.DeleteData(
                table: "QaQuestions",
                keyColumn: "Id",
                keyValue: 5010L);

            migrationBuilder.DeleteData(
                table: "QaQuestions",
                keyColumn: "Id",
                keyValue: 5012L);

            migrationBuilder.DeleteData(
                table: "ParentProfiles",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "ParentProfiles",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "ParentProfiles",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "ParentProfiles",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "ParentProfiles",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "ParentProfiles",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "ParentProfiles",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "ParentProfiles",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "ParentProfiles",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "ParentProfiles",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 12L);

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
        }
    }
}
