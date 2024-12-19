using System.ComponentModel.DataAnnotations;

namespace GolfClub.BLL.Models.Domain
{
    public class LoginFormModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Username")]
        public string UserName { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your Password")]
        public string Password { get; set; } = null!;
    }
}