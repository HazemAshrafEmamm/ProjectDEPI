using DAL.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations.UsersConfig
{
    public class NurseConfiguration : IEntityTypeConfiguration<Nurse>
    {
        public void Configure(EntityTypeBuilder<Nurse> builder)
        {

            // Nurse → ApplicationUser (Composition: delete Nurse deletes User)
            builder.HasOne(n => n.User)
                   .WithOne()
                   .HasForeignKey<Nurse>(n => n.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(n => n.Specialization)
                   .HasMaxLength(100);

            // Nurse → NursingRequests: Restrict (لو فيه requests مش هينفع تحذف nurse)
            builder.HasMany(n => n.NursingRequests)
                   .WithOne(nr => nr.Nurse)
                   .HasForeignKey(nr => nr.NurseId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
