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
        public int RegistrationNumber { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int VIN { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public int PolicyNumber { get; set; }
    }
}
