﻿// <auto-generated />
using System;
using MVC_EF_Start.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MVC_EF_Start.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MVC_EF_Start.Models.DCounty", b =>
                {
                    b.Property<int>("CountyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CountyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RegionID")
                        .HasColumnType("int");

                    b.HasKey("CountyID");

                    b.HasIndex("RegionID");

                    b.ToTable("DCounties");
                });

            modelBuilder.Entity("MVC_EF_Start.Models.DMake", b =>
                {
                    b.Property<int>("MakeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CountyID")
                        .HasColumnType("int");

                    b.Property<string>("MakeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MakeID");

                    b.HasIndex("CountyID");

                    b.ToTable("DMakes");
                });

            modelBuilder.Entity("MVC_EF_Start.Models.DModel", b =>
                {
                    b.Property<int>("ModelID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CountyID")
                        .HasColumnType("int");

                    b.Property<int?>("DMakeMakeID")
                        .HasColumnType("int");

                    b.Property<string>("ModelName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ModelID");

                    b.HasIndex("CountyID");

                    b.HasIndex("DMakeMakeID");

                    b.ToTable("DModels");
                });

            modelBuilder.Entity("MVC_EF_Start.Models.DRegion", b =>
                {
                    b.Property<int>("RegionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RegionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RegionID");

                    b.ToTable("DRegions");
                });

            modelBuilder.Entity("MVC_EF_Start.Models.DVehicle", b =>
                {
                    b.Property<string>("VIN")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CountyID")
                        .HasColumnType("int");

                    b.Property<int?>("DModelModelID")
                        .HasColumnType("int");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MakeID")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ModelID")
                        .HasColumnType("int");

                    b.Property<int>("Range")
                        .HasColumnType("int");

                    b.HasKey("VIN");

                    b.HasIndex("CountyID");

                    b.HasIndex("DModelModelID");

                    b.ToTable("DVehicles");
                });

            modelBuilder.Entity("MVC_EF_Start.Models.DCounty", b =>
                {
                    b.HasOne("MVC_EF_Start.Models.DRegion", "Region")
                        .WithMany("Counties")
                        .HasForeignKey("RegionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("MVC_EF_Start.Models.DMake", b =>
                {
                    b.HasOne("MVC_EF_Start.Models.DCounty", "County")
                        .WithMany("Makes")
                        .HasForeignKey("CountyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("County");
                });

            modelBuilder.Entity("MVC_EF_Start.Models.DModel", b =>
                {
                    b.HasOne("MVC_EF_Start.Models.DCounty", "County")
                        .WithMany("Models")
                        .HasForeignKey("CountyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVC_EF_Start.Models.DMake", null)
                        .WithMany("Models")
                        .HasForeignKey("DMakeMakeID");

                    b.Navigation("County");
                });

            modelBuilder.Entity("MVC_EF_Start.Models.DVehicle", b =>
                {
                    b.HasOne("MVC_EF_Start.Models.DCounty", "County")
                        .WithMany("Vehicles")
                        .HasForeignKey("CountyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVC_EF_Start.Models.DModel", null)
                        .WithMany("Vehicles")
                        .HasForeignKey("DModelModelID");

                    b.Navigation("County");
                });

            modelBuilder.Entity("MVC_EF_Start.Models.DCounty", b =>
                {
                    b.Navigation("Makes");

                    b.Navigation("Models");

                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("MVC_EF_Start.Models.DMake", b =>
                {
                    b.Navigation("Models");
                });

            modelBuilder.Entity("MVC_EF_Start.Models.DModel", b =>
                {
                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("MVC_EF_Start.Models.DRegion", b =>
                {
                    b.Navigation("Counties");
                });
#pragma warning restore 612, 618
        }
    }
}
