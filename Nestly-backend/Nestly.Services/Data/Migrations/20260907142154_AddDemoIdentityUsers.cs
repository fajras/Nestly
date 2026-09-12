using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nestly.Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDemoIdentityUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b5b77b5d-65b6-4f32-93f4-3f76b14e6f3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "66acc663-e31d-4ccb-92cf-855d0fb61076", "AQAAAAIAAYagAAAAENpm1iDauSGTM6k6JE5/Jajv/cV78LWdKSr812CIm8L+eXE9lPecXQ1vcCwryAEmNg==", "e60cd8e4-2945-4002-8e4b-9264fcbd742a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "work7b5d-65b6-4f32-93f4-126sko5e6f3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "17fa4c61-f8e0-411f-880c-ffd6f1151831", "AQAAAAIAAYagAAAAEFk7CNL0p1UDOYfDv/py+MiLSkimzo8nCiulflLKUxNSZqd2PbtY5W/ZqlzaxZLYcA==", "630b9e10-6444-4956-b239-fd494001ff23" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "13edc40b-a767-49d0-a194-610e85ae84d0", 0, "022e5102-383e-4cd4-83e6-8e8e23289077", "dzenita.mujic@nestly.com", true, false, null, "DZENITA.MUJIC@NESTLY.COM", "DZENITA.MUJIC@NESTLY.COM", "AQAAAAIAAYagAAAAEBJ+DBJjmjHtZm4BaKJqf3yMSWlVhQRXgSW69xmXS+HM80U2QhGe2eQkPxyVdxff2A==", null, false, "4111f352-bae0-4f55-a40b-9d2d9e498b22", false, "dzenita.mujic@nestly.com" },
                    { "27cc0316-257a-442d-bbe5-46656ec343e0", 0, "d8891b01-b898-4310-96b3-8ac706eea93c", "emina.softic@nestly.com", true, false, null, "EMINA.SOFTIC@NESTLY.COM", "EMINA.SOFTIC@NESTLY.COM", "AQAAAAIAAYagAAAAEHM6QkIMAIxzz0ON+IgajsDzG6/cJuA7orsO6apY3nVJLxWl45RIulNlhCiiilrD5A==", null, false, "d43f3430-4411-4c61-95bd-085214a1f7c1", false, "emina.softic@nestly.com" },
                    { "2d30b0f8-e94e-4a85-be12-bb2bb528b70b", 0, "f0b378aa-ff6e-46dc-a06e-c4a801bbc96d", "selma.begic@nestly.com", true, false, null, "SELMA.BEGIC@NESTLY.COM", "SELMA.BEGIC@NESTLY.COM", "AQAAAAIAAYagAAAAEIu4rT35uBmJ1uzKFEcJWzRUP/Njg5kS3r5A0B8sO/FkNej5znYsZH5u1aP41BkNRA==", null, false, "fafc0c4c-9555-471d-bccf-fb4aad3b921b", false, "selma.begic@nestly.com" },
                    { "323c2855-426d-4726-9520-bdd2da76a5d5", 0, "fa20814a-a935-4f8b-9302-9e0bc1188254", "amila.zukic@nestly.com", true, false, null, "AMILA.ZUKIC@NESTLY.COM", "AMILA.ZUKIC@NESTLY.COM", "AQAAAAIAAYagAAAAEC8PAzWbgQVxl+YNkULRfLe4IGMIyraK+6QUCc+uyttjULqGcsEJLib5hlJAmaYC+A==", null, false, "65621014-8b62-462a-a245-342a4b502c61", false, "amila.zukic@nestly.com" },
                    { "465ced2a-f9e9-4f23-acbb-5a11658737b2", 0, "5353f306-c464-4fe7-892b-0cab649996ca", "nadira.sehic@nestly.com", true, false, null, "NADIRA.SEHIC@NESTLY.COM", "NADIRA.SEHIC@NESTLY.COM", "AQAAAAIAAYagAAAAEKsrY4Zxsjh6typgaFQY8DpQONzQKZpmTVvE83A26l2GwsjScvrk6Z5AmGMjk1N0kQ==", null, false, "14fa20bf-6ebe-4cbe-b626-0830a83dc8dd", false, "nadira.sehic@nestly.com" },
                    { "8339ef91-9659-4986-b918-af66726adf19", 0, "bca54c02-895a-43c4-97ed-b11729f8303e", "amina.hodzic@nestly.com", true, false, null, "AMINA.HODZIC@NESTLY.COM", "AMINA.HODZIC@NESTLY.COM", "AQAAAAIAAYagAAAAEBi5xmBog5ObhFYJEplpUZR82liv8OsCoiw6I8LWaQZynTVVGNXUm5coQFsuWh/b7g==", null, false, "4c4ded28-4508-4b42-b5d5-6ff8b01504ab", false, "amina.hodzic@nestly.com" },
                    { "aa140327-579a-4a94-b635-b4d9a915d279", 0, "84a31bc3-17f4-41d2-857b-331da0e5bb19", "lejla.kovacevic@nestly.com", true, false, null, "LEJLA.KOVACEVIC@NESTLY.COM", "LEJLA.KOVACEVIC@NESTLY.COM", "AQAAAAIAAYagAAAAEK0oQUnHsr7F/9urWKnNvsdYGM3dX4HxopBf/RG/ef9oP+0QhLX11KUygY3Zomfs/g==", null, false, "aed58ab1-ed0d-4061-8abf-6b6bc537c86d", false, "lejla.kovacevic@nestly.com" },
                    { "d66502ad-89df-4de1-90ab-61c153a26d12", 0, "619dfd1e-5f3b-4d16-b6dc-177aac52e45c", "amra.halilovic@nestly.com", true, false, null, "AMRA.HALILOVIC@NESTLY.COM", "AMRA.HALILOVIC@NESTLY.COM", "AQAAAAIAAYagAAAAEOorp5rJupHsB53k2QPtdi+L9CLfYMB3fh2wPon+5g2sXXnKYmzNLGuyAOIlwumQ+Q==", null, false, "115946c9-bfdc-42d4-a0ca-1838c63b8dad", false, "amra.halilovic@nestly.com" },
                    { "d771c068-ae58-4a8c-abc1-88ea3418bcc3", 0, "e726d280-ae22-4ae7-92bf-90735540531f", "ajla.delic@nestly.com", true, false, null, "AJLA.DELIC@NESTLY.COM", "AJLA.DELIC@NESTLY.COM", "AQAAAAIAAYagAAAAEI+IJdkSd/A7p+0pvFGHOmpbRntKf6XBBy+ETEAkQtMGAv+yUyvCniYbzlqnev/1hA==", null, false, "71e9f7ec-4923-4659-a843-4da4980941db", false, "ajla.delic@nestly.com" },
                    { "e02fd7d5-9498-4439-877b-1170b9ee6f4e", 0, "7567be8d-529d-4897-85e9-feb340b86692", "merisa.karic@nestly.com", true, false, null, "MERISA.KARIC@NESTLY.COM", "MERISA.KARIC@NESTLY.COM", "AQAAAAIAAYagAAAAEN2+MEXEAXY3YEYv0zhvws3pgm6rNceqkbEJPd18TVKwpuoUlgkjKVoZ/FNSe7SFjw==", null, false, "7432ed3e-5443-472b-89be-5e1f213712c0", false, "merisa.karic@nestly.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "13edc40b-a767-49d0-a194-610e85ae84d0" },
                    { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "27cc0316-257a-442d-bbe5-46656ec343e0" },
                    { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "2d30b0f8-e94e-4a85-be12-bb2bb528b70b" },
                    { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "323c2855-426d-4726-9520-bdd2da76a5d5" },
                    { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "465ced2a-f9e9-4f23-acbb-5a11658737b2" },
                    { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "8339ef91-9659-4986-b918-af66726adf19" },
                    { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "aa140327-579a-4a94-b635-b4d9a915d279" },
                    { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "d66502ad-89df-4de1-90ab-61c153a26d12" },
                    { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "d771c068-ae58-4a8c-abc1-88ea3418bcc3" },
                    { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "e02fd7d5-9498-4439-877b-1170b9ee6f4e" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "13edc40b-a767-49d0-a194-610e85ae84d0" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "27cc0316-257a-442d-bbe5-46656ec343e0" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "2d30b0f8-e94e-4a85-be12-bb2bb528b70b" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "323c2855-426d-4726-9520-bdd2da76a5d5" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "465ced2a-f9e9-4f23-acbb-5a11658737b2" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "8339ef91-9659-4986-b918-af66726adf19" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "aa140327-579a-4a94-b635-b4d9a915d279" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "d66502ad-89df-4de1-90ab-61c153a26d12" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "d771c068-ae58-4a8c-abc1-88ea3418bcc3" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2b3b5f31-c1d6-4a82-8e1e-2c318c5bc98e", "e02fd7d5-9498-4439-877b-1170b9ee6f4e" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "13edc40b-a767-49d0-a194-610e85ae84d0");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "27cc0316-257a-442d-bbe5-46656ec343e0");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2d30b0f8-e94e-4a85-be12-bb2bb528b70b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "323c2855-426d-4726-9520-bdd2da76a5d5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "465ced2a-f9e9-4f23-acbb-5a11658737b2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8339ef91-9659-4986-b918-af66726adf19");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aa140327-579a-4a94-b635-b4d9a915d279");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d66502ad-89df-4de1-90ab-61c153a26d12");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d771c068-ae58-4a8c-abc1-88ea3418bcc3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e02fd7d5-9498-4439-877b-1170b9ee6f4e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b5b77b5d-65b6-4f32-93f4-3f76b14e6f3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c4ca6493-b0da-4241-a3d0-bc53439770de", "AQAAAAIAAYagAAAAEOdhAHVfifSq3iEPqoo8iNNXpItOIsGZzOcKxSl2Vj9K/IJHhb+i2JdcoEEkA7eedg==", "13164e58-823f-4bc9-85eb-efa09e00c24d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "work7b5d-65b6-4f32-93f4-126sko5e6f3c",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6df972ea-a3cc-441a-9b4c-ea5743ee60a3", "AQAAAAIAAYagAAAAEPSQv0jMlTTXS5MH8/tyxuer/rzGtR8/iHAZ0NxzLH1JBNzbk3Vzxk7YApMu5gPpog==", "dd1db4ca-763a-40ac-a7ef-9945094aba47" });
        }
    }
}
