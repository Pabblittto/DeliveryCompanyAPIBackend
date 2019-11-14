using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class Warehous
    {

        [Key]
        public int Id { get; set; }

        public int DepartmentId { get; set; }
        public Department department { get; set; }

        public string Street { get; set; }
        public int HouseNumber { get; set; }
    }
}
