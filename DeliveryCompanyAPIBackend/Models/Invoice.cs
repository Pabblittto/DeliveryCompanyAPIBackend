using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class Invoice// faktura
    {
        [Key]
        public int Id { get; set; }

        public int DepartmentId { get; set; }
        public Department department { get; set; }

        public DataType data { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }

    }
}
