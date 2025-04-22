using System;
using System.ComponentModel.DataAnnotations;

namespace CardTradeHub.Models.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username must be between {2} and {1} characters", MinimumLength = 3)]
        [Display(Name = "Username")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Role")]
        public required string Role { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [StringLength(100, ErrorMessage = "Password must be between {2} and {1} characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match")]
        public string? ConfirmNewPassword { get; set; }

        [Display(Name = "Registration Date")]
        public DateTime CreatedAt { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime LastLoginDate { get; set; }
    }
} 