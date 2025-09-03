using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NatoursApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2e9270a8-3e69-4ebe-9f6c-d9a6648a9209", null, "User", "USER" },
                    { "33a0f51a-57d2-4bd5-b4c7-75d64ed8f88c", null, "Lead Guide", "LEAD_GUIDE" },
                    { "38e3becf-f036-4980-8ea1-2472ace383a1", null, "Guide", "GUIDE" },
                    { "5f549ae3-756c-49d2-8677-c2d02192837b", null, "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e9270a8-3e69-4ebe-9f6c-d9a6648a9209");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33a0f51a-57d2-4bd5-b4c7-75d64ed8f88c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38e3becf-f036-4980-8ea1-2472ace383a1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f549ae3-756c-49d2-8677-c2d02192837b");
        }
    }
}
