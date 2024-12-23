namespace GolfClub.DAL.Models
{
    public class UserProfile
    {
        public int UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public string? GolfClubSize { get; set; }
        public DateTime DateUpdated { get; set; }
        public virtual UserAccount? User { get; set; }
    }
}
