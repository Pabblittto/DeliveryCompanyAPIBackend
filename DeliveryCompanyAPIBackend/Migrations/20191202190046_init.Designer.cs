﻿// <auto-generated />
using System;
using DeliveryCompanyAPIBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeliveryCompanyAPIBackend.Migrations
{
    [DbContext(typeof(CompanyContext))]
    [Migration("20191202190046_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Car", b =>
                {
                    b.Property<int>("RegistrationNumber")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DepartmentId");

                    b.Property<string>("Mark");

                    b.Property<string>("Model");

                    b.Property<int>("PolicyNumber");

                    b.Property<int>("VIN");

                    b.HasKey("RegistrationNumber");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Chamber", b =>
                {
                    b.Property<int>("ChamberTypeID");

                    b.Property<int>("ParcelLockerID");

                    b.Property<int>("Amount");

                    b.Property<int>("FreeAmount");

                    b.Property<string>("chamberTypeTypeName");

                    b.HasKey("ChamberTypeID", "ParcelLockerID");

                    b.HasIndex("ParcelLockerID");

                    b.HasIndex("chamberTypeTypeName");

                    b.ToTable("Chambers");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.ChamberType", b =>
                {
                    b.Property<string>("TypeName")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Height");

                    b.Property<int>("Width");

                    b.HasKey("TypeName");

                    b.ToTable("ChamberTypes");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Data");

                    b.Property<string>("FilePath");

                    b.Property<int>("WorkerId");

                    b.HasKey("Id");

                    b.HasIndex("WorkerId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.CourierTablet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddedDate");

                    b.Property<int>("DepartmentId");

                    b.Property<string>("Model");

                    b.Property<int>("ProdYear");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("CourierTablets");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BankAccountNo");

                    b.Property<int>("BuildingNo");

                    b.Property<string>("ManagerTelNo");

                    b.Property<string>("Name");

                    b.Property<string>("OfficeTelNo");

                    b.Property<string>("Street");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DepartmentId");

                    b.Property<string>("Description");

                    b.Property<string>("FilePath");

                    b.Property<int>("data");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DepartmentId");

                    b.Property<int>("PackId");

                    b.Property<int>("ReciverId");

                    b.Property<int>("SenderId");

                    b.Property<string>("State");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("PackId")
                        .IsUnique();

                    b.HasIndex("ReciverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Pack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Height");

                    b.Property<string>("PackTypeId");

                    b.Property<int>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("PackTypeId");

                    b.ToTable("Packs");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.PackType", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MaxWeight");

                    b.Property<int>("MinWeight");

                    b.Property<int>("Price");

                    b.HasKey("Name");

                    b.ToTable("PackTypes");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.ParcelLocker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("StreetId");

                    b.HasKey("Id");

                    b.HasIndex("StreetId");

                    b.ToTable("ParcelLockers");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BuildingNo");

                    b.Property<string>("City");

                    b.Property<string>("Name");

                    b.Property<string>("Street");

                    b.Property<string>("Surname");

                    b.Property<string>("TelNo");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Position", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MaxSalary");

                    b.Property<int>("MinSalary");

                    b.HasKey("Name");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CityName");

                    b.Property<int>("DepartmentId");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Street", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("RegionId");

                    b.Property<string>("StreetName");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Streets");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Warehous", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DepartmentId");

                    b.Property<int>("HouseNumber");

                    b.Property<string>("Street");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DepartmentId");

                    b.Property<string>("Name");

                    b.Property<string>("PositionId");

                    b.Property<string>("Surname");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("PositionId");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Car", b =>
                {
                    b.HasOne("DeliveryCompanyAPIBackend.Models.Department", "Department")
                        .WithMany("Cars")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Chamber", b =>
                {
                    b.HasOne("DeliveryCompanyAPIBackend.Models.ParcelLocker", "parcelLocker")
                        .WithMany("chambers")
                        .HasForeignKey("ParcelLockerID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DeliveryCompanyAPIBackend.Models.ChamberType", "chamberType")
                        .WithMany("chambers")
                        .HasForeignKey("chamberTypeTypeName");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Contract", b =>
                {
                    b.HasOne("DeliveryCompanyAPIBackend.Models.Worker", "worker")
                        .WithMany("Contracts")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.CourierTablet", b =>
                {
                    b.HasOne("DeliveryCompanyAPIBackend.Models.Department", "department")
                        .WithMany("courierTablets")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Invoice", b =>
                {
                    b.HasOne("DeliveryCompanyAPIBackend.Models.Department", "department")
                        .WithMany("invoices")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Order", b =>
                {
                    b.HasOne("DeliveryCompanyAPIBackend.Models.Department", "department")
                        .WithMany("orders")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DeliveryCompanyAPIBackend.Models.Pack", "pack")
                        .WithOne("order")
                        .HasForeignKey("DeliveryCompanyAPIBackend.Models.Order", "PackId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DeliveryCompanyAPIBackend.Models.Person", "Receiver")
                        .WithMany("BeeingReceiver")
                        .HasForeignKey("ReciverId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DeliveryCompanyAPIBackend.Models.Person", "Sender")
                        .WithMany("BeeingSender")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Pack", b =>
                {
                    b.HasOne("DeliveryCompanyAPIBackend.Models.PackType", "type")
                        .WithMany("packs")
                        .HasForeignKey("PackTypeId");
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.ParcelLocker", b =>
                {
                    b.HasOne("DeliveryCompanyAPIBackend.Models.Street", "street")
                        .WithMany("parcelLockers")
                        .HasForeignKey("StreetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Region", b =>
                {
                    b.HasOne("DeliveryCompanyAPIBackend.Models.Department", "department")
                        .WithMany("regions")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Street", b =>
                {
                    b.HasOne("DeliveryCompanyAPIBackend.Models.Region", "region")
                        .WithMany("streets")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Warehous", b =>
                {
                    b.HasOne("DeliveryCompanyAPIBackend.Models.Department", "department")
                        .WithMany("Warehouses")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeliveryCompanyAPIBackend.Models.Worker", b =>
                {
                    b.HasOne("DeliveryCompanyAPIBackend.Models.Department", "department")
                        .WithMany("workers")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DeliveryCompanyAPIBackend.Models.Position", "position")
                        .WithMany("workers")
                        .HasForeignKey("PositionId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}