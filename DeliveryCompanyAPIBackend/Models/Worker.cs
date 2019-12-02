using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class Worker
    {
        [Key]
        public int Id { get; set; }

        public int DepartmentId { get; set; }
        public Department department { get; set; }

        public string Name { get; set; }
        public string Surname { get; set;}

        public string PositionId { get; set; }
        public Position position { get; set; }

        public ICollection<Contract> Contracts { get; set; }// need by convention
    }
}
