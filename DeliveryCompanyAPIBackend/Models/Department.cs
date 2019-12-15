using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class Department
    { 
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int BankAccountNo { get; set; }

        [Required]
        public string Street { get; set; }
        [Required]
        public int BuildingNo { get; set; }
        [Required]
        public string OfficeTelNo { get; set; }
        [Required]
        public string ManagerTelNo { get; set; }


        // pools need by convention
        public ICollection<Car> Cars { get; set; }
        public ICollection<Warehous> Warehouses { get; set;} 
        public ICollection<CourierTablet> courierTablets { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Region> Regions { get; set; }
        public ICollection<Worker> Workers { get; set; }
    }
}
