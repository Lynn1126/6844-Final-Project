using System.Collections.Generic;
using CardTradeHub.Models;

namespace CardTradeHub.Models.ViewModels
{
    public class CardListViewModel
    {
        public IEnumerable<Card> Cards { get; set; } = new List<Card>();
        public PaginationInfo PaginationInfo { get; set; } = new();
        public string? CurrentCategory { get; set; }
        public string? CurrentCondition { get; set; }
        public string? CurrentSort { get; set; }
        public string? CurrentSearch { get; set; }
        public List<string> Categories { get; set; } = new();
        public List<string> Conditions { get; set; } = new();
    }

    public class PaginationInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
} 