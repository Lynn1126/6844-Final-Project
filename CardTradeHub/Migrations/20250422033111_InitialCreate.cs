using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CardTradeHub.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false, defaultValue: "Customer"),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Condition = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    ListedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    UserID = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardID);
                    table.ForeignKey(
                        name: "FK_Cards_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BuyerID = table.Column<string>(type: "TEXT", nullable: false),
                    SellerID = table.Column<string>(type: "TEXT", nullable: false),
                    CardID = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    PaymentMethod = table.Column<string>(type: "TEXT", nullable: false),
                    TransactionReference = table.Column<string>(type: "TEXT", nullable: false),
                    ShippingAddress = table.Column<string>(type: "TEXT", nullable: false),
                    TrackingNumber = table.Column<string>(type: "TEXT", nullable: false),
                    HasDispute = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisputeReason = table.Column<string>(type: "TEXT", nullable: false),
                    DisputeDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DisputeStatus = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_BuyerID",
                        column: x => x.BuyerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_SellerID",
                        column: x => x.SellerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Cards_CardID",
                        column: x => x.CardID,
                        principalTable: "Cards",
                        principalColumn: "CardID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastLoginDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RegisterDate", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, "839b18ec-183d-4361-83c7-0c308108cda0", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@cardtradehub.com", true, null, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, "ADMIN@CARDTRADEHUB.COM", "ADMIN", "AQAAAAIAAYagAAAAEJuEwQC2quN17C/GkpZmTEo9flggVob8vU7KOXKAC2JTXeHV9wsZZo3UtM13nxgMkA==", null, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Admin", "8f94cb50-2d65-45b9-b25c-afcea2aa7f16", false, "admin" },
                    { "2", 0, "2ce09d1e-4e96-48da-b872-eb9c8e31b743", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seller@cardtradehub.com", true, null, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, "SELLER@CARDTRADEHUB.COM", "SELLER", "AQAAAAIAAYagAAAAEL3K/nt8D7tLEuDlqFvqy67GFRijsrh8B01mvdytxYaAeya/rEmXzB6xlEa85LAdVw==", null, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "User", "4b21235c-675c-4ee7-a32d-6370964c23bd", false, "seller" }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "CardID", "Category", "Condition", "Description", "ImageUrl", "ListedDate", "Price", "Status", "Title", "UserID" },
                values: new object[,]
                {
                    { 1, "Pokemon", "Mint", "2019 Pikachu Pokemon Center Kyoto", "/images/cards/pikachu.jpg", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 200.00m, "Available", "Pikachu Pokemon Center Kyoto", "2" },
                    { 2, "Basketball", "Good", "Kobe Bryant 2012-13 Limited Auto", "/images/cards/kobe1.jpg", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3000.00m, "Available", "Kobe Bryant 2012-13 Limited Auto", "2" },
                    { 3, "Football", "Excellent", "2014 Prizm World Cup Lionel Messi Auto PSA 10", "/images/cards/messi.jpg", new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 5000.00m, "Available", "2014 Prizm World Cup Lionel Messi Auto PSA 10", "2" },
                    { 4, "Basketball", "Near Mint", "Yao Ming 2016-17 Panini Flawless Autograph", "/images/cards/yao.jpg", new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1000.00m, "Available", "Yao Ming 2016-17 Panini Flawless Autograph", "2" },
                    { 5, "Magic: The Gathering", "Good", "1993 Unlimited Black Lotus R", "/images/cards/blacklotus.jpg", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3000.00m, "Available", "Black Lotus R", "2" },
                    { 6, "Basketball", "Near Mint", "National Treasures Kevin Durant Auto USA", "/images/cards/durant.jpg", new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 1500.00m, "Available", "National Treasures Kevin Durant Auto USA", "2" },
                    { 7, "Baseball", "Good", "2019 Topps Chrome Shohei Ohtani", "/images/cards/shohei.jpg", new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 50.00m, "Available", "Shohei Ohtani Topps Chrome", "2" },
                    { 8, "Football", "Excellent", "2022-23 Panini Eminence David Beckham Autograph", "/images/cards/beckham.jpg", new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 1000.00m, "Available", "David Beckham Autograph Card", "2" },
                    { 9, "Yu-Gi-Oh", "Near Mint", "Red Eyes Black Dragon PSA 10", "/images/cards/bluedragon.jpg", new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), 8000.00m, "Available", "Red Eyes Black Dragon", "2" },
                    { 10, "Basketball", "Mint", "Kobe Bryant Gold Standard Superscribe Autograph", "/images/cards/kobe2.jpg", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1500.00m, "Available", "Kobe Bryant Gold Standard Superscribe", "2" },
                    { 11, "Pokemon", "Mint", "Shining Mewtwo GX Rainbow Rare card", "/images/cards/mewtwo-gx.jpg", new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 300.00m, "Available", "Mewtwo GX Rainbow Rare", "2" },
                    { 12, "Baseball", "Good", "2021 Topps Definitive Juan Soto Autograph Card", "/images/cards/soto.jpg", new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), 2000.00m, "Available", "Juan Soto Autograph Card", "2" },
                    { 13, "Magic: The Gathering", "Excellent", "Beta Edition Mox Sapphire", "/images/cards/mox-sapphire.jpg", new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), 4000.00m, "Available", "Mox Sapphire", "2" },
                    { 14, "Basketball", "Near Mint", "2020-21 Kobe Bryant Hoops Slam", "/images/cards/kobe3.jpg", new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), 500.00m, "Available", "Kobe Bryant Hoops Slam", "2" },
                    { 15, "Yu-Gi-Oh", "Mint", "First Edition Dark Magician Girl", "/images/cards/dark-magician-girl.jpg", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 400.00m, "Available", "Dark Magician Girl", "2" },
                    { 16, "Hockey", "Good", "1979 O-Pee-Chee Wayne Gretzky Rookie Card", "/images/cards/gretzky-rookie.jpg", new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Utc), 9000.00m, "Available", "Wayne Gretzky Rookie", "2" },
                    { 17, "Pokemon", "Mint", "Pokemon Lillie Full Art PSA 10", "/images/cards/lillie.jpg", new DateTime(2024, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc), 200.00m, "Available", "Lillie Full Art PSA 10", "2" },
                    { 18, "Basketball", "Excellent", "Kareem Abdul-Jabbar Unparalleled Autograph", "/images/cards/jabbar.jpg", new DateTime(2024, 1, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3500.00m, "Available", "Kareem Abdul-Jabbar Unparalleled Autograph", "2" },
                    { 19, "Baseball", "Good", "Topps Chrome Juan Soto Rookie Card PSA 10", "/images/cards/soto2.jpg", new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Utc), 400.00m, "Available", "Juan Soto Rookie Card", "2" },
                    { 20, "Basketball", "Near Mint", "Stephen Curry 2018-19 Immaculate Moments", "/images/cards/curry.jpg", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), 2600.00m, "Available", "Stephen Curry 2018-19 Immaculate Moments", "2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UserID",
                table: "Cards",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BuyerID",
                table: "Transactions",
                column: "BuyerID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CardID",
                table: "Transactions",
                column: "CardID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SellerID",
                table: "Transactions",
                column: "SellerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
