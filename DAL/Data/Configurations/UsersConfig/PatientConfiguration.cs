using DAL.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations.UsersConfig
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {

            // Patient → ApplicationUser (Composition: delete Patient deletes User)
            builder.HasOne(p => p.User)
                   .WithOne()
                   .HasForeignKey<Patient>(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Address)
                   .HasMaxLength(250);

            builder.Property(p => p.DateOfBirth)
                   .IsRequired();

            // Patient → Orders: Restrict (لو فيه أوردر مش هينفع تحذف patient)
            builder.HasMany<Models.OrderModule.Order>()
                   .WithOne(o => o.Patient)
                   .HasForeignKey(o => o.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
