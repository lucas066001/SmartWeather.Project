namespace SmartWeather.Repositories.Context.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.Station;
using SmartWeather.Entities.Component;

internal class ComponentConfiguration : IEntityTypeConfiguration<Component>
{
    public void Configure(EntityTypeBuilder<Component> builder)
    {
        builder.ToTable(nameof(Component));
        builder.HasKey(station => station.Id);
        builder.Property(station => station.Name);
        builder.Property(station => station.Color);
        builder.Property(station => station.Unit);
        builder.Property(station => station.Type);
        builder.Property(station => station.StationId);
        builder.HasOne(e => e.Station)
                            .WithMany(e => e.Components)
                            .HasForeignKey(e => e.StationId)
                            .IsRequired();

        builder.HasIndex(e => e.StationId);
    }
}