using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfClub.BLL.Models
{
    public class FittingTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }
        public string Notes { get; set; }
        public int FittingRequestId { get; set; }
    }
}
