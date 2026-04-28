using DAL.Models;

namespace DAL.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(u => u.Id);

        entity.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        entity.HasIndex(u => u.Email)
            .IsUnique();

        entity.Property(u => u.PasswordHash)
            .IsRequired();

        entity.Property(u => u.Role)
            .IsRequired()
            .HasConversion<string>(); // store enum as string

        entity.Property(u => u.IsActive)
            .HasDefaultValue(true);

        // Global query filter for soft delete
        entity.HasQueryFilter(u => u.IsActive);
    }
}