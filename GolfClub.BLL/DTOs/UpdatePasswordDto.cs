namespace GolfClub.BLL.DTOs
{
    public class UpdatePasswordDto
    {
        public required int UserId { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
