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
        [Required]
        public int ParcelLockerID { get; set; }
        public ParcelLocker parcelLocker { get; set; }

        [Key]
        [Required]
        public int ChamberTypeID { get; set; }
        public ChamberType chamberType { get; set; }

        [Required]
        public int Amount { get; set; }
        [Required]
        public int FreeAmount { get; set; }

    }
}
