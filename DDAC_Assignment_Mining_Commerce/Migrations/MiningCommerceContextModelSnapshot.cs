﻿// <auto-generated />
using System;
using DDAC_Assignment_Mining_Commerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DDAC_Assignment_Mining_Commerce.Migrations
{
    [DbContext(typeof(MiningCommerceContext))]
    partial class MiningCommerceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DDAC_Assignment_Mining_Commerce.Models.AdminModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("userID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("userID");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("DDAC_Assignment_Mining_Commerce.Models.BuyerModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("shipping_address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("userID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("userID");

                    b.ToTable("Buyer");
                });

            modelBuilder.Entity("DDAC_Assignment_Mining_Commerce.Models.ProductModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("productName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("productPrice")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("productQuantity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("sellerID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("sellerID");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("DDAC_Assignment_Mining_Commerce.Models.SellerModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("storeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("store_address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("store_contact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("userID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("userID");

                    b.ToTable("Seller");
                });

            modelBuilder.Entity("DDAC_Assignment_Mining_Commerce.Models.UserModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fullname")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("gender")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("DDAC_Assignment_Mining_Commerce.Models.AdminModel", b =>
                {
                    b.HasOne("DDAC_Assignment_Mining_Commerce.Models.UserModel", "user")
                        .WithMany()
                        .HasForeignKey("userID");

                    b.Navigation("user");
                });

            modelBuilder.Entity("DDAC_Assignment_Mining_Commerce.Models.BuyerModel", b =>
                {
                    b.HasOne("DDAC_Assignment_Mining_Commerce.Models.UserModel", "user")
                        .WithMany()
                        .HasForeignKey("userID");

                    b.Navigation("user");
                });

            modelBuilder.Entity("DDAC_Assignment_Mining_Commerce.Models.ProductModel", b =>
                {
                    b.HasOne("DDAC_Assignment_Mining_Commerce.Models.SellerModel", "seller")
                        .WithMany()
                        .HasForeignKey("sellerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("seller");
                });

            modelBuilder.Entity("DDAC_Assignment_Mining_Commerce.Models.SellerModel", b =>
                {
                    b.HasOne("DDAC_Assignment_Mining_Commerce.Models.UserModel", "user")
                        .WithMany()
                        .HasForeignKey("userID");

                    b.Navigation("user");
                });
#pragma warning restore 612, 618
        }
    }
}
