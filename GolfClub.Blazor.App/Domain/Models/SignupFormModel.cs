using System.ComponentModel.DataAnnotations;

namespace GolfClub.Blazor.App.Domain.Models
{
    public class SignupFormModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your first name.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your last name.")]
        public string LastName { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a username.")]
        public string UserName { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a password.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty;
    }
}
