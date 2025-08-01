using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla.Migrations
{
    /// <inheritdoc />
    public partial class VillaNubmerToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.CreateTable(
                name: "VillaNumbers",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "integer", nullable: false),
                    SpecialDetails = table.Column<string>(type: "text", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VillaNumbers", x => x.VillaNo);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VillaNumbers");

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenity", "CreatedDate", "Description", "ImageUrl", "Name", "Rate", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 5.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc), "Luxurious villa with ocean view.", "https://example.com/images/villa1.jpg", "Sea Breeze Villa", 350.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 4.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc), "Peaceful villa in the mountains.", "https://example.com/images/villa2.jpg", "Mountain Retreat", 280.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 4.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc), "Villa with a stunning lake view.", "https://example.com/images/villa3.jpg", "Lakeview Paradise", 300.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 3.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc), "Modern villa in the desert.", "https://example.com/images/villa4.jpg", "Desert Oasis", 260.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 4.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc), "Remote villa in the tropical forest.", "https://example.com/images/villa5.jpg", "Jungle Hideout", 240.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, 5.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc), "Luxurious villa in the city center.", "https://example.com/images/villa6.jpg", "City Lights Villa", 400.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, 4.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc), "Cozy villa in a snowy region.", "https://example.com/images/villa7.jpg", "Snowy Chalet", 320.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, 5.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc), "Villa on a private island.", "https://example.com/images/villa8.jpg", "Island Bungalow", 500.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 3.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc), "Elegant historic villa with antique decor.", "https://example.com/images/villa9.jpg", "Historic Manor", 290.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, 5.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc), "Minimalist villa perched on a cliff.", "https://example.com/images/villa10.jpg", "Modern Cliff House", 420.0, new DateTime(2025, 7, 17, 12, 0, 0, 0, DateTimeKind.Utc) }
                });
        }
    }
}
