using System.ComponentModel.DataAnnotations;

namespace GolfClub.Blazor.App.Domain.Models
{
    public class UpdateContactModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Please enter a valid phone number (e.g., +1234567890).")]
        public string PhoneNumber { get; set; } = null!;
    }
}
