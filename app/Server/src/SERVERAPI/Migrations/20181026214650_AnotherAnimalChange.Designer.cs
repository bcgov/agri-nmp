﻿// <auto-generated />
using Agri.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SERVERAPI.Migrations
{
    [DbContext(typeof(AgriConfigurationContext))]
    [Migration("20181026214650_AnotherAnimalChange")]
    partial class AnotherAnimalChange
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Agri.Models.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Animals");
                });

            modelBuilder.Entity("Agri.Models.AnimalSubType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AnimalId");

                    b.Property<decimal>("LiquidPerGalPerAnimalPerDay");

                    b.Property<string>("Name");

                    b.Property<decimal>("SolidLiquidSeparationPercentage");

                    b.Property<decimal>("SolidPerGalPerAnimalPerDay");

                    b.Property<decimal>("SolidPerPoundPerAnimalPerDay");

                    b.HasKey("Id");

                    b.HasIndex("AnimalId");

                    b.ToTable("AnimalSubType");
                });

            modelBuilder.Entity("Agri.Models.Browser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MinVersion");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Browsers");
                });

            modelBuilder.Entity("Agri.Models.AnimalSubType", b =>
                {
                    b.HasOne("Agri.Models.Animal", "Animal")
                        .WithMany("AnimalSubTypes")
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
