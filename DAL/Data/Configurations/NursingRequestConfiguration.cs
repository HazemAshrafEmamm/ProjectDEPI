using DAL.Models.NursingModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations
{
    public class NursingRequestConfiguration : IEntityTypeConfiguration<NursingRequest>
    {
        public void Configure(EntityTypeBuilder<NursingRequest> builder)
        {

            builder.Property(nr => nr.CareType)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(nr => nr.Status)
                   .HasMaxLength(50)
                   .HasDefaultValue("Pending");

            builder.Property(nr => nr.RequestedDate)
                   .ValueGeneratedOnAdd();

            builder.Property(nr => nr.CreatedAt)
                   .ValueGeneratedOnAdd();


            builder.HasOne(nr => nr.Patient)
                   .WithMany()
                   .HasForeignKey(nr => nr.PatientId)
                   .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(nr => nr.Nurse)
                   .WithMany(n => n.NursingRequests)
                   .HasForeignKey(nr => nr.NurseId)
                   .OnDelete(DeleteBehavior.NoAction);  

            builder.HasOne(nr => nr.Review)
                   .WithOne(r => r.NursingRequest)
                   .HasForeignKey<NursingReview>(r => r.NursingRequestId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
