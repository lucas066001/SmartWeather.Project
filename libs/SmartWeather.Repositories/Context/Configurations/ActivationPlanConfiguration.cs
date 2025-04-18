﻿namespace SmartWeather.Repositories.Context.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.ActivationPlan;

internal class ActivationPlanConfiguration : IEntityTypeConfiguration<ActivationPlan>
{
    public void Configure(EntityTypeBuilder<ActivationPlan> builder)
    {
        builder.ToTable(nameof(ActivationPlan));
        builder.HasKey(component => component.Id);
        builder.Property(component => component.Name);
        builder.Property(component => component.StartingDate);
        builder.Property(component => component.ActivationTime);
        builder.Property(component => component.EndingDate);
        builder.Property(component => component.PeriodInDay);
        builder.Property(component => component.Duration);
        builder.Property(component => component.ComponentId);
        builder.HasOne(e => e.Component)
                            .WithMany(e => e.ActivationPlans)
                            .HasForeignKey(e => e.ComponentId)
                            .IsRequired();

        builder.HasIndex(e => e.ComponentId);
    }
}