using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardTradeHub.Models
{
    public class Transaction
    {
        private static readonly DateTime DefaultDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public Transaction()
        {
            Date = DefaultDate;
            Status = "Pending";
            PaymentMethod = string.Empty;
            TransactionReference = string.Empty;
            ShippingAddress = string.Empty;
            TrackingNumber = string.Empty;
            DisputeReason = string.Empty;
            DisputeStatus = "None";
            BuyerID = string.Empty;
            SellerID = string.Empty;
        }

        [Key]
        public int TransactionID { get; set; }

        [ForeignKey("Buyer")]
        public string BuyerID { get; set; }

        [ForeignKey("Seller")]
        public string SellerID { get; set; }

        [ForeignKey("Card")]
        public int CardID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public User? Buyer { get; set; }
        public User? Seller { get; set; }
        public Card? Card { get; set; }

        [Required]
        public required string Status { get; set; } // Pending, Completed, Cancelled, Refunded

        public required string PaymentMethod { get; set; }
        public required string TransactionReference { get; set; }

        public required string ShippingAddress { get; set; }
        public required string TrackingNumber { get; set; }

        // For dispute handling
        public bool HasDispute { get; set; }
        public required string DisputeReason { get; set; }
        public DateTime? DisputeDate { get; set; }
        public required string DisputeStatus { get; set; }
    }
}