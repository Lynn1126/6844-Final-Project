using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CardTradeHub.Models;

namespace CardTradeHub.Models.ViewModels
{
    public class MyCardsViewModel
    {
        public List<Card> Cards { get; set; } = new List<Card>();
        public string UserName { get; set; }
        public int TotalCards { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class CardViewModel
    {
        public int CardID { get; set; }

        [Required]
        [Display(Name = "Title")]
        public required string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public required string Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public required string Category { get; set; }

        [Required]
        [Display(Name = "Condition")]
        public required string Condition { get; set; }

        [Required]
        [Display(Name = "Status")]
        public required string Status { get; set; }

        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive => Status != "Deleted";
    }

    public class CreateCardViewModel
    {
        public int CardID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Condition is required")]
        public string Condition { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = "Available";
    }
} 