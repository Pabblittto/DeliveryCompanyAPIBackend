using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class Chamber
    {
        [Key]
        public int ParcelLockerID { get; set; }
        public ParcelLocker parcelLocker { get; set; }

        [Key]
        public int ChamberTypeID { get; set; }
        public ChamberType chamberType { get; set; }

        public int Amount { get; set; }
        public int FreeAmount { get; set; }

    }
}
