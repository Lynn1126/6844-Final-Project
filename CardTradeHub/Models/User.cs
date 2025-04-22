using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CardTradeHub.Models
{
    public class User : IdentityUser
    {
        private static readonly DateTime DefaultDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public User()
        {
            Role = "User";
            IsActive = true;
            CreatedAt = DefaultDate;
            RegisterDate = DefaultDate;
            LastLoginDate = DefaultDate;
            Cards = new List<Card>();
            Transactions = new List<Transaction>();
        }

        [Required]
        [StringLength(20)]
        public required string Role { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        // Navigation properties
        public virtual ICollection<Card> Cards { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}