using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfClub.BLL.DTOs
{
    public class SaveFittingRequestDto
    {
        public DateTime ScheduledDate { get; set; }
        public string? Comments { get; set; }
        public string FittingType { get; set; }
        public int UserId { get; set; }
    }
}
