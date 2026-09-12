using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nestly.Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class EmmaRichDemoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CalendarEvents",
                keyColumn: "Id",
                keyValue: 5001L);

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
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 5002L);

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
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 5001L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 5002L);

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

            migrationBuilder.InsertData(
                table: "BabyGrowths",
                columns: new[] { "Id", "BabyId", "HeadCircumferenceCm", "HeightCm", "WeekNumber", "WeightKg" },
                values: new object[,]
                {
                    { 9001L, 1L, 34.5m, 50.0m, (short)1, 3.4m },
                    { 9002L, 1L, 35.0m, 51.5m, (short)2, 3.7m },
                    { 9003L, 1L, 36.2m, 54.5m, (short)4, 4.4m },
                    { 9004L, 1L, 37.2m, 57.0m, (short)6, 5.0m },
                    { 9005L, 1L, 38.0m, 59.0m, (short)8, 5.5m },
                    { 9006L, 1L, 38.7m, 60.5m, (short)10, 6.0m },
                    { 9007L, 1L, 39.5m, 62.5m, (short)13, 6.6m },
                    { 9008L, 1L, 40.5m, 65.0m, (short)17, 7.3m },
                    { 9009L, 1L, 41.3m, 67.0m, (short)21, 7.9m },
                    { 9010L, 1L, 42.0m, 68.5m, (short)26, 8.4m }
                });

            migrationBuilder.UpdateData(
                table: "BabyProfiles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "BirthDate",
                value: new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 13, 22, 12, 24, 904, DateTimeKind.Utc).AddTicks(1157));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Prvi prenatalni pregled je ključan korak u praćenju trudnoće i obično se zakazuje između 8. i 10. sedmice. Prije pregleda zapišite datum posljednje menstruacije jer će ljekar na osnovu toga procijeniti gestacijsku dob. Sastavite listu lijekova, suplemenata i hroničnih stanja koja imate kako bi ljekar procijenio sigurnost terapije. Pripremite pitanja o prehrani, dozvoljenoj fizičkoj aktivnosti, putovanjima i simptomima koji vas brinu. Na pregledu se obično rade laboratorijske analize krvi i urina kako bi se provjerilo opšte stanje organizma i vrijednosti važnih parametara. Ljekar može uraditi i ultrazvuk kako bi provjerio razvoj ploda i prisustvo otkucaja srca. Nemojte se ustručavati pričati o mučnini, umoru, strahovima ili emocionalnim promjenama, jer sve to spada u važan dio anamneze. Preporučuje se da sa sobom povedete partnera ili blisku osobu koja vam pruža podršku i može zapamtiti informacije umjesto vas. Zapišite preporuke koje dobijete, poput folne kiseline, unosa tečnosti i izbjegavanja određenih namirnica. Redovni prenatalni pregledi kasnije će pomoći da se potencijalni problemi otkriju na vrijeme i trudnoća prati sigurno. Ovaj prvi korak postavlja temelje povjerenja između vas i vašeg doktora, što je od velikog značaja za cijeli period trudnoće.", new DateTime(2026, 8, 16, 22, 12, 24, 904, DateTimeKind.Utc).AddTicks(1168) });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 18, 22, 12, 24, 904, DateTimeKind.Utc).AddTicks(1171));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 19, 22, 12, 24, 904, DateTimeKind.Utc).AddTicks(1174));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 21, 22, 12, 24, 904, DateTimeKind.Utc).AddTicks(1219));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 23, 22, 12, 24, 904, DateTimeKind.Utc).AddTicks(1221));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 25, 22, 12, 24, 904, DateTimeKind.Utc).AddTicks(1224));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 28, 22, 12, 24, 904, DateTimeKind.Utc).AddTicks(1226));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 30, 22, 12, 24, 904, DateTimeKind.Utc).AddTicks(1228));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 1, 22, 12, 24, 904, DateTimeKind.Utc).AddTicks(1231));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 3, 22, 12, 24, 904, DateTimeKind.Utc).AddTicks(1233));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 5, 22, 12, 24, 904, DateTimeKind.Utc).AddTicks(1235));

            migrationBuilder.InsertData(
                table: "CalendarEvents",
                columns: new[] { "Id", "BabyId", "DeletedAt", "Description", "IsDeleted", "Reminder24hSent", "StartAt", "Title", "UserId" },
                values: new object[,]
                {
                    { 9001L, 1L, null, "Sistematski pregled novorođenčeta.", false, false, new DateTime(2026, 3, 17, 10, 0, 0, 0, DateTimeKind.Unspecified), "Prvi sistematski pregled", 1L },
                    { 9002L, 1L, null, "Redovna kontrola rasta i razvoja.", false, false, new DateTime(2026, 4, 9, 10, 0, 0, 0, DateTimeKind.Unspecified), "Kontrola u 1. mjesecu", 1L },
                    { 9003L, 1L, null, "Redovna vakcinacija prema kalendaru.", false, false, new DateTime(2026, 5, 9, 10, 0, 0, 0, DateTimeKind.Unspecified), "Vakcinacija - 2. mjesec", 1L },
                    { 9004L, 1L, null, "Redovna kontrola kod pedijatra.", false, false, new DateTime(2026, 7, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), "Kontrola u 4. mjesecu", 1L },
                    { 9005L, 1L, null, "Redovna vakcinacija prema kalendaru.", false, false, new DateTime(2026, 9, 13, 10, 0, 0, 0, DateTimeKind.Unspecified), "Vakcinacija - 6. mjesec", 1L },
                    { 9006L, 1L, null, "Sistematski pregled za 6 mjeseci.", false, false, new DateTime(2026, 9, 22, 10, 0, 0, 0, DateTimeKind.Unspecified), "Kontrola rasta i razvoja", 1L }
                });

            migrationBuilder.InsertData(
                table: "ChatMessages",
                columns: new[] { "Id", "Content", "ConversationId", "CreatedAt", "SenderId" },
                values: new object[,]
                {
                    { 9001L, "Poštovana doktorice, Emma je sada napunila 6 mjeseci.", 5001L, new DateTime(2026, 9, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), 1L },
                    { 9002L, "Čestitam! Kako napreduje uvođenje čvrste hrane?", 5001L, new DateTime(2026, 9, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), 2L },
                    { 9003L, "Uglavnom dobro prihvata povrće, malo je izbirljivija sa voćem.", 5001L, new DateTime(2026, 9, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), 1L },
                    { 9004L, "To je sasvim uobičajeno, nastavite sa raznovrsnom ponudom bez pritiska.", 5001L, new DateTime(2026, 9, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), 2L },
                    { 9005L, "Hvala vam! Još jedno pitanje - koliko sati sna je normalno u ovom uzrastu?", 5001L, new DateTime(2026, 9, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), 1L },
                    { 9006L, "Obično oko 14-15 sati ukupno, uključujući dnevne dremke.", 5001L, new DateTime(2026, 9, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), 2L },
                    { 9007L, "Razumijem, hvala puno na pomoći!", 5001L, new DateTime(2026, 9, 6, 10, 0, 0, 0, DateTimeKind.Unspecified), 1L }
                });

            migrationBuilder.InsertData(
                table: "DiaperLogs",
                columns: new[] { "Id", "BabyId", "ChangeDate", "ChangeTime", "DeletedAt", "DiaperState", "IsDeleted", "Notes" },
                values: new object[,]
                {
                    { 9001L, 1L, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9002L, 1L, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9003L, 1L, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9004L, 1L, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9005L, 1L, new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9006L, 1L, new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9007L, 1L, new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9008L, 1L, new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9009L, 1L, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9010L, 1L, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9011L, 1L, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9012L, 1L, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9013L, 1L, new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9014L, 1L, new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9015L, 1L, new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9016L, 1L, new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9017L, 1L, new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9018L, 1L, new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9019L, 1L, new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9020L, 1L, new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9021L, 1L, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9022L, 1L, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9023L, 1L, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9024L, 1L, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9025L, 1L, new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9026L, 1L, new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9027L, 1L, new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9028L, 1L, new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9029L, 1L, new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9030L, 1L, new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9031L, 1L, new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9032L, 1L, new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9033L, 1L, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9034L, 1L, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9035L, 1L, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9036L, 1L, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9037L, 1L, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9038L, 1L, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9039L, 1L, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9040L, 1L, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9041L, 1L, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9042L, 1L, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9043L, 1L, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9044L, 1L, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9045L, 1L, new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9046L, 1L, new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9047L, 1L, new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9048L, 1L, new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9049L, 1L, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9050L, 1L, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9051L, 1L, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9052L, 1L, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9053L, 1L, new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9054L, 1L, new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9055L, 1L, new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9056L, 1L, new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9057L, 1L, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9058L, 1L, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9059L, 1L, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9060L, 1L, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9061L, 1L, new DateTime(2026, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9062L, 1L, new DateTime(2026, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9063L, 1L, new DateTime(2026, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9064L, 1L, new DateTime(2026, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9065L, 1L, new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9066L, 1L, new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9067L, 1L, new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9068L, 1L, new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9069L, 1L, new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9070L, 1L, new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9071L, 1L, new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9072L, 1L, new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9073L, 1L, new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9074L, 1L, new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9075L, 1L, new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9076L, 1L, new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9077L, 1L, new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9078L, 1L, new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9079L, 1L, new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9080L, 1L, new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9081L, 1L, new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9082L, 1L, new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9083L, 1L, new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9084L, 1L, new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9085L, 1L, new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9086L, 1L, new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9087L, 1L, new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9088L, 1L, new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9089L, 1L, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9090L, 1L, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9091L, 1L, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9092L, 1L, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9093L, 1L, new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9094L, 1L, new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9095L, 1L, new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9096L, 1L, new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9097L, 1L, new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9098L, 1L, new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9099L, 1L, new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9100L, 1L, new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9101L, 1L, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9102L, 1L, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9103L, 1L, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9104L, 1L, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9105L, 1L, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9106L, 1L, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9107L, 1L, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9108L, 1L, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9109L, 1L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9110L, 1L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9111L, 1L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9112L, 1L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9113L, 1L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9114L, 1L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9115L, 1L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9116L, 1L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9117L, 1L, new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 9118L, 1L, new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 0, 0, 0), null, "stolica", false, null },
                    { 9119L, 1L, new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), null, "mokra", false, null },
                    { 9120L, 1L, new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 30, 0, 0), null, "kombinovano", false, null },
                    { 9121L, 1L, new DateTime(2026, 8, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 10, 0, 0, 0), null, "mokra", false, null },
                    { 9122L, 1L, new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 10, 0, 0, 0), null, "mokra", false, null },
                    { 9123L, 1L, new DateTime(2026, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 10, 0, 0, 0), null, "mokra", false, null },
                    { 9124L, 1L, new DateTime(2026, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 10, 0, 0, 0), null, "mokra", false, null },
                    { 9125L, 1L, new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 10, 0, 0, 0), null, "mokra", false, null },
                    { 9126L, 1L, new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 10, 0, 0, 0), null, "mokra", false, null },
                    { 9127L, 1L, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 10, 0, 0, 0), null, "mokra", false, null },
                    { 9128L, 1L, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 10, 0, 0, 0), null, "mokra", false, null },
                    { 9129L, 1L, new DateTime(2026, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 10, 0, 0, 0), null, "mokra", false, null },
                    { 9130L, 1L, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 10, 0, 0, 0), null, "mokra", false, null },
                    { 9131L, 1L, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 10, 0, 0, 0), null, "mokra", false, null }
                });

            migrationBuilder.InsertData(
                table: "FeedingLogs",
                columns: new[] { "Id", "AmountMl", "AmountUnit", "BabyId", "DeletedAt", "FeedDate", "FeedTime", "FoodTypeId", "IsDeleted", "Notes" },
                values: new object[,]
                {
                    { 9001L, 67m, "g", 1L, null, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 62, false, null },
                    { 9002L, 74m, "g", 1L, null, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 11, false, null },
                    { 9003L, 81m, "g", 1L, null, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 16, false, null },
                    { 9004L, 88m, "g", 1L, null, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 21, false, null },
                    { 9005L, 95m, "g", 1L, null, new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 16, false, null },
                    { 9006L, 102m, "g", 1L, null, new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 21, false, null },
                    { 9007L, 109m, "g", 1L, null, new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 23, false, null },
                    { 9008L, 116m, "g", 1L, null, new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 33, false, null },
                    { 9009L, 63m, "g", 1L, null, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 23, false, null },
                    { 9010L, 70m, "g", 1L, null, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 33, false, null },
                    { 9011L, 77m, "g", 1L, null, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 40, false, null },
                    { 9012L, 84m, "g", 1L, null, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 62, false, null },
                    { 9013L, 91m, "g", 1L, null, new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 40, false, null },
                    { 9014L, 98m, "g", 1L, null, new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 62, false, null },
                    { 9015L, 105m, "g", 1L, null, new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 11, false, null },
                    { 9016L, 112m, "g", 1L, null, new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 16, false, null },
                    { 9017L, 119m, "g", 1L, null, new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 11, false, null },
                    { 9018L, 66m, "g", 1L, null, new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 16, false, null },
                    { 9019L, 73m, "g", 1L, null, new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 21, false, null },
                    { 9020L, 80m, "g", 1L, null, new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 23, false, null },
                    { 9021L, 87m, "g", 1L, null, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 21, false, null },
                    { 9022L, 94m, "g", 1L, null, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 23, false, null },
                    { 9023L, 101m, "g", 1L, null, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 33, false, null },
                    { 9024L, 108m, "g", 1L, null, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 40, false, null },
                    { 9025L, 115m, "g", 1L, null, new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 33, false, null },
                    { 9026L, 62m, "g", 1L, null, new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 40, false, null },
                    { 9027L, 69m, "g", 1L, null, new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 62, false, null },
                    { 9028L, 76m, "g", 1L, null, new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 11, false, null },
                    { 9029L, 83m, "g", 1L, null, new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 62, false, null },
                    { 9030L, 90m, "g", 1L, null, new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 11, false, null },
                    { 9031L, 97m, "g", 1L, null, new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 16, false, null },
                    { 9032L, 104m, "g", 1L, null, new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 21, false, null },
                    { 9033L, 111m, "g", 1L, null, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 16, false, null },
                    { 9034L, 118m, "g", 1L, null, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 21, false, null },
                    { 9035L, 65m, "g", 1L, null, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 23, false, null },
                    { 9036L, 72m, "g", 1L, null, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 33, false, null },
                    { 9037L, 79m, "g", 1L, null, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 23, false, null },
                    { 9038L, 86m, "g", 1L, null, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 33, false, null },
                    { 9039L, 93m, "g", 1L, null, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 40, false, null },
                    { 9040L, 100m, "g", 1L, null, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 62, false, null },
                    { 9041L, 107m, "g", 1L, null, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 40, false, null },
                    { 9042L, 114m, "g", 1L, null, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 62, false, null },
                    { 9043L, 61m, "g", 1L, null, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 11, false, null },
                    { 9044L, 68m, "g", 1L, null, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 16, false, null },
                    { 9045L, 75m, "g", 1L, null, new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 11, false, null },
                    { 9046L, 82m, "g", 1L, null, new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 16, false, null },
                    { 9047L, 89m, "g", 1L, null, new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 21, false, null },
                    { 9048L, 96m, "g", 1L, null, new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 23, false, null },
                    { 9049L, 103m, "g", 1L, null, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 21, false, null },
                    { 9050L, 110m, "g", 1L, null, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 23, false, null },
                    { 9051L, 117m, "g", 1L, null, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 33, false, null },
                    { 9052L, 64m, "g", 1L, null, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 40, false, null },
                    { 9053L, 71m, "g", 1L, null, new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 33, false, null },
                    { 9054L, 78m, "g", 1L, null, new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 40, false, null },
                    { 9055L, 85m, "g", 1L, null, new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 62, false, null },
                    { 9056L, 92m, "g", 1L, null, new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 11, false, null },
                    { 9057L, 99m, "g", 1L, null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 62, false, null },
                    { 9058L, 106m, "g", 1L, null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 11, false, null },
                    { 9059L, 113m, "g", 1L, null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 16, false, null },
                    { 9060L, 60m, "g", 1L, null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 21, false, null },
                    { 9061L, 67m, "g", 1L, null, new DateTime(2026, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 16, false, null },
                    { 9062L, 74m, "g", 1L, null, new DateTime(2026, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 21, false, null },
                    { 9063L, 81m, "g", 1L, null, new DateTime(2026, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 23, false, null },
                    { 9064L, 88m, "g", 1L, null, new DateTime(2026, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 33, false, null },
                    { 9065L, 95m, "g", 1L, null, new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 23, false, null },
                    { 9066L, 102m, "g", 1L, null, new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 33, false, null },
                    { 9067L, 109m, "g", 1L, null, new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 40, false, null },
                    { 9068L, 116m, "g", 1L, null, new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 62, false, null },
                    { 9069L, 63m, "g", 1L, null, new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 40, false, null },
                    { 9070L, 70m, "g", 1L, null, new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 62, false, null },
                    { 9071L, 77m, "g", 1L, null, new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 11, false, null },
                    { 9072L, 84m, "g", 1L, null, new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 16, false, null },
                    { 9073L, 91m, "g", 1L, null, new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 11, false, null },
                    { 9074L, 98m, "g", 1L, null, new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 16, false, null },
                    { 9075L, 105m, "g", 1L, null, new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 21, false, null },
                    { 9076L, 112m, "g", 1L, null, new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 23, false, null },
                    { 9077L, 119m, "g", 1L, null, new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 21, false, null },
                    { 9078L, 66m, "g", 1L, null, new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 23, false, null },
                    { 9079L, 73m, "g", 1L, null, new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 33, false, null },
                    { 9080L, 80m, "g", 1L, null, new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 40, false, null },
                    { 9081L, 87m, "g", 1L, null, new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 33, false, null },
                    { 9082L, 94m, "g", 1L, null, new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 40, false, null },
                    { 9083L, 101m, "g", 1L, null, new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 62, false, null },
                    { 9084L, 108m, "g", 1L, null, new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 11, false, null },
                    { 9085L, 115m, "g", 1L, null, new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 62, false, null },
                    { 9086L, 62m, "g", 1L, null, new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 11, false, null },
                    { 9087L, 69m, "g", 1L, null, new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 16, false, null },
                    { 9088L, 76m, "g", 1L, null, new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 21, false, null },
                    { 9089L, 83m, "g", 1L, null, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 16, false, null },
                    { 9090L, 90m, "g", 1L, null, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 21, false, null },
                    { 9091L, 97m, "g", 1L, null, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 23, false, null },
                    { 9092L, 104m, "g", 1L, null, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 33, false, null },
                    { 9093L, 111m, "g", 1L, null, new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 23, false, null },
                    { 9094L, 118m, "g", 1L, null, new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 33, false, null },
                    { 9095L, 65m, "g", 1L, null, new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 40, false, null },
                    { 9096L, 72m, "g", 1L, null, new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 62, false, null },
                    { 9097L, 79m, "g", 1L, null, new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 40, false, null },
                    { 9098L, 86m, "g", 1L, null, new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 62, false, null },
                    { 9099L, 93m, "g", 1L, null, new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 11, false, null },
                    { 9100L, 100m, "g", 1L, null, new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 16, false, null },
                    { 9101L, 107m, "g", 1L, null, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 11, false, null },
                    { 9102L, 114m, "g", 1L, null, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 16, false, null },
                    { 9103L, 61m, "g", 1L, null, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 21, false, null },
                    { 9104L, 68m, "g", 1L, null, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 23, false, null },
                    { 9105L, 75m, "g", 1L, null, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 21, false, null },
                    { 9106L, 82m, "g", 1L, null, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 23, false, null },
                    { 9107L, 89m, "g", 1L, null, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 33, false, null },
                    { 9108L, 96m, "g", 1L, null, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 40, false, null },
                    { 9109L, 103m, "g", 1L, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 33, false, null },
                    { 9110L, 110m, "g", 1L, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 40, false, null },
                    { 9111L, 117m, "g", 1L, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 62, false, null },
                    { 9112L, 64m, "g", 1L, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 11, false, null },
                    { 9113L, 71m, "g", 1L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 62, false, null },
                    { 9114L, 78m, "g", 1L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 11, false, null },
                    { 9115L, 85m, "g", 1L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 16, false, null },
                    { 9116L, 92m, "g", 1L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 21, false, null },
                    { 9117L, 99m, "g", 1L, null, new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), 16, false, null },
                    { 9118L, 106m, "g", 1L, null, new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 30, 0, 0), 21, false, null },
                    { 9119L, 113m, "g", 1L, null, new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 15, 0, 0, 0), 23, false, null },
                    { 9120L, 60m, "g", 1L, null, new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 33, false, null },
                    { 9121L, 131m, "ml", 1L, null, new DateTime(2026, 8, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, false, null },
                    { 9122L, 142m, "ml", 1L, null, new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, false, null },
                    { 9123L, 153m, "ml", 1L, null, new DateTime(2026, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, false, null },
                    { 9124L, 164m, "ml", 1L, null, new DateTime(2026, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, false, null },
                    { 9125L, 175m, "ml", 1L, null, new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, false, null },
                    { 9126L, 126m, "ml", 1L, null, new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, false, null },
                    { 9127L, 137m, "ml", 1L, null, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, false, null },
                    { 9128L, 148m, "ml", 1L, null, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, false, null },
                    { 9129L, 159m, "ml", 1L, null, new DateTime(2026, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, false, null },
                    { 9130L, 170m, "ml", 1L, null, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, false, null },
                    { 9131L, 121m, "ml", 1L, null, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, false, null }
                });

            migrationBuilder.InsertData(
                table: "HealthEntries",
                columns: new[] { "Id", "BabyId", "DeletedAt", "DoctorVisit", "EntryDate", "IsDeleted", "Medicines", "TemperatureC" },
                values: new object[,]
                {
                    { 9001L, 1L, null, null, new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Vitamin D kapi", 36.5m },
                    { 9002L, 1L, null, null, new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 36.65m },
                    { 9003L, 1L, null, null, new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Vitamin D kapi", 36.8m },
                    { 9004L, 1L, null, null, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 36.5m },
                    { 9005L, 1L, null, null, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Vitamin D kapi", 36.65m },
                    { 9006L, 1L, null, "Redovna kontrola kod pedijatra", new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 36.8m },
                    { 9007L, 1L, null, "Mjesečna kontrola kod pedijatra", new DateTime(2026, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 36.7m },
                    { 9008L, 1L, null, "Mjesečna kontrola kod pedijatra", new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 36.7m },
                    { 9009L, 1L, null, "Mjesečna kontrola kod pedijatra", new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 36.7m },
                    { 9010L, 1L, null, "Mjesečna kontrola kod pedijatra", new DateTime(2026, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 36.7m },
                    { 9011L, 1L, null, "Mjesečna kontrola kod pedijatra", new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 36.7m }
                });

            migrationBuilder.InsertData(
                table: "MealPlans",
                columns: new[] { "Id", "BabyId", "DeletedAt", "FoodTypeId", "IsDeleted", "Rating", "TriedAt" },
                values: new object[,]
                {
                    { 9001L, 1L, null, 11, false, (short)3, new DateTime(2026, 8, 10, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9002L, 1L, null, 16, false, (short)5, new DateTime(2026, 8, 10, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9003L, 1L, null, 16, false, (short)5, new DateTime(2026, 8, 11, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9004L, 1L, null, 21, false, (short)3, new DateTime(2026, 8, 11, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9005L, 1L, null, 21, false, (short)3, new DateTime(2026, 8, 12, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9006L, 1L, null, 23, false, (short)5, new DateTime(2026, 8, 12, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9007L, 1L, null, 23, false, (short)5, new DateTime(2026, 8, 13, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9008L, 1L, null, 33, false, (short)3, new DateTime(2026, 8, 13, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9009L, 1L, null, 33, false, (short)3, new DateTime(2026, 8, 14, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9010L, 1L, null, 40, false, (short)5, new DateTime(2026, 8, 14, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9011L, 1L, null, 40, false, (short)5, new DateTime(2026, 8, 15, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9012L, 1L, null, 62, false, (short)3, new DateTime(2026, 8, 15, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9013L, 1L, null, 62, false, (short)3, new DateTime(2026, 8, 16, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9014L, 1L, null, 10, false, (short)5, new DateTime(2026, 8, 16, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9015L, 1L, null, 10, false, (short)5, new DateTime(2026, 8, 17, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9016L, 1L, null, 13, false, (short)3, new DateTime(2026, 8, 17, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9017L, 1L, null, 13, false, (short)3, new DateTime(2026, 8, 18, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9018L, 1L, null, 20, false, (short)5, new DateTime(2026, 8, 18, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9019L, 1L, null, 20, false, (short)5, new DateTime(2026, 8, 19, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9020L, 1L, null, 22, false, (short)3, new DateTime(2026, 8, 19, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9021L, 1L, null, 22, false, (short)3, new DateTime(2026, 8, 20, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9022L, 1L, null, 30, false, (short)5, new DateTime(2026, 8, 20, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9023L, 1L, null, 30, false, (short)5, new DateTime(2026, 8, 21, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9024L, 1L, null, 34, false, (short)3, new DateTime(2026, 8, 21, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9025L, 1L, null, 34, false, (short)3, new DateTime(2026, 8, 22, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9026L, 1L, null, 60, false, (short)5, new DateTime(2026, 8, 22, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9027L, 1L, null, 60, false, (short)5, new DateTime(2026, 8, 23, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9028L, 1L, null, 70, false, (short)3, new DateTime(2026, 8, 23, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9029L, 1L, null, 70, false, (short)3, new DateTime(2026, 8, 24, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9030L, 1L, null, 11, false, (short)5, new DateTime(2026, 8, 24, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9031L, 1L, null, 11, false, (short)5, new DateTime(2026, 8, 25, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9032L, 1L, null, 16, false, (short)3, new DateTime(2026, 8, 25, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9033L, 1L, null, 16, false, (short)3, new DateTime(2026, 8, 26, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9034L, 1L, null, 21, false, (short)5, new DateTime(2026, 8, 26, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9035L, 1L, null, 21, false, (short)5, new DateTime(2026, 8, 27, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9036L, 1L, null, 23, false, (short)3, new DateTime(2026, 8, 27, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9037L, 1L, null, 23, false, (short)3, new DateTime(2026, 8, 28, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9038L, 1L, null, 33, false, (short)5, new DateTime(2026, 8, 28, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9039L, 1L, null, 33, false, (short)5, new DateTime(2026, 8, 29, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9040L, 1L, null, 40, false, (short)3, new DateTime(2026, 8, 29, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9041L, 1L, null, 40, false, (short)3, new DateTime(2026, 8, 30, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9042L, 1L, null, 62, false, (short)5, new DateTime(2026, 8, 30, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9043L, 1L, null, 62, false, (short)5, new DateTime(2026, 8, 31, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9044L, 1L, null, 10, false, (short)3, new DateTime(2026, 8, 31, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9045L, 1L, null, 10, false, (short)3, new DateTime(2026, 9, 1, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9046L, 1L, null, 13, false, (short)5, new DateTime(2026, 9, 1, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9047L, 1L, null, 13, false, (short)5, new DateTime(2026, 9, 2, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9048L, 1L, null, 20, false, (short)3, new DateTime(2026, 9, 2, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9049L, 1L, null, 20, false, (short)3, new DateTime(2026, 9, 3, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9050L, 1L, null, 22, false, (short)5, new DateTime(2026, 9, 3, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9051L, 1L, null, 22, false, (short)5, new DateTime(2026, 9, 4, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9052L, 1L, null, 30, false, (short)3, new DateTime(2026, 9, 4, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9053L, 1L, null, 30, false, (short)3, new DateTime(2026, 9, 5, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9054L, 1L, null, 34, false, (short)5, new DateTime(2026, 9, 5, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9055L, 1L, null, 34, false, (short)5, new DateTime(2026, 9, 6, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9056L, 1L, null, 60, false, (short)3, new DateTime(2026, 9, 6, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9057L, 1L, null, 60, false, (short)3, new DateTime(2026, 9, 7, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9058L, 1L, null, 70, false, (short)5, new DateTime(2026, 9, 7, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9059L, 1L, null, 70, false, (short)5, new DateTime(2026, 9, 8, 12, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 9060L, 1L, null, 11, false, (short)3, new DateTime(2026, 9, 8, 12, 30, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "MedicationPlans",
                columns: new[] { "Id", "Dose", "EndDate", "MedicineName", "ParentProfileId", "StartDate" },
                values: new object[,]
                {
                    { 9001L, "1 tableta", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Prenatalni vitamini (Vitamin D + Folna kiselina)", 1L, new DateTime(2025, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9002L, "1 tableta", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Željezo (Fe) dodatak", 1L, new DateTime(2025, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Milestones",
                columns: new[] { "Id", "AchievedDate", "BabyId", "CreatedAt", "DeletedAt", "IsDeleted", "Notes", "Title" },
                values: new object[,]
                {
                    { 9001L, new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Prvi osmijeh" },
                    { 9002L, new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Prati predmete pogledom" },
                    { 9003L, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Podiže glavu dok leži na stomaku" },
                    { 9004L, new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Guguče i smije se naglas" },
                    { 9005L, new DateTime(2026, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, new DateTime(2026, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Prevrće se sa stomaka na leđa" },
                    { 9006L, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Počinje sjedati uz pridržavanje" },
                    { 9007L, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "Pokazuje interes za hranu" }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "CreatedAt", "IsRead", "Message", "Title", "UserId" },
                values: new object[,]
                {
                    { 9001, new DateTime(2026, 9, 6, 9, 30, 0, 0, DateTimeKind.Unspecified), true, "Doktor je odgovorio na vaše pitanje.", "Odgovoreno pitanje", 1L },
                    { 9002, new DateTime(2026, 9, 8, 9, 30, 0, 0, DateTimeKind.Unspecified), false, "Vakcinacija je zakazana za 5 dana.", "Podsjetnik za termin", 1L },
                    { 9003, new DateTime(2026, 9, 5, 9, 30, 0, 0, DateTimeKind.Unspecified), true, "Imate novu poruku od doktora.", "Nova poruka", 1L },
                    { 9004, new DateTime(2026, 8, 27, 9, 30, 0, 0, DateTimeKind.Unspecified), true, "Novo dostignuće je dodano za bebu Emma.", "Dostignuće zabilježeno", 1L }
                });

            migrationBuilder.UpdateData(
                table: "Pregnancies",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DueDate", "LmpDate" },
                values: new object[] { new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "QaQuestions",
                columns: new[] { "Id", "AskedById", "CreatedAt", "DeletedAt", "IsDeleted", "QuestionText" },
                values: new object[,]
                {
                    { 9001L, 1L, new DateTime(2026, 9, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Beba od 6 mjeseci se budi nekoliko puta tokom noći, da li je to normalno?" },
                    { 9002L, 1L, new DateTime(2026, 9, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Koliko obroka čvrste hrane dnevno je preporučeno za bebu od 6 mjeseci?" }
                });

            migrationBuilder.InsertData(
                table: "SleepLogs",
                columns: new[] { "Id", "BabyId", "DeletedAt", "EndTime", "IsDeleted", "SleepDate", "StartTime" },
                values: new object[,]
                {
                    { 9001L, 1L, null, new TimeSpan(0, 6, 35, 0, 0), false, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 20, 0, 0) },
                    { 9002L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9003L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 15, 0, 0) },
                    { 9004L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9005L, 1L, null, new TimeSpan(0, 6, 35, 0, 0), false, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 25, 0, 0) },
                    { 9006L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9007L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 20, 0, 0) },
                    { 9008L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9009L, 1L, null, new TimeSpan(0, 6, 35, 0, 0), false, new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 15, 0, 0) },
                    { 9010L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9011L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 25, 0, 0) },
                    { 9012L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9013L, 1L, null, new TimeSpan(0, 6, 35, 0, 0), false, new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 20, 0, 0) },
                    { 9014L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9015L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 15, 0, 0) },
                    { 9016L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9017L, 1L, null, new TimeSpan(0, 6, 35, 0, 0), false, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 25, 0, 0) },
                    { 9018L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9019L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 20, 0, 0) },
                    { 9020L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9021L, 1L, null, new TimeSpan(0, 6, 35, 0, 0), false, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 15, 0, 0) },
                    { 9022L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9023L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 25, 0, 0) },
                    { 9024L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9025L, 1L, null, new TimeSpan(0, 6, 35, 0, 0), false, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 20, 0, 0) },
                    { 9026L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9027L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 15, 0, 0) },
                    { 9028L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9029L, 1L, null, new TimeSpan(0, 6, 35, 0, 0), false, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 25, 0, 0) },
                    { 9030L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9031L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 20, 0, 0) },
                    { 9032L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9033L, 1L, null, new TimeSpan(0, 6, 35, 0, 0), false, new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 15, 0, 0) },
                    { 9034L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9035L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 25, 0, 0) },
                    { 9036L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9037L, 1L, null, new TimeSpan(0, 6, 35, 0, 0), false, new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 20, 0, 0) },
                    { 9038L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9039L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 15, 0, 0) },
                    { 9040L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9041L, 1L, null, new TimeSpan(0, 6, 35, 0, 0), false, new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 25, 0, 0) },
                    { 9042L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9043L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 20, 0, 0) },
                    { 9044L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9045L, 1L, null, new TimeSpan(0, 6, 35, 0, 0), false, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 15, 0, 0) },
                    { 9046L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9047L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 25, 0, 0) },
                    { 9048L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9049L, 1L, null, new TimeSpan(0, 6, 35, 0, 0), false, new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 20, 0, 0) },
                    { 9050L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9051L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 15, 0, 0) },
                    { 9052L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9053L, 1L, null, new TimeSpan(0, 6, 35, 0, 0), false, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 25, 0, 0) },
                    { 9054L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9055L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 20, 0, 0) },
                    { 9056L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9057L, 1L, null, new TimeSpan(0, 6, 35, 0, 0), false, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 15, 0, 0) },
                    { 9058L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9059L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 25, 0, 0) },
                    { 9060L, 1L, null, new TimeSpan(0, 14, 30, 0, 0), false, new DateTime(2026, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 0, 0, 0) },
                    { 9061L, 1L, null, new TimeSpan(0, 6, 30, 0, 0), false, new DateTime(2026, 8, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 9062L, 1L, null, new TimeSpan(0, 6, 30, 0, 0), false, new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 9063L, 1L, null, new TimeSpan(0, 6, 30, 0, 0), false, new DateTime(2026, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 9064L, 1L, null, new TimeSpan(0, 6, 30, 0, 0), false, new DateTime(2026, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 9065L, 1L, null, new TimeSpan(0, 6, 30, 0, 0), false, new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 9066L, 1L, null, new TimeSpan(0, 6, 30, 0, 0), false, new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 9067L, 1L, null, new TimeSpan(0, 6, 30, 0, 0), false, new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 9068L, 1L, null, new TimeSpan(0, 6, 30, 0, 0), false, new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 9069L, 1L, null, new TimeSpan(0, 6, 30, 0, 0), false, new DateTime(2026, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 9070L, 1L, null, new TimeSpan(0, 6, 30, 0, 0), false, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 9071L, 1L, null, new TimeSpan(0, 6, 30, 0, 0), false, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "SymptomDiaries",
                columns: new[] { "Id", "Date", "DeletedAt", "Fatigue", "Headache", "Heartburn", "IsDeleted", "LegSwelling", "Nausea", "ParentProfileId" },
                values: new object[,]
                {
                    { 9001L, new DateTime(2025, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9002L, new DateTime(2025, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9003L, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9004L, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9005L, new DateTime(2025, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9006L, new DateTime(2025, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9007L, new DateTime(2025, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9008L, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9009L, new DateTime(2025, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9010L, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9011L, new DateTime(2025, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9012L, new DateTime(2025, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9013L, new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9014L, new DateTime(2025, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9015L, new DateTime(2025, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9016L, new DateTime(2025, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9017L, new DateTime(2025, 6, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9018L, new DateTime(2025, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9019L, new DateTime(2025, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9020L, new DateTime(2025, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9021L, new DateTime(2025, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9022L, new DateTime(2025, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9023L, new DateTime(2025, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9024L, new DateTime(2025, 6, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9025L, new DateTime(2025, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9026L, new DateTime(2025, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9027L, new DateTime(2025, 6, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9028L, new DateTime(2025, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9029L, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9030L, new DateTime(2025, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9031L, new DateTime(2025, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9032L, new DateTime(2025, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9033L, new DateTime(2025, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9034L, new DateTime(2025, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9035L, new DateTime(2025, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9036L, new DateTime(2025, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9037L, new DateTime(2025, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9038L, new DateTime(2025, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9039L, new DateTime(2025, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9040L, new DateTime(2025, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9041L, new DateTime(2025, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9042L, new DateTime(2025, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9043L, new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9044L, new DateTime(2025, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9045L, new DateTime(2025, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9046L, new DateTime(2025, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9047L, new DateTime(2025, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9048L, new DateTime(2025, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9049L, new DateTime(2025, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9050L, new DateTime(2025, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9051L, new DateTime(2025, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9052L, new DateTime(2025, 7, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9053L, new DateTime(2025, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9054L, new DateTime(2025, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9055L, new DateTime(2025, 7, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9056L, new DateTime(2025, 7, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9057L, new DateTime(2025, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9058L, new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9059L, new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9060L, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9061L, new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9062L, new DateTime(2025, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9063L, new DateTime(2025, 8, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9064L, new DateTime(2025, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9065L, new DateTime(2025, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9066L, new DateTime(2025, 8, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9067L, new DateTime(2025, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9068L, new DateTime(2025, 8, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9069L, new DateTime(2025, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9070L, new DateTime(2025, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9071L, new DateTime(2025, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9072L, new DateTime(2025, 8, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9073L, new DateTime(2025, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9074L, new DateTime(2025, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9075L, new DateTime(2025, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9076L, new DateTime(2025, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9077L, new DateTime(2025, 8, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9078L, new DateTime(2025, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9079L, new DateTime(2025, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9080L, new DateTime(2025, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9081L, new DateTime(2025, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9082L, new DateTime(2025, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9083L, new DateTime(2025, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9084L, new DateTime(2025, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9085L, new DateTime(2025, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9086L, new DateTime(2025, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9087L, new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9088L, new DateTime(2025, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, null, false, null, 5, 1L },
                    { 9089L, new DateTime(2025, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 3, 1L },
                    { 9090L, new DateTime(2025, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 4, 1L },
                    { 9091L, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, null, false, null, 3, 1L },
                    { 9092L, new DateTime(2025, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9093L, new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9094L, new DateTime(2025, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9095L, new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9096L, new DateTime(2025, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9097L, new DateTime(2025, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9098L, new DateTime(2025, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9099L, new DateTime(2025, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9100L, new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9101L, new DateTime(2025, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9102L, new DateTime(2025, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9103L, new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9104L, new DateTime(2025, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9105L, new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9106L, new DateTime(2025, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9107L, new DateTime(2025, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9108L, new DateTime(2025, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9109L, new DateTime(2025, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9110L, new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9111L, new DateTime(2025, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9112L, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9113L, new DateTime(2025, 9, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9114L, new DateTime(2025, 9, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9115L, new DateTime(2025, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9116L, new DateTime(2025, 9, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9117L, new DateTime(2025, 9, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9118L, new DateTime(2025, 9, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9119L, new DateTime(2025, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9120L, new DateTime(2025, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9121L, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9122L, new DateTime(2025, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9123L, new DateTime(2025, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9124L, new DateTime(2025, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9125L, new DateTime(2025, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9126L, new DateTime(2025, 10, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9127L, new DateTime(2025, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9128L, new DateTime(2025, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9129L, new DateTime(2025, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9130L, new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9131L, new DateTime(2025, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9132L, new DateTime(2025, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9133L, new DateTime(2025, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9134L, new DateTime(2025, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9135L, new DateTime(2025, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9136L, new DateTime(2025, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9137L, new DateTime(2025, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9138L, new DateTime(2025, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9139L, new DateTime(2025, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9140L, new DateTime(2025, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9141L, new DateTime(2025, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9142L, new DateTime(2025, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9143L, new DateTime(2025, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9144L, new DateTime(2025, 10, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9145L, new DateTime(2025, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9146L, new DateTime(2025, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9147L, new DateTime(2025, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9148L, new DateTime(2025, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9149L, new DateTime(2025, 10, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9150L, new DateTime(2025, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9151L, new DateTime(2025, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9152L, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9153L, new DateTime(2025, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9154L, new DateTime(2025, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9155L, new DateTime(2025, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9156L, new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9157L, new DateTime(2025, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9158L, new DateTime(2025, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9159L, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9160L, new DateTime(2025, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9161L, new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9162L, new DateTime(2025, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9163L, new DateTime(2025, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9164L, new DateTime(2025, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9165L, new DateTime(2025, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9166L, new DateTime(2025, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9167L, new DateTime(2025, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9168L, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9169L, new DateTime(2025, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9170L, new DateTime(2025, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9171L, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9172L, new DateTime(2025, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9173L, new DateTime(2025, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9174L, new DateTime(2025, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9175L, new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9176L, new DateTime(2025, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9177L, new DateTime(2025, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9178L, new DateTime(2025, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9179L, new DateTime(2025, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9180L, new DateTime(2025, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9181L, new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9182L, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9183L, new DateTime(2025, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9184L, new DateTime(2025, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9185L, new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, 1, false, null, null, 1L },
                    { 9186L, new DateTime(2025, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, false, null, 1, 1L },
                    { 9187L, new DateTime(2025, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, 2, 1L },
                    { 9188L, new DateTime(2025, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 3, null, false, null, null, 1L },
                    { 9189L, new DateTime(2025, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, null, false, null, null, 1L },
                    { 9190L, new DateTime(2025, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9191L, new DateTime(2025, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9192L, new DateTime(2025, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9193L, new DateTime(2025, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9194L, new DateTime(2025, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9195L, new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9196L, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9197L, new DateTime(2025, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9198L, new DateTime(2025, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9199L, new DateTime(2025, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9200L, new DateTime(2025, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9201L, new DateTime(2025, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9202L, new DateTime(2025, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9203L, new DateTime(2025, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9204L, new DateTime(2025, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9205L, new DateTime(2025, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9206L, new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9207L, new DateTime(2025, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9208L, new DateTime(2025, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9209L, new DateTime(2025, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9210L, new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9211L, new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9212L, new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9213L, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9214L, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9215L, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9216L, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9217L, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9218L, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9219L, new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9220L, new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9221L, new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9222L, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9223L, new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9224L, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9225L, new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9226L, new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9227L, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9228L, new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9229L, new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9230L, new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9231L, new DateTime(2026, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9232L, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9233L, new DateTime(2026, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9234L, new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9235L, new DateTime(2026, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9236L, new DateTime(2026, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9237L, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9238L, new DateTime(2026, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9239L, new DateTime(2026, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9240L, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9241L, new DateTime(2026, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9242L, new DateTime(2026, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9243L, new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9244L, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9245L, new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9246L, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9247L, new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9248L, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9249L, new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9250L, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9251L, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9252L, new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9253L, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9254L, new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9255L, new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9256L, new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9257L, new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9258L, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9259L, new DateTime(2026, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9260L, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9261L, new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9262L, new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9263L, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9264L, new DateTime(2026, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9265L, new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9266L, new DateTime(2026, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9267L, new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9268L, new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9269L, new DateTime(2026, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9270L, new DateTime(2026, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9271L, new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9272L, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9273L, new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9274L, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9275L, new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L },
                    { 9276L, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9277L, new DateTime(2026, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 2, 3, false, 3, null, 1L },
                    { 9278L, new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, null, 4, false, 4, null, 1L },
                    { 9279L, new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, null, 2, false, 2, null, 1L },
                    { 9280L, new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, null, 3, false, 3, null, 1L }
                });

            migrationBuilder.InsertData(
                table: "MedicationIntakeLogs",
                columns: new[] { "Id", "IntakeTime", "PlanId", "ReminderSent", "ScheduledDate", "Taken", "TakenAt" },
                values: new object[,]
                {
                    { 9001L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9002L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9003L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 1, 25, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9004L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 1, 25, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9005L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 1, 26, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9006L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 1, 26, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9007L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 1, 27, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9008L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 1, 27, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9009L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 1, 28, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9010L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 1, 28, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9011L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 1, 29, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9012L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9013L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9014L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 1, 30, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9015L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 1, 31, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9016L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 1, 31, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9017L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 1, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9018L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 1, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9019L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 2, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9020L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 2, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9021L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 3, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9022L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9023L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 4, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9024L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 4, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9025L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9026L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 5, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9027L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 6, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9028L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 6, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9029L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 7, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9030L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 7, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9031L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 8, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9032L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9033L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 9, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9034L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 9, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9035L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 10, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9036L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 10, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9037L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9038L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 11, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9039L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 12, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9040L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 12, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9041L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 13, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9042L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9043L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 14, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9044L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 14, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9045L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 15, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9046L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 15, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9047L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 16, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9048L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 16, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9049L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9050L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 17, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9051L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 18, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9052L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9053L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 19, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9054L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 19, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9055L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 20, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9056L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 20, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9057L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 21, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9058L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 21, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9059L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 22, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9060L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 22, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9061L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9062L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9063L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 24, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9064L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 24, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9065L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 25, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9066L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 25, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9067L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 26, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9068L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 26, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9069L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 27, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9070L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 27, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9071L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 2, 28, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9072L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9073L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9074L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 1, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9075L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 2, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9076L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 2, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9077L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 3, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9078L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 3, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9079L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 4, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9080L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 4, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9081L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 5, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9082L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9083L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 6, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9084L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 6, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9085L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null },
                    { 9086L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 7, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9087L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 8, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9088L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 8, 20, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 9089L, new TimeSpan(0, 8, 0, 0, 0), 9001L, true, new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 9, 8, 5, 0, 0, DateTimeKind.Unspecified) },
                    { 9090L, new TimeSpan(0, 20, 0, 0, 0), 9002L, true, new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2026, 3, 9, 20, 10, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "MedicationScheduleTimes",
                columns: new[] { "Id", "IntakeTime", "PlanId" },
                values: new object[,]
                {
                    { 9001L, new TimeSpan(0, 8, 0, 0, 0), 9001L },
                    { 9002L, new TimeSpan(0, 20, 0, 0, 0), 9002L }
                });

            migrationBuilder.InsertData(
                table: "QaAnswers",
                columns: new[] { "Id", "AnswerText", "AnsweredById", "CreatedAt", "QuestionId" },
                values: new object[] { 9001L, "Noćna buđenja su i dalje česta u ovom uzrastu, posebno tokom rasta zuba. Ako beba brzo zaspi nakon utjehe, nema razloga za brigu.", 1L, new DateTime(2026, 9, 6, 14, 0, 0, 0, DateTimeKind.Unspecified), 9001L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 9001L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 9002L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 9003L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 9004L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 9005L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 9006L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 9007L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 9008L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 9009L);

            migrationBuilder.DeleteData(
                table: "BabyGrowths",
                keyColumn: "Id",
                keyValue: 9010L);

            migrationBuilder.DeleteData(
                table: "CalendarEvents",
                keyColumn: "Id",
                keyValue: 9001L);

            migrationBuilder.DeleteData(
                table: "CalendarEvents",
                keyColumn: "Id",
                keyValue: 9002L);

            migrationBuilder.DeleteData(
                table: "CalendarEvents",
                keyColumn: "Id",
                keyValue: 9003L);

            migrationBuilder.DeleteData(
                table: "CalendarEvents",
                keyColumn: "Id",
                keyValue: 9004L);

            migrationBuilder.DeleteData(
                table: "CalendarEvents",
                keyColumn: "Id",
                keyValue: 9005L);

            migrationBuilder.DeleteData(
                table: "CalendarEvents",
                keyColumn: "Id",
                keyValue: 9006L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9001L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9002L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9003L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9004L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9005L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9006L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9007L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9001L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9002L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9003L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9004L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9005L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9006L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9007L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9008L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9009L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9010L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9011L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9012L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9013L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9014L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9015L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9016L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9017L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9018L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9019L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9020L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9021L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9022L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9023L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9024L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9025L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9026L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9027L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9028L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9029L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9030L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9031L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9032L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9033L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9034L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9035L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9036L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9037L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9038L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9039L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9040L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9041L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9042L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9043L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9044L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9045L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9046L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9047L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9048L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9049L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9050L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9051L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9052L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9053L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9054L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9055L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9056L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9057L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9058L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9059L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9060L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9061L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9062L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9063L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9064L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9065L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9066L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9067L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9068L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9069L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9070L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9071L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9072L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9073L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9074L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9075L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9076L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9077L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9078L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9079L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9080L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9081L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9082L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9083L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9084L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9085L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9086L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9087L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9088L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9089L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9090L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9091L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9092L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9093L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9094L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9095L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9096L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9097L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9098L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9099L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9100L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9101L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9102L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9103L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9104L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9105L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9106L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9107L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9108L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9109L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9110L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9111L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9112L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9113L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9114L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9115L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9116L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9117L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9118L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9119L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9120L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9121L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9122L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9123L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9124L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9125L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9126L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9127L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9128L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9129L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9130L);

            migrationBuilder.DeleteData(
                table: "DiaperLogs",
                keyColumn: "Id",
                keyValue: 9131L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9001L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9002L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9003L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9004L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9005L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9006L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9007L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9008L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9009L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9010L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9011L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9012L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9013L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9014L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9015L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9016L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9017L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9018L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9019L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9020L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9021L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9022L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9023L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9024L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9025L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9026L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9027L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9028L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9029L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9030L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9031L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9032L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9033L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9034L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9035L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9036L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9037L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9038L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9039L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9040L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9041L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9042L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9043L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9044L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9045L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9046L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9047L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9048L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9049L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9050L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9051L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9052L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9053L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9054L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9055L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9056L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9057L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9058L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9059L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9060L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9061L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9062L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9063L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9064L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9065L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9066L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9067L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9068L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9069L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9070L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9071L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9072L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9073L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9074L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9075L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9076L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9077L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9078L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9079L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9080L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9081L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9082L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9083L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9084L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9085L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9086L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9087L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9088L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9089L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9090L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9091L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9092L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9093L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9094L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9095L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9096L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9097L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9098L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9099L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9100L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9101L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9102L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9103L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9104L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9105L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9106L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9107L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9108L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9109L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9110L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9111L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9112L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9113L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9114L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9115L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9116L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9117L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9118L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9119L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9120L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9121L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9122L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9123L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9124L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9125L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9126L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9127L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9128L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9129L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9130L);

            migrationBuilder.DeleteData(
                table: "FeedingLogs",
                keyColumn: "Id",
                keyValue: 9131L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 9001L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 9002L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 9003L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 9004L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 9005L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 9006L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 9007L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 9008L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 9009L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 9010L);

            migrationBuilder.DeleteData(
                table: "HealthEntries",
                keyColumn: "Id",
                keyValue: 9011L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9001L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9002L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9003L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9004L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9005L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9006L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9007L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9008L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9009L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9010L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9011L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9012L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9013L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9014L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9015L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9016L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9017L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9018L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9019L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9020L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9021L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9022L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9023L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9024L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9025L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9026L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9027L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9028L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9029L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9030L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9031L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9032L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9033L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9034L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9035L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9036L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9037L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9038L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9039L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9040L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9041L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9042L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9043L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9044L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9045L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9046L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9047L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9048L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9049L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9050L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9051L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9052L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9053L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9054L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9055L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9056L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9057L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9058L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9059L);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "Id",
                keyValue: 9060L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9001L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9002L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9003L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9004L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9005L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9006L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9007L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9008L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9009L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9010L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9011L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9012L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9013L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9014L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9015L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9016L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9017L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9018L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9019L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9020L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9021L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9022L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9023L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9024L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9025L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9026L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9027L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9028L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9029L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9030L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9031L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9032L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9033L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9034L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9035L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9036L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9037L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9038L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9039L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9040L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9041L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9042L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9043L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9044L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9045L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9046L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9047L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9048L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9049L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9050L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9051L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9052L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9053L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9054L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9055L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9056L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9057L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9058L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9059L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9060L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9061L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9062L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9063L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9064L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9065L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9066L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9067L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9068L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9069L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9070L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9071L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9072L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9073L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9074L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9075L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9076L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9077L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9078L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9079L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9080L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9081L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9082L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9083L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9084L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9085L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9086L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9087L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9088L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9089L);

            migrationBuilder.DeleteData(
                table: "MedicationIntakeLogs",
                keyColumn: "Id",
                keyValue: 9090L);

            migrationBuilder.DeleteData(
                table: "MedicationScheduleTimes",
                keyColumn: "Id",
                keyValue: 9001L);

            migrationBuilder.DeleteData(
                table: "MedicationScheduleTimes",
                keyColumn: "Id",
                keyValue: 9002L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 9001L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 9002L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 9003L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 9004L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 9005L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 9006L);

            migrationBuilder.DeleteData(
                table: "Milestones",
                keyColumn: "Id",
                keyValue: 9007L);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 9001);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 9002);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 9003);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 9004);

            migrationBuilder.DeleteData(
                table: "QaAnswers",
                keyColumn: "Id",
                keyValue: 9001L);

            migrationBuilder.DeleteData(
                table: "QaQuestions",
                keyColumn: "Id",
                keyValue: 9002L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9001L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9002L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9003L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9004L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9005L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9006L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9007L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9008L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9009L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9010L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9011L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9012L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9013L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9014L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9015L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9016L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9017L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9018L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9019L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9020L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9021L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9022L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9023L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9024L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9025L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9026L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9027L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9028L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9029L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9030L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9031L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9032L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9033L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9034L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9035L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9036L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9037L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9038L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9039L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9040L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9041L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9042L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9043L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9044L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9045L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9046L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9047L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9048L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9049L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9050L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9051L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9052L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9053L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9054L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9055L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9056L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9057L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9058L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9059L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9060L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9061L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9062L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9063L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9064L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9065L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9066L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9067L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9068L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9069L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9070L);

            migrationBuilder.DeleteData(
                table: "SleepLogs",
                keyColumn: "Id",
                keyValue: 9071L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9001L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9002L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9003L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9004L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9005L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9006L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9007L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9008L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9009L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9010L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9011L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9012L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9013L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9014L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9015L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9016L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9017L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9018L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9019L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9020L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9021L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9022L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9023L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9024L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9025L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9026L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9027L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9028L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9029L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9030L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9031L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9032L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9033L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9034L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9035L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9036L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9037L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9038L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9039L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9040L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9041L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9042L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9043L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9044L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9045L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9046L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9047L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9048L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9049L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9050L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9051L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9052L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9053L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9054L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9055L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9056L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9057L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9058L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9059L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9060L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9061L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9062L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9063L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9064L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9065L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9066L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9067L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9068L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9069L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9070L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9071L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9072L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9073L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9074L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9075L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9076L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9077L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9078L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9079L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9080L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9081L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9082L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9083L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9084L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9085L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9086L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9087L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9088L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9089L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9090L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9091L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9092L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9093L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9094L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9095L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9096L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9097L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9098L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9099L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9100L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9101L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9102L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9103L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9104L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9105L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9106L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9107L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9108L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9109L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9110L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9111L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9112L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9113L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9114L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9115L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9116L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9117L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9118L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9119L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9120L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9121L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9122L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9123L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9124L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9125L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9126L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9127L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9128L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9129L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9130L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9131L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9132L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9133L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9134L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9135L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9136L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9137L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9138L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9139L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9140L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9141L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9142L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9143L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9144L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9145L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9146L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9147L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9148L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9149L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9150L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9151L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9152L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9153L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9154L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9155L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9156L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9157L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9158L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9159L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9160L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9161L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9162L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9163L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9164L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9165L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9166L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9167L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9168L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9169L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9170L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9171L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9172L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9173L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9174L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9175L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9176L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9177L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9178L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9179L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9180L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9181L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9182L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9183L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9184L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9185L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9186L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9187L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9188L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9189L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9190L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9191L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9192L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9193L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9194L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9195L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9196L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9197L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9198L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9199L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9200L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9201L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9202L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9203L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9204L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9205L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9206L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9207L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9208L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9209L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9210L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9211L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9212L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9213L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9214L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9215L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9216L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9217L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9218L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9219L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9220L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9221L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9222L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9223L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9224L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9225L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9226L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9227L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9228L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9229L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9230L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9231L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9232L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9233L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9234L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9235L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9236L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9237L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9238L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9239L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9240L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9241L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9242L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9243L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9244L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9245L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9246L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9247L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9248L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9249L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9250L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9251L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9252L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9253L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9254L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9255L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9256L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9257L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9258L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9259L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9260L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9261L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9262L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9263L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9264L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9265L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9266L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9267L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9268L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9269L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9270L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9271L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9272L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9273L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9274L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9275L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9276L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9277L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9278L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9279L);

            migrationBuilder.DeleteData(
                table: "SymptomDiaries",
                keyColumn: "Id",
                keyValue: 9280L);

            migrationBuilder.DeleteData(
                table: "MedicationPlans",
                keyColumn: "Id",
                keyValue: 9001L);

            migrationBuilder.DeleteData(
                table: "MedicationPlans",
                keyColumn: "Id",
                keyValue: 9002L);

            migrationBuilder.DeleteData(
                table: "QaQuestions",
                keyColumn: "Id",
                keyValue: 9001L);

            migrationBuilder.UpdateData(
                table: "BabyProfiles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "BirthDate",
                value: new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Prvi prenatalni pregled je ključan korak u praćenju trudnoće i obično se zakazuje između 8. i 10. sedmice. Prije pregleda zapišite datum posljednje menstruacije jer će ljekar na osnovu toga procijeniti gestacijsku dob. Sastavite listu lijekova, suplemenata i hroničnih stanja koja imate kako bi ljekar procijenio sigurnost terapije. Pripremite pitanja o prehrani, dozvoljenoj fizičkoj aktivnosti, putovanjima i simptomima koji vas brinu. Na pregledu se obično rade laboratorijske analize krvi i urina kako bi se provjerilo opće stanje organizma i vrijednosti važnih parametara. Ljekar može uraditi i ultrazvuk kako bi provjerio razvoj ploda i prisustvo otkucaja srca. Nemojte se ustručavati pričati o mučnini, umoru, strahovima ili emocionalnim promjenama, jer sve to spada u važan dio anamneze. Preporučuje se da sa sobom povedete partnera ili blisku osobu koja vam pruža podršku i može zapamtiti informacije umjesto vas. Zapišite preporuke koje dobijete, poput folne kiseline, unosa tečnosti i izbjegavanja određenih namirnica. Redovni prenatalni pregledi kasnije će pomoći da se potencijalni problemi otkriju na vrijeme i trudnoća prati sigurno. Ovaj prvi korak postavlja temelje povjerenja između vas i vašeg doktora, što je od velikog značaja za cijeli period trudnoće.", new DateTime(2026, 8, 16, 20, 57, 47, 343, DateTimeKind.Utc).AddTicks(9265) });

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

            migrationBuilder.InsertData(
                table: "CalendarEvents",
                columns: new[] { "Id", "BabyId", "DeletedAt", "Description", "IsDeleted", "Reminder24hSent", "StartAt", "Title", "UserId" },
                values: new object[] { 5001L, 1L, null, "Redovni sistematski pregled za bebu Emma.", false, false, new DateTime(2026, 9, 11, 10, 0, 0, 0, DateTimeKind.Unspecified), "Kontrola kod pedijatra", 1L });

            migrationBuilder.InsertData(
                table: "DiaperLogs",
                columns: new[] { "Id", "BabyId", "ChangeDate", "ChangeTime", "DeletedAt", "DiaperState", "IsDeleted", "Notes" },
                values: new object[,]
                {
                    { 5001L, 1L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0), null, "mokra", false, null },
                    { 5002L, 1L, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 0, 0, 0), null, "stolica", false, null },
                    { 5003L, 1L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0), null, "kombinovano", false, null },
                    { 5004L, 1L, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 18, 30, 0, 0), null, "mokra", false, null }
                });

            migrationBuilder.InsertData(
                table: "FeedingLogs",
                columns: new[] { "Id", "AmountMl", "AmountUnit", "BabyId", "DeletedAt", "FeedDate", "FeedTime", "FoodTypeId", "IsDeleted", "Notes" },
                values: new object[,]
                {
                    { 5001L, 47m, "g", 1L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0), 11, false, null },
                    { 5002L, 54m, "g", 1L, null, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 30, 0, 0), 22, false, null },
                    { 5003L, 61m, "g", 1L, null, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 15, 0, 0), 16, false, null },
                    { 5004L, 68m, "g", 1L, null, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 19, 0, 0, 0), 20, false, null }
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

            migrationBuilder.UpdateData(
                table: "Pregnancies",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "DueDate", "LmpDate" },
                values: new object[] { new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "SleepLogs",
                columns: new[] { "Id", "BabyId", "DeletedAt", "EndTime", "IsDeleted", "SleepDate", "StartTime" },
                values: new object[,]
                {
                    { 5001L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5002L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) },
                    { 5003L, 1L, null, new TimeSpan(0, 6, 45, 0, 0), false, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 20, 30, 0, 0) }
                });
        }
    }
}
