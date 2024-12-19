namespace GolfClub.BLL.Models.Domain
{
    public class UpdatePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatPassword { get; set; }
    }
}
