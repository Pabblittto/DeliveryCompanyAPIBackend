using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class Contract
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int WorkerId { get; set; }
        public Worker worker { get; set; }

        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string FilePath { get; set; }

    }
}
