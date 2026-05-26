using DAL.Models.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations.OrderConfig
{
    public class MedicationConfiguration : IEntityTypeConfiguration<Medication>
    {
        public void Configure(EntityTypeBuilder<Medication> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(m => m.Price)
                   .HasColumnType("decimal(10,2)");

            builder.Property(m => m.Stock)
                   .IsRequired();

            builder.Property(m => m.Is_available)
                   .HasDefaultValue(true);

            // Medication → Order_Items: Restrict
            // لو الدواء اتباع في أوردر مش هينفع تحذفه
            builder.HasMany(m => m.Order_Item)
                   .WithOne(oi => oi.Medication)
                   .HasForeignKey(oi => oi.MedicationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
