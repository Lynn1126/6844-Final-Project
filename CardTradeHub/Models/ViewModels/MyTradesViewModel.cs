using System;
using System.Collections.Generic;
using CardTradeHub.Models;

namespace CardTradeHub.Models.ViewModels
{
    public class MyTradesViewModel
    {
        public List<Transaction> Purchases { get; set; } = new List<Transaction>();
        public List<Transaction> Sales { get; set; } = new List<Transaction>();

        public class TransactionViewModel
        {
            public int TransactionID { get; set; }
            public string CardTitle { get; set; }
            public string CardCategory { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
            public string Status { get; set; }
            public string SellerName { get; set; }
            public string TrackingNumber { get; set; }
        }
    }
} 