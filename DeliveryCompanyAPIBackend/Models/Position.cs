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
        public string Name { get; set; }
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }

        public ICollection<Worker> workers { get; set; }
    }
}
