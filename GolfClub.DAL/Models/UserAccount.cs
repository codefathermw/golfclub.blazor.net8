namespace GolfClub.DAL.Models
{
    public class UserAccount
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ICollection<Fitting>? FittingRequests { get; set; }
        public virtual ICollection<UserRole>? UserRoles { get; set; }
        public virtual UserProfile? UserProfile { get; set; }
    }
}
