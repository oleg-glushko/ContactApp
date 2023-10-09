﻿// <auto-generated />
using ContactApp.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ContactApp.Migrations
{
    [DbContext(typeof(ContactDbContext))]
    [Migration("20231009002842_SeedContactsData")]
    partial class SeedContactsData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0-rc.1.23419.6");

            modelBuilder.Entity("ContactApp.Contacts.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("First")
                        .HasColumnType("TEXT");

                    b.Property<string>("Last")
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Contacts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "john@example.com",
                            First = "John",
                            Last = "Smith",
                            Phone = "123-456-7890"
                        },
                        new
                        {
                            Id = 2,
                            Email = "dcran@example.com",
                            First = "Dana",
                            Last = "Crandith",
                            Phone = "123-456-7890"
                        },
                        new
                        {
                            Id = 3,
                            Email = "en@example.com",
                            First = "Edith",
                            Last = "Neutvaar",
                            Phone = "123-456-7890"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
