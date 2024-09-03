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
        builder.HasKey(component => component.Id);
        builder.Property(component => component.Name);
        builder.Property(component => component.Color);
        builder.Property(component => component.Unit);
        builder.Property(component => component.Type);
        builder.Property(component => component.GpioPin);
        builder.Property(component => component.StationId);
        builder.HasOne(e => e.Station)
                            .WithMany(e => e.Components)
                            .HasForeignKey(e => e.StationId)
                            .IsRequired();

        builder.HasIndex(e => e.StationId);
    }
}