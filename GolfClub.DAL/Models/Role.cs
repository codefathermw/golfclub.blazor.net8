using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfClub.DAL.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } // e.g., "Admin", "UserAccount"
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
