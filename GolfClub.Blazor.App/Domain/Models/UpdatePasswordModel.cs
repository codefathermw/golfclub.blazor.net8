using System.ComponentModel.DataAnnotations;

namespace GolfClub.BLL.Models.Domain
{
    public class UpdatePasswordModel
    {
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 12 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,12}$", ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one number, and one symbol.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword", ErrorMessage = "The passwords do not match.")]
        public string RepeatPassword { get; set; } = string.Empty;
    }
}