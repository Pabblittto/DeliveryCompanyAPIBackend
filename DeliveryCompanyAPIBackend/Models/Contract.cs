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

        public int WorkerId { get; set; }
        public Worker worker { get; set; }

        public DataType Data { get; set; }
        public string FilePath { get; set; }

    }
}
