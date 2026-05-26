using DAL.Models.Consultation;
using DAL.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations
{
    public class ConsultationConfiguration : IEntityTypeConfiguration<Consultation>
    {
        public void Configure(EntityTypeBuilder<Consultation> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Status)
                   .HasConversion<string>()
                   .HasDefaultValue(ConsultationStatus.Requested);

            builder.Property(c => c.RequestedAt)
                   .IsRequired();

            builder.Property(c => c.CreatedAt)
                   .ValueGeneratedOnAdd();

            // ⚠️ Multiple Cascade Paths:
            // Consultation له Patient و Doctor
            // نفس المشكلة → واحد Cascade والتاني NoAction

            builder.HasOne(c => c.Patient)
                   .WithMany()
                   .HasForeignKey(c => c.PatientId)
                   .OnDelete(DeleteBehavior.Cascade);  // Patient هو الـ owner

            builder.HasOne(c => c.Doctor)
                   .WithMany()
                   .HasForeignKey(c => c.DoctorId)
                   .OnDelete(DeleteBehavior.NoAction);  // ⛔ مش Cascade

            // Consultation → ConsultationReview: Cascade
            // لو الـ consultation اتحذفت الـ review بتتحذف معاها
            builder.HasOne(c => c.Review)
                   .WithOne(r => r.Consultation)
                   .HasForeignKey<ConsultationReview>(r => r.ConsultationId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
