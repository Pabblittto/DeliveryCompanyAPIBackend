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
        [Required]
        public string Name { get; set; }
        [Required]
        public int MinWeight { get; set; }
        [Required]
        public int MaxWeight { get; set; }
        [Required]
        public int Price { get; set; }
        public ICollection<Pack> packs { get; set; }

    }
}
