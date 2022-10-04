﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TestHeroku
{
    public partial class postgresContext : DbContext
    {
        public postgresContext()
        {
        }

        public postgresContext(DbContextOptions<postgresContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Orderproducts> Orderproducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");

            modelBuilder.Entity<Orderproducts>(entity =>
            {
                entity.HasKey(e => e.Orderproductid)
                    .HasName("orderproducts_pkey");

                entity.ToTable("orderproducts");

                entity.Property(e => e.Orderproductid)
                    .HasColumnName("orderproductid")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Activeingredients)
                    .HasMaxLength(100)
                    .HasColumnName("activeingredients");

                entity.Property(e => e.Category)
                    .HasMaxLength(100)
                    .HasColumnName("category");

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

                entity.Property(e => e.Modifiedby)
                    .HasMaxLength(50)
                    .HasColumnName("modifiedby");

                entity.Property(e => e.Modifieddate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modifieddate");

                entity.Property(e => e.Nameonpackage)
                    .HasMaxLength(100)
                    .HasColumnName("nameonpackage");

                entity.Property(e => e.Ordersid).HasColumnName("ordersid");

                entity.Property(e => e.Priceperpackclientpays)
                    .HasMaxLength(100)
                    .HasColumnName("priceperpackclientpays");

                entity.Property(e => e.Productid).HasColumnName("productid");

                entity.Property(e => e.Productsourcedfrom)
                    .HasMaxLength(100)
                    .HasColumnName("productsourcedfrom");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Shippingcostperorder)
                    .HasMaxLength(100)
                    .HasColumnName("shippingcostperorder");

                entity.Property(e => e.Strength)
                    .HasMaxLength(100)
                    .HasColumnName("strength");

                entity.Property(e => e.Totalpacksordered)
                    .HasMaxLength(100)
                    .HasColumnName("totalpacksordered");

                entity.Property(e => e.Totalpriceclientpays)
                    .HasMaxLength(100)
                    .HasColumnName("totalpriceclientpays");

                entity.Property(e => e.Totalpricecustomerpays)
                    .HasMaxLength(100)
                    .HasColumnName("totalpricecustomerpays");

                entity.Property(e => e.Unitsperpack)
                    .HasMaxLength(100)
                    .HasColumnName("unitsperpack");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}