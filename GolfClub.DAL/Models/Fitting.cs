using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfClub.DAL.Models
{
    public class Fitting
    {
        public int FittingId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string? Comments { get; set; }
        public string ReferenceNumber { get; set; }
        public string Status { get; set; }
        public string FittingType { get; set; }
        public int UserId { get; set; }
        public UserAccount User { get; set; }
    }
}
