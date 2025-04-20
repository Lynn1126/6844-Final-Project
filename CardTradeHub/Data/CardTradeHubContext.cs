using Microsoft.EntityFrameworkCore;
using CardTradeHub.Models;

namespace CardTradeHub.Data
{
    public class CardTradeHubContext : DbContext
    {
        private static readonly DateTime _seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public CardTradeHubContext(DbContextOptions<CardTradeHubContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
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
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    Username = "admin",
                    Email = "admin@cardtradehub.com",
                    PasswordHash = "AQAAAAEAACcQAAAAEHYxOE9wZQ==", // This should be properly hashed
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = _seedDate,
                    RegisterDate = _seedDate,
                    LastLoginDate = _seedDate,
                    IsEmailVerified = false
                }
            );

            // Seed data for cards - Gaming Cards
            var cards = new List<Card>
            {
                new Card
                {
                    CardID = 1,
                    Title = "Charizard First Edition",
                    Description = "First edition Charizard in mint condition",
                    Category = "Pokemon",
                    Condition = "Mint",
                    Price = 1000.00M,
                    ImageUrl = "/images/cards/charizard.jpg",
                    ListedDate = _seedDate,
                    Status = "Available",
                    UserID = 1
                },
                new Card
                {
                    CardID = 2,
                    Title = "Black Lotus MTG",
                    Description = "Rare Black Lotus card from Magic: The Gathering",
                    Category = "Magic: The Gathering",
                    Condition = "Good",
                    Price = 5000.00M,
                    ImageUrl = "/images/cards/black-lotus.jpg",
                    ListedDate = _seedDate.AddDays(1),
                    Status = "Available",
                    UserID = 1
                },
                new Card
                {
                    CardID = 3,
                    Title = "Blue-Eyes White Dragon",
                    Description = "Classic Yu-Gi-Oh! card in excellent condition",
                    Category = "Yu-Gi-Oh",
                    Condition = "Excellent",
                    Price = 500.00M,
                    ImageUrl = "/images/cards/blue-eyes.jpg",
                    ListedDate = _seedDate.AddDays(2),
                    Status = "Available",
                    UserID = 1
                },
                new Card
                {
                    CardID = 4,
                    Title = "Pikachu Illustrator",
                    Description = "Extremely rare Pikachu Illustrator promo card",
                    Category = "Pokemon",
                    Condition = "Near Mint",
                    Price = 10000.00M,
                    ImageUrl = "/images/cards/pikachu-illustrator.jpg",
                    ListedDate = _seedDate.AddDays(3),
                    Status = "Available",
                    UserID = 1
                },
                new Card
                {
                    CardID = 5,
                    Title = "Time Walk",
                    Description = "Vintage Time Walk card from MTG Alpha set",
                    Category = "Magic: The Gathering",
                    Condition = "Good",
                    Price = 3000.00M,
                    ImageUrl = "/images/cards/time-walk.jpg",
                    ListedDate = _seedDate.AddDays(4),
                    Status = "Available",
                    UserID = 1
                },
                // Sports Cards
                new Card
                {
                    CardID = 6,
                    Title = "Michael Jordan Rookie Card",
                    Description = "1986 Fleer Michael Jordan Rookie Card",
                    Category = "Basketball",
                    Condition = "Near Mint",
                    Price = 7500.00M,
                    ImageUrl = "/images/cards/jordan-rookie.jpg",
                    ListedDate = _seedDate.AddDays(5),
                    Status = "Available",
                    UserID = 1
                },
                new Card
                {
                    CardID = 7,
                    Title = "Mickey Mantle 1952 Topps",
                    Description = "1952 Topps Mickey Mantle #311",
                    Category = "Baseball",
                    Condition = "Good",
                    Price = 15000.00M,
                    ImageUrl = "/images/cards/mantle-1952.jpg",
                    ListedDate = _seedDate.AddDays(6),
                    Status = "Available",
                    UserID = 1
                },
                new Card
                {
                    CardID = 8,
                    Title = "Tom Brady Rookie Card",
                    Description = "2000 Playoff Contenders Tom Brady Rookie Card",
                    Category = "Football",
                    Condition = "Excellent",
                    Price = 8000.00M,
                    ImageUrl = "/images/cards/brady-rookie.jpg",
                    ListedDate = _seedDate.AddDays(7),
                    Status = "Available",
                    UserID = 1
                },
                new Card
                {
                    CardID = 9,
                    Title = "Exodia the Forbidden One",
                    Description = "Complete set of all 5 Exodia pieces",
                    Category = "Yu-Gi-Oh",
                    Condition = "Near Mint",
                    Price = 800.00M,
                    ImageUrl = "/images/cards/exodia.jpg",
                    ListedDate = _seedDate.AddDays(8),
                    Status = "Available",
                    UserID = 1
                },
                new Card
                {
                    CardID = 10,
                    Title = "LeBron James Rookie Card",
                    Description = "2003-04 Upper Deck LeBron James Rookie Card",
                    Category = "Basketball",
                    Condition = "Mint",
                    Price = 6000.00M,
                    ImageUrl = "/images/cards/lebron-rookie.jpg",
                    ListedDate = _seedDate.AddDays(9),
                    Status = "Available",
                    UserID = 1
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
                    UserID = 1
                },
                new Card
                {
                    CardID = 12,
                    Title = "Babe Ruth 1933 Goudey",
                    Description = "1933 Goudey Babe Ruth Baseball Card",
                    Category = "Baseball",
                    Condition = "Good",
                    Price = 12000.00M,
                    ImageUrl = "/images/cards/ruth-1933.jpg",
                    ListedDate = _seedDate.AddDays(11),
                    Status = "Available",
                    UserID = 1
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
                    UserID = 1
                },
                new Card
                {
                    CardID = 14,
                    Title = "Kobe Bryant Rookie Card",
                    Description = "1996-97 Topps Chrome Kobe Bryant Rookie Card",
                    Category = "Basketball",
                    Condition = "Near Mint",
                    Price = 5500.00M,
                    ImageUrl = "/images/cards/kobe-rookie.jpg",
                    ListedDate = _seedDate.AddDays(13),
                    Status = "Available",
                    UserID = 1
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
                    UserID = 1
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
                    UserID = 1
                },
                new Card
                {
                    CardID = 17,
                    Title = "Ancient Mew Promo",
                    Description = "Pokemon Movie Ancient Mew Promo Card",
                    Category = "Pokemon",
                    Condition = "Near Mint",
                    Price = 200.00M,
                    ImageUrl = "/images/cards/ancient-mew.jpg",
                    ListedDate = _seedDate.AddDays(16),
                    Status = "Available",
                    UserID = 1
                },
                new Card
                {
                    CardID = 18,
                    Title = "Jerry Rice Rookie Card",
                    Description = "1986 Topps Jerry Rice Rookie Card",
                    Category = "Football",
                    Condition = "Excellent",
                    Price = 3500.00M,
                    ImageUrl = "/images/cards/rice-rookie.jpg",
                    ListedDate = _seedDate.AddDays(17),
                    Status = "Available",
                    UserID = 1
                },
                new Card
                {
                    CardID = 19,
                    Title = "Ancestral Recall",
                    Description = "Alpha Edition Ancestral Recall",
                    Category = "Magic: The Gathering",
                    Condition = "Good",
                    Price = 4500.00M,
                    ImageUrl = "/images/cards/ancestral-recall.jpg",
                    ListedDate = _seedDate.AddDays(18),
                    Status = "Available",
                    UserID = 1
                },
                new Card
                {
                    CardID = 20,
                    Title = "Red Eyes Black Dragon",
                    Description = "First Edition Red Eyes Black Dragon",
                    Category = "Yu-Gi-Oh",
                    Condition = "Near Mint",
                    Price = 600.00M,
                    ImageUrl = "/images/cards/red-eyes.jpg",
                    ListedDate = _seedDate.AddDays(19),
                    Status = "Available",
                    UserID = 1
                }
            };

            modelBuilder.Entity<Card>().HasData(cards);

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
        }
    }
} 