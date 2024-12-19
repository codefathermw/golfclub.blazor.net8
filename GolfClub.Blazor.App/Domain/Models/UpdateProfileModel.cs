namespace GolfClub.BLL.Models.Domain
{
    public class UpdateProfileModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string DateOfBirth { get; set; }
        public required string Address { get; set; }
        public required string GolfClubSize { get; set; }
    }
}
