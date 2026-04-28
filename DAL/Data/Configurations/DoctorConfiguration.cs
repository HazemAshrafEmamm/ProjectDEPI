using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Models;
using DAL.Models.Users;


public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
         
        builder.HasOne(d => d.User)  
               .WithOne()
               .HasForeignKey<Doctor>(d => d.UserId)
               .OnDelete(DeleteBehavior.Cascade);  // Composition

        builder.Property(d => d.Specialty)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(d => d.Location)
               .HasMaxLength(250);

    }
}