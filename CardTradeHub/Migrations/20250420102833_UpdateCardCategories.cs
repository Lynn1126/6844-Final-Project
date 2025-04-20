using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CardTradeHub.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCardCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 1,
                column: "Title",
                value: "Charizard First Edition");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 2,
                column: "Category",
                value: "Magic: The Gathering");

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "CardID", "Category", "Condition", "Description", "ImageUrl", "ListedDate", "Price", "Status", "Title", "UserID" },
                values: new object[,]
                {
                    { 4, "Pokemon", "Near Mint", "Extremely rare Pikachu Illustrator promo card", "/images/cards/pikachu-illustrator.jpg", new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 10000.00m, "Available", "Pikachu Illustrator", 1 },
                    { 5, "Magic: The Gathering", "Good", "Vintage Time Walk card from MTG Alpha set", "/images/cards/time-walk.jpg", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3000.00m, "Available", "Time Walk", 1 },
                    { 6, "Basketball", "Near Mint", "1986 Fleer Michael Jordan Rookie Card", "/images/cards/jordan-rookie.jpg", new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 7500.00m, "Available", "Michael Jordan Rookie Card", 1 },
                    { 7, "Baseball", "Good", "1952 Topps Mickey Mantle #311", "/images/cards/mantle-1952.jpg", new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 15000.00m, "Available", "Mickey Mantle 1952 Topps", 1 },
                    { 8, "Football", "Excellent", "2000 Playoff Contenders Tom Brady Rookie Card", "/images/cards/brady-rookie.jpg", new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 8000.00m, "Available", "Tom Brady Rookie Card", 1 },
                    { 9, "Yu-Gi-Oh", "Near Mint", "Complete set of all 5 Exodia pieces", "/images/cards/exodia.jpg", new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 800.00m, "Available", "Exodia the Forbidden One", 1 },
                    { 10, "Basketball", "Mint", "2003-04 Upper Deck LeBron James Rookie Card", "/images/cards/lebron-rookie.jpg", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 6000.00m, "Available", "LeBron James Rookie Card", 1 },
                    { 11, "Pokemon", "Mint", "Shining Mewtwo GX Rainbow Rare card", "/images/cards/mewtwo-gx.jpg", new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 300.00m, "Available", "Mewtwo GX Rainbow Rare", 1 },
                    { 12, "Baseball", "Good", "1933 Goudey Babe Ruth Baseball Card", "/images/cards/ruth-1933.jpg", new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), 12000.00m, "Available", "Babe Ruth 1933 Goudey", 1 },
                    { 13, "Magic: The Gathering", "Excellent", "Beta Edition Mox Sapphire", "/images/cards/mox-sapphire.jpg", new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), 4000.00m, "Available", "Mox Sapphire", 1 },
                    { 14, "Basketball", "Near Mint", "1996-97 Topps Chrome Kobe Bryant Rookie Card", "/images/cards/kobe-rookie.jpg", new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), 5500.00m, "Available", "Kobe Bryant Rookie Card", 1 },
                    { 15, "Yu-Gi-Oh", "Mint", "First Edition Dark Magician Girl", "/images/cards/dark-magician-girl.jpg", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 400.00m, "Available", "Dark Magician Girl", 1 },
                    { 16, "Hockey", "Good", "1979 O-Pee-Chee Wayne Gretzky Rookie Card", "/images/cards/gretzky-rookie.jpg", new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), 9000.00m, "Available", "Wayne Gretzky Rookie", 1 },
                    { 17, "Pokemon", "Near Mint", "Pokemon Movie Ancient Mew Promo Card", "/images/cards/ancient-mew.jpg", new DateTime(2024, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), 200.00m, "Available", "Ancient Mew Promo", 1 },
                    { 18, "Football", "Excellent", "1986 Topps Jerry Rice Rookie Card", "/images/cards/rice-rookie.jpg", new DateTime(2024, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3500.00m, "Available", "Jerry Rice Rookie Card", 1 },
                    { 19, "Magic: The Gathering", "Good", "Alpha Edition Ancestral Recall", "/images/cards/ancestral-recall.jpg", new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), 4500.00m, "Available", "Ancestral Recall", 1 },
                    { 20, "Yu-Gi-Oh", "Near Mint", "First Edition Red Eyes Black Dragon", "/images/cards/red-eyes.jpg", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), 600.00m, "Available", "Red Eyes Black Dragon", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 20);

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 1,
                column: "Title",
                value: "Mint Condition Charizard");

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 2,
                column: "Category",
                value: "Magic");
        }
    }
}
