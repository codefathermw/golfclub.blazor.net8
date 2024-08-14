using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfClub.BLL.Models
{
    public class ResponseModel<T>
    {
        public bool IsErrorOccured { get; set; }
        public string Message { get; set; } = null!;
        public T Result { get; set; }
    }
}
