using System.ComponentModel.DataAnnotations;

namespace GolfClub.Blazor.App.Domain.Models
{
    public class UpdatePasswordModel
    {
        public string OldPassword { get; set; } = null!;

        [Required]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 12 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])[A-Za-z\d\W_]{8,12}$", ErrorMessage = "Password must contain at least a lowercase letter, an uppercase letter, a number, and a special character.")]
        public string NewPassword { get; set; } = null!;

        [Required]
        [Compare("NewPassword", ErrorMessage = "The passwords do not match.")]
        public string RepeatPassword { get; set; } = null!;
    }
}