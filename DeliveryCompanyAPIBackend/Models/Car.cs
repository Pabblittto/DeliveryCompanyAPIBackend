using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class Car
    {
        [Key]
        [Required]
        public int RegistrationNumber { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [Required]
        public int VIN { get; set; }
        [Required]
        public string Mark { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int PolicyNumber { get; set; }
    }
}
