using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class Pack
    {
        [Key]
        public int Id { get; set; }

        public string PackTypeId { get; set; }
        public PackType type { get; set; }

        public int Weight { get; set; }
        public int Height { get; set; }
        public Order order { get; set; }
    }
}
