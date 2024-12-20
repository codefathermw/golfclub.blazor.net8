using System.ComponentModel.DataAnnotations;

namespace GolfClub.Blazor.App.Domain.Models
{
    public class UpdateProfileModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must only contain alphabetic characters.")]
        public string FirstName { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must only contain alphabetic characters.")]
        public string LastName { get; set; } = null!;

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date of birth.")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Address must be at least 10 characters long.")]
        public string? Address { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Golf Club Size must be a valid number.")]
        public string GolfClubSize { get; set; } = null!;
    }
}
