﻿// <auto-generated />
using System;
using API_and_DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API_and_DataBase.Migrations
{
    [DbContext(typeof(CompanyContext))]
    [Migration("20220727200127_IntialCreate")]
    partial class IntialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("API_and_DataBase.Models.Car", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("API_and_DataBase.Models.CarProduct", b =>
                {
                    b.Property<int>("CarID")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("CarID", "ProductID");

                    b.HasIndex("ProductID");

                    b.ToTable("CarProducts");
                });

            modelBuilder.Entity("API_and_DataBase.Models.Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("API_and_DataBase.Models.Expenses", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("API_and_DataBase.Models.ExportProduct", b =>
                {
                    b.Property<int>("ReceiptID")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int?>("ExportRecieptID")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ReceiptID", "ProductID");

                    b.HasIndex("ExportRecieptID");

                    b.HasIndex("ProductID");

                    b.ToTable("ExportProducts");
                });

            modelBuilder.Entity("API_and_DataBase.Models.ExportReciept", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int?>("CarBuyID")
                        .HasColumnType("int");

                    b.Property<int?>("CarSellID")
                        .HasColumnType("int");

                    b.Property<int?>("CustomerID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Paid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Remaining")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("CarBuyID");

                    b.HasIndex("CarSellID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("UserName");

                    b.ToTable("ExportReciepts");
                });

            modelBuilder.Entity("API_and_DataBase.Models.ImportProduct", b =>
                {
                    b.Property<int>("ReceiptID")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int?>("ImportRecieptID")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ReceiptID", "ProductID");

                    b.HasIndex("ImportRecieptID");

                    b.HasIndex("ProductID");

                    b.ToTable("ImportProducts");
                });

            modelBuilder.Entity("API_and_DataBase.Models.ImportReciept", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Paid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Remaining")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("SupplierID")
                        .HasColumnType("int");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("SupplierID");

                    b.HasIndex("UserName");

                    b.ToTable("ImportReciepts");
                });

            modelBuilder.Entity("API_and_DataBase.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<decimal>("BuyingPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("SellingPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("API_and_DataBase.Models.Supplier", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("API_and_DataBase.Models.Transactions", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReceiptID")
                        .HasColumnType("int");

                    b.Property<string>("ReceiptType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Receiver")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("User")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("API_and_DataBase.Models.Users", b =>
                {
                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Type")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("UserName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("API_and_DataBase.Models.CarProduct", b =>
                {
                    b.HasOne("API_and_DataBase.Models.Car", "Car")
                        .WithMany("CarProducts")
                        .HasForeignKey("CarID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API_and_DataBase.Models.Product", "Product")
                        .WithMany("CarProducts")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("API_and_DataBase.Models.ExportProduct", b =>
                {
                    b.HasOne("API_and_DataBase.Models.ExportReciept", "ExportReciept")
                        .WithMany("ExportProducts")
                        .HasForeignKey("ExportRecieptID");

                    b.HasOne("API_and_DataBase.Models.Product", "Product")
                        .WithMany("ExportProducts")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExportReciept");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("API_and_DataBase.Models.ExportReciept", b =>
                {
                    b.HasOne("API_and_DataBase.Models.Car", "CarBuy")
                        .WithMany("BuyReciepts")
                        .HasForeignKey("CarBuyID");

                    b.HasOne("API_and_DataBase.Models.Car", "CarSell")
                        .WithMany("SellReciepts")
                        .HasForeignKey("CarSellID");

                    b.HasOne("API_and_DataBase.Models.Customer", "Customer")
                        .WithMany("ExportReciepts")
                        .HasForeignKey("CustomerID");

                    b.HasOne("API_and_DataBase.Models.Users", "User")
                        .WithMany("ExportReciepts")
                        .HasForeignKey("UserName");

                    b.Navigation("CarBuy");

                    b.Navigation("CarSell");

                    b.Navigation("Customer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API_and_DataBase.Models.ImportProduct", b =>
                {
                    b.HasOne("API_and_DataBase.Models.ImportReciept", "ImportReciept")
                        .WithMany("ImportProducts")
                        .HasForeignKey("ImportRecieptID");

                    b.HasOne("API_and_DataBase.Models.Product", "Product")
                        .WithMany("ImportProducts")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ImportReciept");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("API_and_DataBase.Models.ImportReciept", b =>
                {
                    b.HasOne("API_and_DataBase.Models.Supplier", "Supplier")
                        .WithMany("ImportReciepts")
                        .HasForeignKey("SupplierID");

                    b.HasOne("API_and_DataBase.Models.Users", "User")
                        .WithMany("ImportReciepts")
                        .HasForeignKey("UserName");

                    b.Navigation("Supplier");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API_and_DataBase.Models.Car", b =>
                {
                    b.Navigation("BuyReciepts");

                    b.Navigation("CarProducts");

                    b.Navigation("SellReciepts");
                });

            modelBuilder.Entity("API_and_DataBase.Models.Customer", b =>
                {
                    b.Navigation("ExportReciepts");
                });

            modelBuilder.Entity("API_and_DataBase.Models.ExportReciept", b =>
                {
                    b.Navigation("ExportProducts");
                });

            modelBuilder.Entity("API_and_DataBase.Models.ImportReciept", b =>
                {
                    b.Navigation("ImportProducts");
                });

            modelBuilder.Entity("API_and_DataBase.Models.Product", b =>
                {
                    b.Navigation("CarProducts");

                    b.Navigation("ExportProducts");

                    b.Navigation("ImportProducts");
                });

            modelBuilder.Entity("API_and_DataBase.Models.Supplier", b =>
                {
                    b.Navigation("ImportReciepts");
                });

            modelBuilder.Entity("API_and_DataBase.Models.Users", b =>
                {
                    b.Navigation("ExportReciepts");

                    b.Navigation("ImportReciepts");
                });
#pragma warning restore 612, 618
        }
    }
}
