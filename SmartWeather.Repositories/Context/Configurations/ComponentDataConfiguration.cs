namespace SmartWeather.Repositories.Context.Configurations;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SmartWeather.Entities.ComponentData;

public class ComponentDataConfiguration : IEntityTypeConfiguration<ComponentData>
{
    public void Configure(EntityTypeBuilder<ComponentData> builder)
    {
        builder.ToTable(nameof(ComponentData));
        builder.HasKey(component => component.Id);
        builder.Property(component => component.Value);
        builder.Property(component => component.DateTime);
        builder.Property(component => component.ComponentId);

        builder.HasOne(e => e.Component)
                            .WithMany(e => e.ComponentDatas)
                            .HasForeignKey(e => e.ComponentId)
                            .IsRequired();

        builder.HasIndex(e => new { e.ComponentId, e.DateTime });
    }
}