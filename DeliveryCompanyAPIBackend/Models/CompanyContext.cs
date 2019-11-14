using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class CompanyContext:DbContext
    {
        public CompanyContext(DbContextOptions options) : base(options)
        {

        }

        public CompanyContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // tu wszelkie ustawienie bazy danych trzeba wykonać
        }



        public DbSet<Department> Departments { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<CourierTablet> CourierTablets { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Pack> Packs { get; set; }
        public DbSet<PackType> PackTypes { get; set; }
        public DbSet<ParcelLocker> ParcelLockers { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<Warehous> Warehouses { get; set; }
        public DbSet<Worker> Workers { get; set; }

    }
}
