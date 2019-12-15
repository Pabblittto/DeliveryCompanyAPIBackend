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
        [Required]
        public int DepartmentId { get; set; }
        public Department department { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public string Description { get; set; }
        [Required]
        public string FilePath { get; set; }

    }
}
