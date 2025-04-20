using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardTradeHub.Models
{
    public class User
    {
        private static readonly DateTime DefaultDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public User()
        {
            Username = string.Empty;
            Email = string.Empty;
            PasswordHash = string.Empty;
            Role = "User";
            CreatedAt = DefaultDate;
            RegisterDate = DefaultDate;
            LastLoginDate = DefaultDate;
            Cards = new List<Card>();
            Transactions = new List<Transaction>();
        }

        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }

        public DateTime RegisterDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsEmailVerified { get; set; }

        // Navigation properties
        public ICollection<Card> Cards { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}