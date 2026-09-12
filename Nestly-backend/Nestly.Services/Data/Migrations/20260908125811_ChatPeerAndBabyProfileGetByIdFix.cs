using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nestly.Services.Migrations.NestlyDb
{
    /// <inheritdoc />
    public partial class ChatPeerAndBabyProfileGetByIdFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 5003);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 9003);

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 14, 12, 58, 7, 755, DateTimeKind.Utc).AddTicks(7683));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 17, 12, 58, 7, 755, DateTimeKind.Utc).AddTicks(7693));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 19, 12, 58, 7, 755, DateTimeKind.Utc).AddTicks(7696));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 20, 12, 58, 7, 755, DateTimeKind.Utc).AddTicks(7698));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 22, 12, 58, 7, 755, DateTimeKind.Utc).AddTicks(7700));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 24, 12, 58, 7, 755, DateTimeKind.Utc).AddTicks(7703));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 12, 58, 7, 755, DateTimeKind.Utc).AddTicks(7705));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 29, 12, 58, 7, 755, DateTimeKind.Utc).AddTicks(7707));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 9L,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 31, 12, 58, 7, 755, DateTimeKind.Utc).AddTicks(7709));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 10L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 2, 12, 58, 7, 755, DateTimeKind.Utc).AddTicks(7711));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 11L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 4, 12, 58, 7, 755, DateTimeKind.Utc).AddTicks(7713));

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 12L,
                column: "CreatedAt",
                value: new DateTime(2026, 9, 6, 12, 58, 7, 755, DateTimeKind.Utc).AddTicks(7715));

            migrationBuilder.UpdateData(
                table: "ChatConversations",
                keyColumn: "Id",
                keyValue: 5001L,
                columns: new[] { "CreatedAt", "User2Id" },
                values: new object[] { new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L });

            migrationBuilder.UpdateData(
                table: "ChatConversations",
                keyColumn: "Id",
                keyValue: 5002L,
                columns: new[] { "CreatedAt", "User1Id", "User2Id" },
                values: new object[] { new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, 6L });

            migrationBuilder.UpdateData(
                table: "ChatConversations",
                keyColumn: "Id",
                keyValue: 5003L,
                columns: new[] { "CreatedAt", "User1Id", "User2Id" },
                values: new object[] { new DateTime(2026, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, 9L });

            migrationBuilder.UpdateData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9001L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Zdravo Amina! Vidjela sam da je i tvoja beba rođena otprilike u isto vrijeme kad i Emma.", new DateTime(2026, 9, 2, 11, 15, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9002L,
                columns: new[] { "Content", "CreatedAt", "SenderId" },
                values: new object[] { "Zdravo! Da, razlika je par sedmica. Kako vama ide sa uvođenjem čvrste hrane?", new DateTime(2026, 9, 2, 11, 20, 0, 0, DateTimeKind.Unspecified), 3L });

            migrationBuilder.UpdateData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9003L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Polako, Emma najviše voli povrće, malo je izbirljivija sa voćem.", new DateTime(2026, 9, 2, 11, 25, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9004L,
                columns: new[] { "Content", "CreatedAt", "SenderId" },
                values: new object[] { "Isto i moja! Probaj joj miksati voće sa jogurtom, meni je to upalilo.", new DateTime(2026, 9, 2, 11, 30, 0, 0, DateTimeKind.Unspecified), 3L });

            migrationBuilder.UpdateData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9005L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Hvala na ideji, probat ću sutra.", new DateTime(2026, 9, 2, 11, 32, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9006L,
                columns: new[] { "Content", "CreatedAt", "SenderId" },
                values: new object[] { "Javi mi kako je prošlo. Kako spava noću kod vas?", new DateTime(2026, 9, 4, 20, 10, 0, 0, DateTimeKind.Unspecified), 3L });

            migrationBuilder.UpdateData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9007L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Prilično dobro, budi se jednom oko 3h ujutro pa opet zaspi.", new DateTime(2026, 9, 4, 20, 15, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "ChatMessages",
                columns: new[] { "Id", "Content", "ConversationId", "CreatedAt", "SenderId" },
                values: new object[,]
                {
                    { 9008L, "To je super u ovom uzrastu! Mi se još budimo dva-tri puta.", 5001L, new DateTime(2026, 9, 4, 20, 18, 0, 0, DateTimeKind.Unspecified), 3L },
                    { 9009L, "Emina, jesi li ti već bila na vakcinaciji ovaj mjesec?", 5002L, new DateTime(2026, 9, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), 1L },
                    { 9010L, "Jesam, prošle sedmice. Prošlo je bez problema, samo malo temperature uveče.", 5002L, new DateTime(2026, 9, 4, 9, 5, 0, 0, DateTimeKind.Unspecified), 6L },
                    { 9011L, "Dobro je znati, mi imamo zakazano za par dana pa sam malo nervozna.", 5002L, new DateTime(2026, 9, 4, 9, 8, 0, 0, DateTimeKind.Unspecified), 1L },
                    { 9012L, "Nema razloga za brigu, samo ponesi sirup za temperaturu za svaki slučaj.", 5002L, new DateTime(2026, 9, 4, 9, 10, 0, 0, DateTimeKind.Unspecified), 6L },
                    { 9013L, "Hoću, hvala ti puno!", 5002L, new DateTime(2026, 9, 4, 9, 12, 0, 0, DateTimeKind.Unspecified), 1L },
                    { 9014L, "Nema na čemu, javi kako je prošlo.", 5002L, new DateTime(2026, 9, 4, 9, 13, 0, 0, DateTimeKind.Unspecified), 6L },
                    { 9015L, "Bok Amela, kako vam ide sa spavanjem po danu? Moja je totalno preokrenula raspored.", 5003L, new DateTime(2026, 9, 6, 15, 0, 0, 0, DateTimeKind.Unspecified), 9L },
                    { 9016L, "Haha, i mi smo prošli kroz tu fazu prije mjesec dana. Pomoglo je da uvedemo isto vrijeme spavanja svaki dan.", 5003L, new DateTime(2026, 9, 6, 15, 10, 0, 0, DateTimeKind.Unspecified), 1L },
                    { 9017L, "Probat ću to. A kako vam ide sa zubićima, je li već počela nicati?", 5003L, new DateTime(2026, 9, 6, 15, 15, 0, 0, DateTimeKind.Unspecified), 9L },
                    { 9018L, "Da, prvi zubić se pojavio prije nedjelju dana. Malo je bila nemirna par dana.", 5003L, new DateTime(2026, 9, 6, 15, 18, 0, 0, DateTimeKind.Unspecified), 1L },
                    { 9019L, "Hvala na savjetu, javit ću se ako bude trebalo još pitanja!", 5003L, new DateTime(2026, 9, 6, 15, 20, 0, 0, DateTimeKind.Unspecified), 9L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9008L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9009L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9010L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9011L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9012L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9013L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9014L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9015L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9016L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9017L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9018L);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9019L);

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
                column: "CreatedAt",
                value: new DateTime(2026, 8, 16, 22, 12, 24, 904, DateTimeKind.Utc).AddTicks(1168));

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

            migrationBuilder.UpdateData(
                table: "ChatConversations",
                keyColumn: "Id",
                keyValue: 5001L,
                columns: new[] { "CreatedAt", "User2Id" },
                values: new object[] { new DateTime(2026, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 2L });

            migrationBuilder.UpdateData(
                table: "ChatConversations",
                keyColumn: "Id",
                keyValue: 5002L,
                columns: new[] { "CreatedAt", "User1Id", "User2Id" },
                values: new object[] { new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 3L, 2L });

            migrationBuilder.UpdateData(
                table: "ChatConversations",
                keyColumn: "Id",
                keyValue: 5003L,
                columns: new[] { "CreatedAt", "User1Id", "User2Id" },
                values: new object[] { new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 9L, 2L });

            migrationBuilder.UpdateData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9001L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Poštovana doktorice, Emma je sada napunila 6 mjeseci.", new DateTime(2026, 9, 2, 10, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9002L,
                columns: new[] { "Content", "CreatedAt", "SenderId" },
                values: new object[] { "Čestitam! Kako napreduje uvođenje čvrste hrane?", new DateTime(2026, 9, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), 2L });

            migrationBuilder.UpdateData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9003L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Uglavnom dobro prihvata povrće, malo je izbirljivija sa voćem.", new DateTime(2026, 9, 3, 10, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9004L,
                columns: new[] { "Content", "CreatedAt", "SenderId" },
                values: new object[] { "To je sasvim uobičajeno, nastavite sa raznovrsnom ponudom bez pritiska.", new DateTime(2026, 9, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), 2L });

            migrationBuilder.UpdateData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9005L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Hvala vam! Još jedno pitanje - koliko sati sna je normalno u ovom uzrastu?", new DateTime(2026, 9, 5, 10, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9006L,
                columns: new[] { "Content", "CreatedAt", "SenderId" },
                values: new object[] { "Obično oko 14-15 sati ukupno, uključujući dnevne dremke.", new DateTime(2026, 9, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), 2L });

            migrationBuilder.UpdateData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 9007L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Razumijem, hvala puno na pomoći!", new DateTime(2026, 9, 6, 10, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "ChatMessages",
                columns: new[] { "Id", "Content", "ConversationId", "CreatedAt", "SenderId" },
                values: new object[,]
                {
                    { 5001L, "Poštovana doktorice, imam pitanje vezano za ishranu bebe.", 5001L, new DateTime(2026, 9, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), 1L },
                    { 5002L, "Izvolite, slobodno pitajte.", 5001L, new DateTime(2026, 9, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), 2L },
                    { 5003L, "Da li Emma može jesti jaja u ovoj fazi?", 5001L, new DateTime(2026, 9, 3, 10, 0, 0, 0, DateTimeKind.Unspecified), 1L },
                    { 5004L, "Da, dobro termički obrađena jaja su u redu od 8. mjeseca.", 5001L, new DateTime(2026, 9, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), 2L },
                    { 5005L, "Zdravo doktorice, imam mučninu svako jutro, je li to zabrinjavajuće?", 5002L, new DateTime(2026, 9, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), 3L },
                    { 5006L, "Nije, to je uobičajeno u prvom trimestru. Javite se ako povraćanje bude jako učestalo.", 5002L, new DateTime(2026, 9, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), 2L },
                    { 5007L, "Hvala vam puno!", 5002L, new DateTime(2026, 9, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), 3L },
                    { 5008L, "Poštovani, Lamija odbija čvrstu hranu zadnjih dana.", 5003L, new DateTime(2026, 9, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), 9L },
                    { 5009L, "To je uobičajena faza, nastavite nuditi raznovrsnu hranu bez pritiska.", 5003L, new DateTime(2026, 9, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), 2L },
                    { 5010L, "U redu, hvala na savjetu.", 5003L, new DateTime(2026, 9, 6, 10, 0, 0, 0, DateTimeKind.Unspecified), 9L }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "CreatedAt", "IsRead", "Message", "Title", "UserId" },
                values: new object[,]
                {
                    { 5003, new DateTime(2026, 9, 5, 9, 30, 0, 0, DateTimeKind.Unspecified), true, "Imate novu poruku od doktora.", "Nova poruka", 9L },
                    { 9003, new DateTime(2026, 9, 5, 9, 30, 0, 0, DateTimeKind.Unspecified), true, "Imate novu poruku od doktora.", "Nova poruka", 1L }
                });
        }
    }
}
