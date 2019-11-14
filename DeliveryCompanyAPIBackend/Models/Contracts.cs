using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class Contracts
    {
        [Key]
        public int Id { get; set; }

        public int WorkersId { get; set; }
        public Workers Worker { get; set; }

        public DataType Data { get; set; }
        public string FilePath { get; set; }

    }
}
