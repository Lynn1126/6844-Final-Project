using Microsoft.EntityFrameworkCore;
using CardTradeHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CardTradeHub.Data
{
    public class CardTradeHubContext : IdentityDbContext<User>
    {
        private static readonly DateTime _seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public CardTradeHubContext(DbContextOptions<CardTradeHubContext> options)
            : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("Customer");

            modelBuilder.Entity<User>()
                .Property(u => u.IsActive)
                .HasDefaultValue(true);

            // Seed admin user
            var hasher = new PasswordHasher<User>();
            var adminUser = new User
            {
                Id = "1", // IdentityUser uses string Id
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@cardtradehub.com",
                NormalizedEmail = "ADMIN@CARDTRADEHUB.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin123!"),
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = "Admin",
                IsActive = true,
                CreatedAt = _seedDate,
                RegisterDate = _seedDate,
                LastLoginDate = _seedDate
            };

            // Seed regular user
            var regularUser = new User
            {
                Id = "2",
                UserName = "seller",
                NormalizedUserName = "SELLER",
                Email = "seller@cardtradehub.com",
                NormalizedEmail = "SELLER@CARDTRADEHUB.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Seller123!"),
                SecurityStamp = Guid.NewGuid().ToString(),
                Role = "User",
                IsActive = true,
                CreatedAt = _seedDate,
                RegisterDate = _seedDate,
                LastLoginDate = _seedDate
            };

            modelBuilder.Entity<User>().HasData(adminUser, regularUser);

            // Configure relationships
            modelBuilder.Entity<Card>()
                .HasOne(c => c.User)
                .WithMany(u => u.Cards)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Buyer)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.BuyerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Seller)
                .WithMany()
                .HasForeignKey(t => t.SellerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data for cards - Gaming Cards
            var cards = new List<Card>
            {
                new Card
                {
                    CardID = 1,
                    Title = "Pikachu Pokemon Center Kyoto",
                    Description = "2019 Pikachu Pokemon Center Kyoto",
                    Category = "Pokemon",
                    Condition = "Mint",
                    Price = 200.00M,
                    ImageUrl = "/images/cards/pikachu.jpg",
                    ListedDate = _seedDate,
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 2,
                    Title = "Kobe Bryant 2012-13 Limited Auto",
                    Description = "Kobe Bryant 2012-13 Limited Auto",
                    Category = "Basketball",
                    Condition = "Good",
                    Price = 3000.00M,
                    ImageUrl = "/images/cards/kobe1.jpg",
                    ListedDate = _seedDate.AddDays(1),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 3,
                    Title = "2014 Prizm World Cup Lionel Messi Auto PSA 10",
                    Description = "2014 Prizm World Cup Lionel Messi Auto PSA 10",
                    Category = "Football",
                    Condition = "Excellent",
                    Price = 5000.00M,
                    ImageUrl = "/images/cards/messi.jpg",
                    ListedDate = _seedDate.AddDays(2),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 4,
                    Title = "Yao Ming 2016-17 Panini Flawless Autograph",
                    Description = "Yao Ming 2016-17 Panini Flawless Autograph",
                    Category = "Basketball",
                    Condition = "Near Mint",
                    Price = 1000.00M,
                    ImageUrl = "/images/cards/yao.jpg",
                    ListedDate = _seedDate.AddDays(3),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 5,
                    Title = "Black Lotus R",
                    Description = "1993 Unlimited Black Lotus R",
                    Category = "Magic: The Gathering",
                    Condition = "Good",
                    Price = 3000.00M,
                    ImageUrl = "/images/cards/blacklotus.jpg",
                    ListedDate = _seedDate.AddDays(4),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                // Sports Cards
                new Card
                {
                    CardID = 6,
                    Title = "National Treasures Kevin Durant Auto USA",
                    Description = "National Treasures Kevin Durant Auto USA",
                    Category = "Basketball",
                    Condition = "Near Mint",
                    Price = 1500.00M,
                    ImageUrl = "/images/cards/durant.jpg",
                    ListedDate = _seedDate.AddDays(5),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 7,
                    Title = "Shohei Ohtani Topps Chrome",
                    Description = "2019 Topps Chrome Shohei Ohtani",
                    Category = "Baseball",
                    Condition = "Good",
                    Price = 50.00M,
                    ImageUrl = "/images/cards/shohei.jpg",
                    ListedDate = _seedDate.AddDays(6),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 8,
                    Title = "David Beckham Autograph Card",
                    Description = "2022-23 Panini Eminence David Beckham Autograph",
                    Category = "Football",
                    Condition = "Excellent",
                    Price = 1000.00M,
                    ImageUrl = "/images/cards/beckham.jpg",
                    ListedDate = _seedDate.AddDays(7),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 9,
                    Title = "Red Eyes Black Dragon",
                    Description = "Red Eyes Black Dragon PSA 10",
                    Category = "Yu-Gi-Oh",
                    Condition = "Near Mint",
                    Price = 8000.00M,
                    ImageUrl = "/images/cards/bluedragon.jpg",
                    ListedDate = _seedDate.AddDays(8),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 10,
                    Title = "Kobe Bryant Gold Standard Superscribe",
                    Description = "Kobe Bryant Gold Standard Superscribe Autograph",
                    Category = "Basketball",
                    Condition = "Mint",
                    Price = 1500.00M,
                    ImageUrl = "/images/cards/kobe2.jpg",
                    ListedDate = _seedDate.AddDays(9),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 11,
                    Title = "Mewtwo GX Rainbow Rare",
                    Description = "Shining Mewtwo GX Rainbow Rare card",
                    Category = "Pokemon",
                    Condition = "Mint",
                    Price = 300.00M,
                    ImageUrl = "/images/cards/mewtwo-gx.jpg",
                    ListedDate = _seedDate.AddDays(10),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 12,
                    Title = "Juan Soto Autograph Card",
                    Description = "2021 Topps Definitive Juan Soto Autograph Card",
                    Category = "Baseball",
                    Condition = "Good",
                    Price = 2000.00M,
                    ImageUrl = "/images/cards/soto.jpg",
                    ListedDate = _seedDate.AddDays(11),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 13,
                    Title = "Mox Sapphire",
                    Description = "Beta Edition Mox Sapphire",
                    Category = "Magic: The Gathering",
                    Condition = "Excellent",
                    Price = 4000.00M,
                    ImageUrl = "/images/cards/mox-sapphire.jpg",
                    ListedDate = _seedDate.AddDays(12),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 14,
                    Title = "Kobe Bryant Hoops Slam",
                    Description = "2020-21 Kobe Bryant Hoops Slam",
                    Category = "Basketball",
                    Condition = "Near Mint",
                    Price = 500.00M,
                    ImageUrl = "/images/cards/kobe3.jpg",
                    ListedDate = _seedDate.AddDays(13),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 15,
                    Title = "Dark Magician Girl",
                    Description = "First Edition Dark Magician Girl",
                    Category = "Yu-Gi-Oh",
                    Condition = "Mint",
                    Price = 400.00M,
                    ImageUrl = "/images/cards/dark-magician-girl.jpg",
                    ListedDate = _seedDate.AddDays(14),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 16,
                    Title = "Wayne Gretzky Rookie",
                    Description = "1979 O-Pee-Chee Wayne Gretzky Rookie Card",
                    Category = "Hockey",
                    Condition = "Good",
                    Price = 9000.00M,
                    ImageUrl = "/images/cards/gretzky-rookie.jpg",
                    ListedDate = _seedDate.AddDays(15),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 17,
                    Title = "Lillie Full Art PSA 10",
                    Description = "Pokemon Lillie Full Art PSA 10",
                    Category = "Pokemon",
                    Condition = "Mint",
                    Price = 200.00M,
                    ImageUrl = "/images/cards/lillie.jpg",
                    ListedDate = _seedDate.AddDays(16),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 18,
                    Title = "Kareem Abdul-Jabbar Unparalleled Autograph",
                    Description = "Kareem Abdul-Jabbar Unparalleled Autograph",
                    Category = "Basketball",
                    Condition = "Excellent",
                    Price = 3500.00M,
                    ImageUrl = "/images/cards/jabbar.jpg",
                    ListedDate = _seedDate.AddDays(17),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 19,
                    Title = "Juan Soto Rookie Card",
                    Description = "Topps Chrome Juan Soto Rookie Card PSA 10",
                    Category = "Baseball",
                    Condition = "Good",
                    Price = 400.00M,
                    ImageUrl = "/images/cards/soto2.jpg",
                    ListedDate = _seedDate.AddDays(18),
                    Status = "Available",
                    UserID = regularUser.Id
                },
                new Card
                {
                    CardID = 20,
                    Title = "Stephen Curry 2018-19 Immaculate Moments",
                    Description = "Stephen Curry 2018-19 Immaculate Moments",
                    Category = "Basketball",
                    Condition = "Near Mint",
                    Price = 2600.00M,
                    ImageUrl = "/images/cards/curry.jpg",
                    ListedDate = _seedDate.AddDays(19),
                    Status = "Available",
                    UserID = regularUser.Id
                }
            };

            modelBuilder.Entity<Card>().HasData(cards);
        }
    }
} 