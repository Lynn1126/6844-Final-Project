using System.ComponentModel.DataAnnotations;

namespace CardTradeHub.Models.ViewModels
{
    public class ProfileViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Current Password")]
        public string? CurrentPassword { get; set; }

        [StringLength(100, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "New password and confirm password do not match.")]
        public string? ConfirmNewPassword { get; set; }

        [Display(Name = "Registration Date")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "User Role")]
        public string Role { get; set; }
    }
} 