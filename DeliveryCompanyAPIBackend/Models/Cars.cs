using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class Cars
    {
        [Key]
        public int RegistrationNumber { get; set; }

        public int DepartmentsId { get; set; }
        public Departments Department { get; set; }

        public int VIN { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public int PolicyNumber { get; set; }
    }
}
