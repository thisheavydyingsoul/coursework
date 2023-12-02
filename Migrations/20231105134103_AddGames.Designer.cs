﻿// <auto-generated />
using System;
using CourseWorkAdmin.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CourseWorkAdmin.Migrations
{
    [DbContext(typeof(ComputerClubDBContext))]
    [Migration("20231105134103_AddGames")]
    partial class AddGames
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CourseWorkAdmin.Models.DeviceGame", b =>
                {
                    b.Property<int>("DeviceId")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<string>("GameName")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(1);

                    b.HasKey("DeviceId", "GameName");

                    b.HasIndex("GameName");

                    b.ToTable("DevicesGames");
                });

            modelBuilder.Entity("CourseWorkAdmin.Models.Game", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Picture")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Name");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("CourseWorkAdmin.Models.Administrator", b =>
                {
                    b.Property<string>("Username")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHr")
                        .HasColumnType("bit");

                    b.Property<int?>("OfficeId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Username");

                    b.HasIndex("OfficeId");

                    b.ToTable("Administrators");
                });

            modelBuilder.Entity("CourseWorkAdmin.Models.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Condition")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("DayRate")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("NightRate")
                        .HasColumnType("int");

                    b.Property<int>("OfficeId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Picture")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("OfficeId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("CourseWorkAdmin.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdministratorUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Contents")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdministratorUsername");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("CourseWorkAdmin.Models.Office", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Offices");
                });

            modelBuilder.Entity("CourseWorkAdmin.Models.DeviceGame", b =>
                {
                    b.HasOne("CourseWorkAdmin.Models.Device", "Device")
                        .WithMany("DevicesGames")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CourseWorkAdmin.Models.Game", "Game")
                        .WithMany("DevicesGames")
                        .HasForeignKey("GameName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("CourseWorkAdmin.Models.Administrator", b =>
                {
                    b.HasOne("CourseWorkAdmin.Models.Office", "Office")
                        .WithMany("Administrators")
                        .HasForeignKey("OfficeId");

                    b.Navigation("Office");
                });

            modelBuilder.Entity("CourseWorkAdmin.Models.Device", b =>
                {
                    b.HasOne("CourseWorkAdmin.Models.Office", "Office")
                        .WithMany("Devices")
                        .HasForeignKey("OfficeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Office");
                });

            modelBuilder.Entity("CourseWorkAdmin.Models.Log", b =>
                {
                    b.HasOne("CourseWorkAdmin.Models.Administrator", "Administrator")
                        .WithMany("Logs")
                        .HasForeignKey("AdministratorUsername")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Administrator");
                });

            modelBuilder.Entity("CourseWorkAdmin.Models.Game", b =>
                {
                    b.Navigation("DevicesGames");
                });

            modelBuilder.Entity("CourseWorkAdmin.Models.Administrator", b =>
                {
                    b.Navigation("Logs");
                });

            modelBuilder.Entity("CourseWorkAdmin.Models.Device", b =>
                {
                    b.Navigation("DevicesGames");
                });

            modelBuilder.Entity("CourseWorkAdmin.Models.Office", b =>
                {
                    b.Navigation("Administrators");

                    b.Navigation("Devices");
                });
#pragma warning restore 612, 618
        }
    }
}
