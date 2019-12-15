using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class ChamberType
    {
        [Key]
        public string TypeName { get; set; }
        [Required]
        public int Height { get; set; }
        [Required]

        public int Width { get; set; }

        public ICollection<Chamber> chambers { get; set; }
    }
}
