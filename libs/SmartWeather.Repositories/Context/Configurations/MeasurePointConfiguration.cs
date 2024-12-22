using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWeather.Entities.ComponentData;
using SmartWeather.Entities.MeasurePoint;

namespace SmartWeather.Repositories.Context.Configurations;

public class MeasurePointConfiguration : IEntityTypeConfiguration<MeasurePoint>
{
    public void Configure(EntityTypeBuilder<MeasurePoint> builder)
    {
        builder.ToTable(nameof(MeasurePoint));
        builder.HasKey(mp => mp.Id);
        builder.Property(mp => mp.LocalId);
        builder.Property(mp => mp.Name);
        builder.Property(mp => mp.Color);
        builder.Property(mp => mp.Unit);
        builder.Property(mp => mp.ComponentId);

        builder.HasOne(e => e.Component)
                            .WithMany(e => e.MeasurePoints)
                            .HasForeignKey(e => e.ComponentId)
                            .IsRequired();
    }
}
