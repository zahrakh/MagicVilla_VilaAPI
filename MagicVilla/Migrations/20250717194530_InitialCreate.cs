using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Villas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Rate = table.Column<double>(type: "double precision", nullable: false),
                    Amenity = table.Column<double>(type: "double precision", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villas", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Villas");
        }
    }
}
