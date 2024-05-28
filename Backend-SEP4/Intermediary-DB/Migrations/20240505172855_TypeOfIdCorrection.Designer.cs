﻿// <auto-generated />
using System;
using DBComm.Logic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DBComm.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20240505172855_TypeOfIdCorrection")]
    partial class TypeOfIdCorrection
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DBComm.Shared.Admin", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("HomeId")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("HomeId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("DBComm.Shared.Door", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("HomeId")
                        .HasColumnType("text");

                    b.Property<int>("LockPassword")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("HomeId");

                    b.ToTable("Doors");
                });

            modelBuilder.Entity("DBComm.Shared.Home", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Homes");
                });

            modelBuilder.Entity("DBComm.Shared.HumidityReading", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("ReadAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RoomId")
                        .HasColumnType("text");

                    b.Property<double>("Value")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("HumidityReadings");
                });

            modelBuilder.Entity("DBComm.Shared.LightReading", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("ReadAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RoomId")
                        .HasColumnType("text");

                    b.Property<double>("Value")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("LightReadings");
                });

            modelBuilder.Entity("DBComm.Shared.Member", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AdminId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HomeId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("HomeId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("DBComm.Shared.Notification", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("HomeId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("SendAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("HomeId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("DBComm.Shared.Room", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HomeId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("HomeId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("DBComm.Shared.TemperatureReading", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("ReadAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RoomId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Value")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("TemperatureReadings");
                });

            modelBuilder.Entity("DBComm.Shared.Admin", b =>
                {
                    b.HasOne("DBComm.Shared.Home", "Home")
                        .WithMany()
                        .HasForeignKey("HomeId");

                    b.Navigation("Home");
                });

            modelBuilder.Entity("DBComm.Shared.Door", b =>
                {
                    b.HasOne("DBComm.Shared.Home", "Home")
                        .WithMany("Doors")
                        .HasForeignKey("HomeId");

                    b.Navigation("Home");
                });

            modelBuilder.Entity("DBComm.Shared.HumidityReading", b =>
                {
                    b.HasOne("DBComm.Shared.Room", "Room")
                        .WithMany("HumidityReadings")
                        .HasForeignKey("RoomId");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("DBComm.Shared.LightReading", b =>
                {
                    b.HasOne("DBComm.Shared.Room", "Room")
                        .WithMany("LightReadings")
                        .HasForeignKey("RoomId");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("DBComm.Shared.Member", b =>
                {
                    b.HasOne("DBComm.Shared.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DBComm.Shared.Home", "Home")
                        .WithMany("Members")
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("Home");
                });

            modelBuilder.Entity("DBComm.Shared.Notification", b =>
                {
                    b.HasOne("DBComm.Shared.Home", "Home")
                        .WithMany("Notifications")
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Home");
                });

            modelBuilder.Entity("DBComm.Shared.Room", b =>
                {
                    b.HasOne("DBComm.Shared.Home", "Home")
                        .WithMany("Rooms")
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Home");
                });

            modelBuilder.Entity("DBComm.Shared.TemperatureReading", b =>
                {
                    b.HasOne("DBComm.Shared.Room", "Room")
                        .WithMany("TemperatureReadings")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("DBComm.Shared.Home", b =>
                {
                    b.Navigation("Doors");

                    b.Navigation("Members");

                    b.Navigation("Notifications");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("DBComm.Shared.Room", b =>
                {
                    b.Navigation("HumidityReadings");

                    b.Navigation("LightReadings");

                    b.Navigation("TemperatureReadings");
                });
#pragma warning restore 612, 618
        }
    }
}
