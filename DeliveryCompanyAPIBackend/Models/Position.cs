using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class Position
    {
        [Key]
        [Required]
        public string Name { get; set; }
        [Required]
        public int MinSalary { get; set; }
        [Required]
        public int MaxSalary { get; set; }

        public ICollection<Worker> workers { get; set; }
    }
}
