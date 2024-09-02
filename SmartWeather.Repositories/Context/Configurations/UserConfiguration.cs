using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWeather.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWeather.Repositories.Context.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));
            builder.HasKey(user => user.Id);
            builder.Property(user => user.Username);
            builder.Property(user => user.Email);
            builder.Property(user => user.PasswordHash);
            builder.Property(user => user.Role);
            builder.HasMany(e => e.Stations)
                    .WithOne(e => e.User)
                    .HasForeignKey(e => e.UserId)
                    .IsRequired();
            builder.HasIndex(e => e.Email).IsUnique();
        }
    }
}
