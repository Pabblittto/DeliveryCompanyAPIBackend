using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class Departments
    {

        // pools need by convention
        public ICollection<Cars> Cars { get; set; }
    }
}
