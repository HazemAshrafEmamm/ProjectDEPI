using DAL.Models.AppointmentModule;
using DAL.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.AppointmentDate)
                   .IsRequired();

            builder.Property(a => a.AppointmentTime)
                   .IsRequired();

            builder.Property(a => a.Status)
                   .HasConversion<string>()
                   .HasDefaultValue(AppointmentStatus.Pending);

            builder.Property(a => a.Notes)
                   .HasMaxLength(500);

            // ⚠️ Multiple Cascade Paths Problem:
            // Appointment له علاقة مع Patient و Doctor و DoctorSchedule
            // لو الثلاثة Cascade → SQL Server هيرفض (multiple cascade paths)
            // الحل:
            //   Schedule → Cascade (الـ owner الأساسي)
            //   Patient  → NoAction (Restrict في EF)
            //   Doctor   → NoAction (Restrict في EF)

            builder.HasOne(a => a.Patient)
                   .WithMany()
                   .HasForeignKey(a => a.PatientId)
                   .OnDelete(DeleteBehavior.NoAction);  // ⛔ مش Cascade

            builder.HasOne(a => a.Doctor)
                   .WithMany()
                   .HasForeignKey(a => a.DoctorId)
                   .OnDelete(DeleteBehavior.NoAction);  // ⛔ مش Cascade

            // Schedule → Appointments: Cascade (متعرفة في DoctorScheduleConfiguration)
        }
    }
}
