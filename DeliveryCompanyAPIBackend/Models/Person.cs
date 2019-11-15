using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int BuildingNo { get; set; }
        public string TelNo { get; set; }

        
        public ICollection<Order> BeeingSender { get; set; }

        public ICollection<Order> BeeingReceiver { get; set; }

    }
}
