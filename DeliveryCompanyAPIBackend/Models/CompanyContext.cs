using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryCompanyAPIBackend.Models
{
    public class CompanyContext: IdentityDbContext
    {
        public CompanyContext(DbContextOptions options) : base(options)
        {

        }

        //public CompanyContext()
        //{

        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            // konfigurowanie bazy danych 

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {//konfigurowanie shematu bazy danych
            base.OnModelCreating(builder);
            builder.Entity<Order>()
                .HasOne(obj => obj.Receiver)
                .WithMany(obj => obj.BeeingReceiver)
                .HasForeignKey(obj => obj.ReciverId);

            builder.Entity<Order>()
                .HasOne(obj => obj.Sender)
                .WithMany(obj => obj.BeeingSender)
                .HasForeignKey(obj => obj.SenderId);

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
