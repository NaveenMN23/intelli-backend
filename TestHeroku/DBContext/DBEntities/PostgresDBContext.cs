﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IntelliCRMAPIService.DBContext
{
    public partial class PostgresDBContext : DbContext
    {
        public PostgresDBContext()
        {
        }

        public PostgresDBContext(DbContextOptions<PostgresDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Userdetails> Userdetails { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Customerproduct> Customerproduct { get; set; }
        public virtual DbSet<Productmaster> Productmaster { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrdersProducts> OrdersProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");

            modelBuilder.Entity<Customerproduct>(entity =>
            {
                entity.ToTable("customerproduct");

                entity.Property(e => e.Customerproductid)
                    .HasColumnName("customerproductid")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Createdby)
                    .HasMaxLength(50)
                    .HasColumnName("createdby");

                entity.Property(e => e.Createddate).HasColumnName("createddate");

                entity.Property(e => e.Modifiedby)
                    .HasMaxLength(50)
                    .HasColumnName("modifiedby");

                entity.Property(e => e.Modifieddate).HasColumnName("modifieddate");

                entity.Property(e => e.Productid)
                    .HasMaxLength(10)
                    .HasColumnName("productid");

                entity.Property(e => e.Productname)
                    .HasMaxLength(50)
                    .HasColumnName("productname");

                entity.Property(e => e.Productprice)
                    .HasMaxLength(50)
                    .HasColumnName("productprice");

                entity.Property(e => e.Qtyassign)
                    .HasMaxLength(50)
                    .HasColumnName("qtyassign");

                entity.Property(e => e.Useridfk).HasColumnName("useridfk");
                entity.Property(e => e.Email).HasColumnName("email");


                entity.HasOne(d => d.UseridfkNavigation)
                    .WithMany(p => p.Customerproduct)
                    .HasForeignKey(d => d.Useridfk)
                    .HasConstraintName("customerproduct_useridfk_fkey");
            });

            modelBuilder.Entity<Userdetails>(entity =>
            {
                entity.ToTable("userdetails");

                entity.Property(e => e.Userdetailsid)
                    .HasColumnName("userdetailsid")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .HasColumnName("address");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .HasColumnName("city");

                entity.Property(e => e.Coutry)
                    .HasMaxLength(100)
                    .HasColumnName("coutry");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(50)
                    .HasColumnName("createdby");

                entity.Property(e => e.Createddate).HasColumnName("createddate");

                entity.Property(e => e.Creditlimit).HasColumnName("creditlimit");

                entity.Property(e => e.Modifiedby)
                    .HasMaxLength(50)
                    .HasColumnName("modifiedby");

                entity.Property(e => e.Modifieddate).HasColumnName("modifieddate");

                entity.Property(e => e.Soareceviedamount).HasColumnName("soareceviedamount");

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .HasColumnName("state");

                entity.Property(e => e.UseridFk).HasColumnName("userid_fk");

                entity.HasOne(d => d.UseridFkNavigation)
                    .WithMany(p => p.Userdetails)
                    .HasForeignKey(d => d.UseridFk)
                    .HasConstraintName("userdetails_userid_fk_fkey");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Userid)
                    .HasName("users_pkey");

                entity.ToTable("users");

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Priority)
                    .HasColumnName("priority");

                entity.Property(e => e.Accountstatus).HasColumnName("accountstatus");

                entity.Property(e => e.Accounttype).HasColumnName("accounttype");

                entity.Property(e => e.Contactnumber)
                    .HasMaxLength(20)
                    .HasColumnName("contactnumber");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(50)
                    .HasColumnName("createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createddate");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .HasColumnName("lastname");

                entity.Property(e => e.Modifiedby)
                    .HasMaxLength(50)
                    .HasColumnName("modifiedby");

                entity.Property(e => e.Modifieddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modifieddate");

                entity.Property(e => e.Password)
                    .HasMaxLength(500)
                    .HasColumnName("password");

                entity.Property(e => e.Rightsforcustomeraccount).HasColumnName("rightsforcustomeraccount");
                entity.Property(e => e.RightsForOrder).HasColumnName("rightsfororder");

                entity.Property(e => e.RightsForProduct).HasColumnName("rightsforproduct");


                entity.Property(e => e.Rolename)
                    .HasMaxLength(100)
                    .HasColumnName("rolename");

                entity.Property(e => e.Salt)
                    .HasMaxLength(100)
                    .HasColumnName("salt");
            });

            modelBuilder.Entity<Productmaster>(entity =>
            {
                entity.HasKey(e => e.Productid)
                    .HasName("productmaster_pkey");

                entity.ToTable("productmaster");

                entity.Property(e => e.Productid)
                    .HasMaxLength(10)
                    .HasColumnName("productid");

                entity.Property(e => e.Activeingredient)
                    .HasMaxLength(100)
                    .HasColumnName("activeingredient");

                entity.Property(e => e.Batch)
                    .HasMaxLength(100)
                    .HasColumnName("batch");

                entity.Property(e => e.Boe)
                    .HasMaxLength(100)
                    .HasColumnName("boe");

                entity.Property(e => e.Category)
                    .HasMaxLength(100)
                    .HasColumnName("category");

                entity.Property(e => e.Cifpriceperpack).HasColumnName("cifpriceperpack");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(50)
                    .HasColumnName("createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createddate");

                entity.Property(e => e.Dosageform)
                    .HasMaxLength(100)
                    .HasColumnName("dosageform");

                entity.Property(e => e.Equsbrandname)
                    .HasMaxLength(100)
                    .HasColumnName("equsbrandname");

                entity.Property(e => e.Expirydaterange)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("expirydaterange");

                entity.Property(e => e.Licenceholder)
                    .HasMaxLength(100)
                    .HasColumnName("licenceholder");

                entity.Property(e => e.Manufacturer)
                    .HasMaxLength(100)
                    .HasColumnName("manufacturer");

                entity.Property(e => e.Modifiedby)
                    .HasMaxLength(50)
                    .HasColumnName("modifiedby");

                entity.Property(e => e.Modifieddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modifieddate");

                entity.Property(e => e.Nameonpackage)
                    .HasMaxLength(100)
                    .HasColumnName("nameonpackage");

                entity.Property(e => e.Productsourcedfrom)
                    .HasMaxLength(100)
                    .HasColumnName("productsourcedfrom");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.Rxwarningcautionarynote)
                    .HasMaxLength(100)
                    .HasColumnName("rxwarningcautionarynote");

                entity.Property(e => e.Sellingpriceperpack).HasColumnName("sellingpriceperpack");

                entity.Property(e => e.Strength)
                    .HasMaxLength(100)
                    .HasColumnName("strength");

                entity.Property(e => e.Unitsperpack).HasColumnName("unitsperpack");

                entity.Property(e => e.Weight).HasColumnName("weight");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("orders");

                entity.Property(e => e.Ordersid)
                    .HasColumnName("ordersid")
                    .UseIdentityAlwaysColumn();


                entity.Property(e => e.TrackingNo)
                   .HasMaxLength(10)
                   .HasColumnName("trackingno");

                entity.Property(e => e.Address1)
                    .HasMaxLength(100)
                    .HasColumnName("address1");

                entity.Property(e => e.Address2)
                    .HasMaxLength(100)
                    .HasColumnName("address2");

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .HasColumnName("city");

                entity.Property(e => e.Createdby)
                    .HasMaxLength(50)
                    .HasColumnName("createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createddate");

                entity.Property(e => e.Customername)
                    .HasMaxLength(100)
                    .HasColumnName("customername");

                entity.Property(e => e.Customerphonenumber)
                    .HasMaxLength(100)
                    .HasColumnName("customerphonenumber");

                entity.Property(e => e.Status)
                    .HasMaxLength(100)
                    .HasColumnName("status");

                entity.Property(e => e.Date)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date");

                entity.Property(e => e.Directionsofuse)
                    .HasMaxLength(100)
                    .HasColumnName("directionsofuse");

                entity.Property(e => e.DoctorName)
                    .HasMaxLength(100)
                    .HasColumnName("doctorname");


                entity.Property(e => e.Emailaddress)
                    .HasMaxLength(100)
                    .HasColumnName("emailaddress");


                entity.Property(e => e.Modifiedby)
                    .HasMaxLength(50)
                    .HasColumnName("modifiedby");

                entity.Property(e => e.Modifieddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modifieddate");


                entity.Property(e => e.Onlinepharmacy)
                    .HasMaxLength(100)
                    .HasColumnName("onlinepharmacy");

                entity.Property(e => e.OnlinepharmacyName)
                    .HasMaxLength(100)
                    .HasColumnName("onlinepharmacyname");

                entity.Property(e => e.Onlinepharmacyphonenumber)
                    .HasMaxLength(100)
                    .HasColumnName("onlinepharmacyphonenumber");

                entity.Property(e => e.Ordernumber)
                    .HasMaxLength(100)
                    .HasColumnName("ordernumber");

                entity.Property(e => e.Prescribername)
                    .HasMaxLength(100)
                    .HasColumnName("prescribername");

                entity.Property(e => e.Prescriptionattached)
                    .HasMaxLength(100)
                    .HasColumnName("prescriptionattached");


                entity.Property(e => e.Province)
                    .HasMaxLength(100)
                    .HasColumnName("province");



                entity.Property(e => e.Referencenumber)
                    .HasMaxLength(100)
                    .HasColumnName("referencenumber");

                entity.Property(e => e.Refill)
                    .HasMaxLength(100)
                    .HasColumnName("refill");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(100)
                    .HasColumnName("remarks");

                entity.Property(e => e.Rxwarningcautionarynote)
                    .HasMaxLength(100)
                    .HasColumnName("rxwarningcautionarynote");


                entity.Property(e => e.Shippingcostperorder)
                        .HasMaxLength(100)
                        .HasColumnName("shippingcostperorder");

                entity.Property(e => e.Totalpriceclientpays)
                        .HasMaxLength(100)
                        .HasColumnName("totalpriceclientpays");


                entity.Property(e => e.Zipcode)
                    .HasMaxLength(100)
                    .HasColumnName("zipcode");
            });

            modelBuilder.Entity<OrdersProducts>(entity =>
            {
                entity.HasKey(e => e.OrderProductId)
                    .HasName("orderproducts_pkey");

                entity.ToTable("orderproducts");

                entity.Property(e => e.OrderProductId)
                    .HasColumnName("orderproductid")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Category)
                .HasMaxLength(100)
                .HasColumnName("category");

                entity.Property(e => e.Dosageform)
                        .HasMaxLength(100)
                        .HasColumnName("dosageform");

                entity.Property(e => e.OrdersID)
                        .HasColumnName("ordersid");

                entity.Property(e => e.Equsbrandname)
                        .HasMaxLength(100)
                        .HasColumnName("equsbrandname");

                entity.Property(e => e.Activeingredients)
                        .HasMaxLength(100)
                        .HasColumnName("activeingredients");

                entity.Property(e => e.Totalpricecustomerpays)
                        .HasMaxLength(100)
                        .HasColumnName("totalpricecustomerpays");

                entity.Property(e => e.Unitsperpack)
                        .HasMaxLength(100)
                        .HasColumnName("unitsperpack");

                entity.Property(e => e.Strength)
                        .HasMaxLength(100)
                        .HasColumnName("strength");

                entity.Property(e => e.Totalpacksordered)
                        .HasMaxLength(100)
                        .HasColumnName("totalpacksordered");
                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Priceperpackclientpays)
                        .HasMaxLength(100)
                        .HasColumnName("priceperpackclientpays");

                entity.Property(e => e.Productid).HasColumnName("productid");

                entity.Property(e => e.Productsourcedfrom)
                        .HasMaxLength(100)
                        .HasColumnName("productsourcedfrom");

                entity.Property(e => e.Nameonpackage)
                        .HasMaxLength(100)
                        .HasColumnName("nameonpackage");

                entity.Property(e => e.Createdby)
                        .HasMaxLength(50)
                        .HasColumnName("createdby");

                entity.Property(e => e.Createddate)
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("createddate");

                entity.Property(e => e.Modifiedby)
                        .HasMaxLength(50)
                        .HasColumnName("modifiedby");

                entity.Property(e => e.Modifieddate)
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("modifieddate");

            }
        );

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                 .SelectMany(t => t.GetProperties())
                 .Where
                 (p
                  => p.ClrType == typeof(DateTime)
                     || p.ClrType == typeof(DateTime?)
                 ))
            {
                property.SetColumnType("timestamp without time zone");
            }

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}