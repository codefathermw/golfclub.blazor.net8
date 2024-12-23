namespace GolfClub.BLL.DTOs
{
    public class UserProfileDto
    {
        public required int UserId { get; set; }
        public required string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public string? GolfClubSize { get; set; }
    }
}
