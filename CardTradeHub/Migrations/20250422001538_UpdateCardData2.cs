using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardTradeHub.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCardData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "2019 Pikachu Pokemon Center Kyoto", "/images/cards/pikachu.jpg", 200.00m, "Pikachu Pokemon Center Kyoto" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 2,
                columns: new[] { "Category", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "Basketball", "Kobe Bryant 2012-13 Limited Auto", "/images/cards/kobe1.jpg", 3000.00m, "Kobe Bryant 2012-13 Limited Auto" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 3,
                columns: new[] { "Category", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "Football", "2014 Prizm World Cup Lionel Messi Auto PSA 10", "/images/cards/messi.jpg", 5000.00m, "2014 Prizm World Cup Lionel Messi Auto PSA 10" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 4,
                columns: new[] { "Category", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "Basketball", "Yao Ming 2016-17 Panini Flawless Autograph", "/images/cards/yao.jpg", 1000.00m, "Yao Ming 2016-17 Panini Flawless Autograph" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 5,
                columns: new[] { "Description", "ImageUrl", "Title" },
                values: new object[] { "1993 Unlimited Black Lotus R", "/images/cards/blacklotus.jpg", "Black Lotus R" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 6,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "National Treasures Kevin Durant Auto USA", "/images/cards/durant.jpg", 1500.00m, "National Treasures Kevin Durant Auto USA" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 7,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "2019 Topps Chrome Shohei Ohtani", "/images/cards/shohei.jpg", 50.00m, "Shohei Ohtani Topps Chrome" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 8,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "2022-23 Panini Eminence David Beckham Autograph", "/images/cards/beckham.jpg", 1000.00m, "David Beckham Autograph Card" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 9,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "Red Eyes Black Dragon PSA 10", "/images/cards/bluedragon.jpg", 8000.00m, "Red Eyes Black Dragon" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 10,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "Kobe Bryant Gold Standard Superscribe Autograph", "/images/cards/kobe2.jpg", 1500.00m, "Kobe Bryant Gold Standard Superscribe" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 12,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "2021 Topps Definitive Juan Soto Autograph Card", "/images/cards/soto.jpg", 2000.00m, "Juan Soto Autograph Card" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 14,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "2020-21 Kobe Bryant Hoops Slam", "/images/cards/kobe3.jpg", 500.00m, "Kobe Bryant Hoops Slam" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 17,
                columns: new[] { "Condition", "Description", "ImageUrl", "Title" },
                values: new object[] { "Mint", "Pokemon Lillie Full Art PSA 10", "/images/cards/lillie.jpg", "Lillie Full Art PSA 10" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 18,
                columns: new[] { "Category", "Description", "ImageUrl", "Title" },
                values: new object[] { "Basketball", "Kareem Abdul-Jabbar Unparalleled Autograph", "/images/cards/jabbar.jpg", "Kareem Abdul-Jabbar Unparalleled Autograph" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 19,
                columns: new[] { "Category", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "Baseball", "Topps Chrome Juan Soto Rookie Card PSA 10", "/images/cards/soto2.jpg", 400.00m, "Juan Soto Rookie Card" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 20,
                columns: new[] { "Category", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "Basketball", "Stephen Curry 2018-19 Immaculate Moments", "/images/cards/curry.jpg", 2600.00m, "Stephen Curry 2018-19 Immaculate Moments" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "First edition Charizard in mint condition", "/images/cards/charizard.jpg", 1000.00m, "Charizard First Edition" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 2,
                columns: new[] { "Category", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "Magic: The Gathering", "Rare Black Lotus card from Magic: The Gathering", "/images/cards/black-lotus.jpg", 5000.00m, "Black Lotus MTG" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 3,
                columns: new[] { "Category", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "Yu-Gi-Oh", "Classic Yu-Gi-Oh! card in excellent condition", "/images/cards/blue-eyes.jpg", 500.00m, "Blue-Eyes White Dragon" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 4,
                columns: new[] { "Category", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "Pokemon", "Extremely rare Pikachu Illustrator promo card", "/images/cards/pikachu-illustrator.jpg", 10000.00m, "Pikachu Illustrator" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 5,
                columns: new[] { "Description", "ImageUrl", "Title" },
                values: new object[] { "Vintage Time Walk card from MTG Alpha set", "/images/cards/time-walk.jpg", "Time Walk" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 6,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "1986 Fleer Michael Jordan Rookie Card", "/images/cards/jordan-rookie.jpg", 7500.00m, "Michael Jordan Rookie Card" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 7,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "1952 Topps Mickey Mantle #311", "/images/cards/mantle-1952.jpg", 15000.00m, "Mickey Mantle 1952 Topps" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 8,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "2000 Playoff Contenders Tom Brady Rookie Card", "/images/cards/brady-rookie.jpg", 8000.00m, "Tom Brady Rookie Card" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 9,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "Complete set of all 5 Exodia pieces", "/images/cards/exodia.jpg", 800.00m, "Exodia the Forbidden One" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 10,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "2003-04 Upper Deck LeBron James Rookie Card", "/images/cards/lebron-rookie.jpg", 6000.00m, "LeBron James Rookie Card" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 12,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "1933 Goudey Babe Ruth Baseball Card", "/images/cards/ruth-1933.jpg", 12000.00m, "Babe Ruth 1933 Goudey" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 14,
                columns: new[] { "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "1996-97 Topps Chrome Kobe Bryant Rookie Card", "/images/cards/kobe-rookie.jpg", 5500.00m, "Kobe Bryant Rookie Card" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 17,
                columns: new[] { "Condition", "Description", "ImageUrl", "Title" },
                values: new object[] { "Near Mint", "Pokemon Movie Ancient Mew Promo Card", "/images/cards/ancient-mew.jpg", "Ancient Mew Promo" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 18,
                columns: new[] { "Category", "Description", "ImageUrl", "Title" },
                values: new object[] { "Football", "1986 Topps Jerry Rice Rookie Card", "/images/cards/rice-rookie.jpg", "Jerry Rice Rookie Card" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 19,
                columns: new[] { "Category", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "Magic: The Gathering", "Alpha Edition Ancestral Recall", "/images/cards/ancestral-recall.jpg", 4500.00m, "Ancestral Recall" });

            migrationBuilder.UpdateData(
                table: "Cards",
                keyColumn: "CardID",
                keyValue: 20,
                columns: new[] { "Category", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { "Yu-Gi-Oh", "First Edition Red Eyes Black Dragon", "/images/cards/red-eyes.jpg", 600.00m, "Red Eyes Black Dragon" });
        }
    }
}
