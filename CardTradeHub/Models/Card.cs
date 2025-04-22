using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardTradeHub.Models
{
    public class Card
    {
        private static readonly DateTime DefaultDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public Card()
        {
            Title = string.Empty;
            Description = string.Empty;
            Category = string.Empty;
            Condition = string.Empty;
            Status = "Available";
            ListedDate = DefaultDate;
            Transactions = new List<Transaction>();
            UserID = string.Empty;
        }

        [Key]
        public int CardID { get; set; }

        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required string Category { get; set; }

        [Required]
        public required string Condition { get; set; }

        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Price must be between 0.01 and 1,000,000")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        
        public DateTime ListedDate { get; set; }
        
        [Required]
        public required string Status { get; set; } // Available, Sold, Pending
        
        // Foreign keys
        [Required]
        [ForeignKey("User")]
        public string UserID { get; set; }

        public User? User { get; set; }

        // Navigation properties
        public ICollection<Transaction> Transactions { get; set; }
    }
}