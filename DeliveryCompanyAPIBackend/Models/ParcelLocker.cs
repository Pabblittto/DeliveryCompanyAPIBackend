using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class ParcelLocker// paczkomat
    {
        [Key]
        public int Id { get; set; }

        public int StreetId { get; set; }
        public Street street { get; set; }

        public ICollection<Chamber> chambers { get; set; }
    }
}
