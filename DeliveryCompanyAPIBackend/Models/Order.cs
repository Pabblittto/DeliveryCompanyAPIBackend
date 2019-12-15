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
        [Required]
        public int DepartmentId { get; set; }
        public Department department { get; set; }
        [Required]
        public int SenderId { get; set; }
        public Person Sender { get; set; }
        [Required]
        public int ReciverId { get; set; }
        public Person Receiver { get; set; }

        public string State { get; set; }//state of the order
        [Required]
        public int PackId { get; set; }
        public Pack pack { get; set; }
    }
}
