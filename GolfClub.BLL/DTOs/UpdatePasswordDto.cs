namespace GolfClub.BLL.DTOs
{
    public class UpdatePasswordDto
    {
        public required string UserName { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
