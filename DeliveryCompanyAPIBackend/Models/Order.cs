using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class Order// zamówienia
    {
        [Key]
        public int Id { get; set; }

        public int DepartmentId { get; set; }
        public Department department { get; set; }

        public int SenderId { get; set; }
        public Person Sender { get; set; }

        public int ReciverId { get; set; }
        public Person Receiver { get; set; }

        public string State { get; set; }//state of the order

        public int PackId { get; set; }
        public Pack pack { get; set; }
    }
}
