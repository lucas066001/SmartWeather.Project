namespace SmartWeather.Repositories.Context.Configurations;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.ComponentData;

public class MeasureDataConfiguration : IEntityTypeConfiguration<MeasureData>
{
    public void Configure(EntityTypeBuilder<MeasureData> builder)
    {
        builder.ToTable(nameof(MeasureData));
        builder.HasKey(md => md.Id);
        builder.Property(md => md.Value);
        builder.Property(md => md.DateTime);
        builder.Property(md => md.MeasurePointId);

        builder.HasOne(e => e.MeasurePoint)
                            .WithMany(e => e.MeasureDatas)
                            .HasForeignKey(e => e.MeasurePointId)
                            .IsRequired();

        builder.HasIndex(e => new { e.MeasurePointId, e.DateTime }).IsUnique(false);
    }
}