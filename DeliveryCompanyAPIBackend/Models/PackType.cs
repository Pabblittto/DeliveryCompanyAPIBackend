using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class PackType
    {
        [Key]
        public string Name { get; set; }
        public int MinWeight { get; set; }
        public int MaxWeight { get; set; }
        public int Price { get; set; }
        public ICollection<Pack> packs { get; set; }

    }
}
