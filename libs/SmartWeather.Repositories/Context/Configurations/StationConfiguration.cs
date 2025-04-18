﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartWeather.Entities.Station;

namespace SmartWeather.Repositories.Context.Configurations;

internal class StationConfiguration : IEntityTypeConfiguration<Station>
{
    public void Configure(EntityTypeBuilder<Station> builder)
    {
        builder.ToTable(nameof(Station));
        builder.HasKey(station => station.Id);
        builder.Property(station => station.Name);
        builder.Property(station => station.MacAddress);
        builder.Property(station => station.Latitude);
        builder.Property(station => station.Longitude);
        builder.Property(station => station.Type);
        builder.Property(station => station.UserId).IsRequired(false);
        builder.HasOne(e => e.User)
                            .WithMany(e => e.Stations)
                            .HasForeignKey(e => e.UserId)
                            .IsRequired(false);

        builder.HasMany(e => e.Components)
                            .WithOne(e => e.Station)
                            .HasForeignKey(e => e.StationId)
                            .IsRequired();

        builder.HasIndex(e => e.UserId).IsUnique(false);
        
        builder.HasIndex(e => e.MacAddress).IsUnique();
    }
}
