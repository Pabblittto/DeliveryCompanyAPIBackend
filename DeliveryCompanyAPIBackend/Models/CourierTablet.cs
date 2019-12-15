using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class CourierTablet   // to do wywaleniAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA w choloere
    {
        [Key]
        public int Id { get; set; }

        public int DepartmentId { get; set; }
        public Department department { get; set; }

        public DataType AddedDate { get; set; }
        public string Model { get; set; }
        public DataType ProdYear { get; set; }

    }
}
