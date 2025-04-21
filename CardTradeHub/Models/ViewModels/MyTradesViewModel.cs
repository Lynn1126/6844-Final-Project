using System;
using System.Collections.Generic;
using CardTradeHub.Models;
using System.ComponentModel.DataAnnotations;

namespace CardTradeHub.Models.ViewModels
{
    public class MyTradesViewModel
    {
        public List<Transaction> Purchases { get; set; } = new List<Transaction>();
        public List<Transaction> Sales { get; set; } = new List<Transaction>();
    }

    public class TradeDetailsViewModel
    {
        public int TradeID { get; set; }
        public int CardID { get; set; }
        public decimal Price { get; set; }
        public DateTime TradeDate { get; set; }

        [Required]
        [Display(Name = "Card Title")]
        public required string CardTitle { get; set; }

        [Required]
        [Display(Name = "Category")]
        public required string CardCategory { get; set; }

        [Required]
        [Display(Name = "Status")]
        public required string Status { get; set; }

        [Required]
        [Display(Name = "Seller")]
        public required string SellerName { get; set; }

        [Display(Name = "Tracking Number")]
        public string? TrackingNumber { get; set; }
    }
} 