using DAL.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations.UsersConfig
{
    public class PharmacistConfiguration : IEntityTypeConfiguration<Pharmacist>
    {
        public void Configure(EntityTypeBuilder<Pharmacist> builder)
        {
            builder.HasKey(p => p.Id);

            // Pharmacist → ApplicationUser (Composition: delete Pharmacist deletes User)
            builder.HasOne(p => p.User)
                   .WithOne()
                   .HasForeignKey<Pharmacist>(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.PharmacyName)
                   .HasMaxLength(200);

            // Pharmacist → Medications: Restrict (لو فيه medications مش هينفع تحذف pharmacist)
            builder.HasMany(p => p.Medications)
                   .WithOne(m => m.Pharmacist)
                   .HasForeignKey(m => m.PharmacistId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
