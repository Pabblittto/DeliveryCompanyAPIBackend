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
        public string Name { get; set; }
        public int BankAccountNo { get; set; }

        public string Street { get; set; }
        public int BuildingNo { get; set; }
        public string OfficeTelNo { get; set; }
        public string ManagerTelNo { get; set; }


        // pools need by convention
        public ICollection<Car> Cars { get; set; }
        public ICollection<Warehous> Warehouses { get; set;} 
        public ICollection<CourierTablet> courierTablets { get; set; }
        public ICollection<Invoice> invoices { get; set; }
        public ICollection<Order> orders { get; set; }
        public ICollection<Region> regions { get; set; }
        public ICollection<Worker> workers { get; set; }
    }
}
