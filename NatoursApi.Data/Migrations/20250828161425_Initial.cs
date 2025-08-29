using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NatoursApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    MaxGroupSize = table.Column<int>(type: "integer", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    RatingsAverage = table.Column<decimal>(type: "numeric(3,1)", precision: 3, scale: 1, nullable: false),
                    RatingsQuantity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    PriceDiscount = table.Column<decimal>(type: "numeric", nullable: true),
                    Summary = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TourStartDates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TourId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourStartDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourStartDates_Tours_TourId",
                        column: x => x.TourId,
                        principalTable: "Tours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tours_Name",
                table: "Tours",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_Price_RatingsAverage",
                table: "Tours",
                columns: new[] { "Price", "RatingsAverage" });

            migrationBuilder.CreateIndex(
                name: "IX_Tours_Slug",
                table: "Tours",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TourStartDates_TourId_Date",
                table: "TourStartDates",
                columns: new[] { "TourId", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TourStartDates");

            migrationBuilder.DropTable(
                name: "Tours");
        }
    }
}
