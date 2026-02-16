using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.DAL.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class seedIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "isDeafult", "isDeleted" },
                values: new object[,]
                {
                    { "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1", "42C40519-7422-4D96-843A-540DFE5E4455", "Admin", "ADMIN", false, false },
                    { "FA41809D-F0CF-48B4-A8B1-C29C42B9A2C4", "B972488C-A2A7-4BB7-83F6-1FF7D84C09D4", "Member", "MEMBER", true, false }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "firstName", "lastName" },
                values: new object[] { "98673a28-c8af-45c0-9efd-3f4af6e42529", 0, "DCA6A18C-C782-4E5E-A755-22C6002FCD0B", "admin@survey-basket.com", true, false, null, "ADMIN@SURVEY-BASKET.COM", "ADMIN@SURVEY-BASKET.COM", "AQAAAAIAAYagAAAAEL2v+4MLrrl1SN1l7aKIJHbzA8/s3AZeKN1H0RgqKqEVRoslRl7Uzk7IsxflEzEbhA==", null, false, "E882853128BE4715B5C795146A171981", false, "admin@survey-basket.com", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permissions", "polls:read", "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1" },
                    { 2, "Permissions", "polls:add", "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1" },
                    { 3, "Permissions", "polls:update", "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1" },
                    { 4, "Permissions", "polls:delete", "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1" },
                    { 5, "Permissions", "questions:read", "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1" },
                    { 6, "Permissions", "questions:add", "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1" },
                    { 7, "Permissions", "questions:update", "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1" },
                    { 8, "Permissions", "users:read", "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1" },
                    { 9, "Permissions", "users:add", "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1" },
                    { 10, "Permissions", "users:update", "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1" },
                    { 11, "Permissions", "roles:read", "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1" },
                    { 12, "Permissions", "roles:add", "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1" },
                    { 13, "Permissions", "roles:update", "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1" },
                    { 14, "Permissions", "results:read", "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1", "98673a28-c8af-45c0-9efd-3f4af6e42529" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "FA41809D-F0CF-48B4-A8B1-C29C42B9A2C4");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1", "98673a28-c8af-45c0-9efd-3f4af6e42529" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31DBF7C3-FB22-4CBB-A1C5-3E84FC009AD1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "98673a28-c8af-45c0-9efd-3f4af6e42529");
        }
    }
}
