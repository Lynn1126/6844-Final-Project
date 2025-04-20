using System.ComponentModel.DataAnnotations;

namespace CardTradeHub.Models.ViewModels
{
    public class MyCardsViewModel
    {
        public List<Card> UserCards { get; set; }
    }

    public class CreateCardViewModel
    {
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
        [Range(0.01, 1000000, ErrorMessage = "Price must be between $0.01 and $1,000,000")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
    }
} 