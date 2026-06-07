using DAL.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations.UsersConfig
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {

            // Doctor → ApplicationUser (Composition: delete Doctor deletes User)
            builder.HasOne(d => d.User)
                   .WithOne()
                   .HasForeignKey<Doctor>(d => d.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(d => d.Specialty)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.Location)
                   .HasMaxLength(250);

            // Doctor → DoctorSchedules (One-to-Many)
            // Configured in DoctorScheduleConfiguration
        }
    }
}
