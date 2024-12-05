﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartWeather.Repositories.Context;

#nullable disable

namespace SmartWeather.Repositories.Migrations
{
    [DbContext(typeof(SmartWeatherContext))]
    [Migration("20241205093752_AddAdminSeed")]
    partial class AddAdminSeed
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SmartWeather.Entities.Component.Component", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("GpioPin")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("StationId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.ToTable("Component", (string)null);
                });

            modelBuilder.Entity("SmartWeather.Entities.MeasurePoint.MeasurePoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ComponentId")
                        .HasColumnType("int");

                    b.Property<int>("LocalId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Unit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ComponentId");

                    b.ToTable("MeasurePoint", (string)null);
                });

            modelBuilder.Entity("SmartWeather.Entities.Station.Station", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<float>("Latitude")
                        .HasColumnType("float");

                    b.Property<float>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("MacAddress")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MacAddress")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Station", (string)null);
                });

            modelBuilder.Entity("SmartWeather.Entities.User.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("User", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@smartweather.net",
                            PasswordHash = "749F09BADE8ACA755660EEB17792DA880218D4FBDC4E25FBEC279D7FE9F65D70",
                            Role = 0,
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("SmartWeather.Entities.Component.Component", b =>
                {
                    b.HasOne("SmartWeather.Entities.Station.Station", "Station")
                        .WithMany("Components")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Station");
                });

            modelBuilder.Entity("SmartWeather.Entities.MeasurePoint.MeasurePoint", b =>
                {
                    b.HasOne("SmartWeather.Entities.Component.Component", "Component")
                        .WithMany("MeasurePoints")
                        .HasForeignKey("ComponentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Component");
                });

            modelBuilder.Entity("SmartWeather.Entities.Station.Station", b =>
                {
                    b.HasOne("SmartWeather.Entities.User.User", "User")
                        .WithMany("Stations")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SmartWeather.Entities.Component.Component", b =>
                {
                    b.Navigation("MeasurePoints");
                });

            modelBuilder.Entity("SmartWeather.Entities.Station.Station", b =>
                {
                    b.Navigation("Components");
                });

            modelBuilder.Entity("SmartWeather.Entities.User.User", b =>
                {
                    b.Navigation("Stations");
                });
#pragma warning restore 612, 618
        }
    }
}
