using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfClub.DAL.Models
{
    public class UserAccount
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Fitting> FittingRequests { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
