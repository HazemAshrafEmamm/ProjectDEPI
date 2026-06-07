using DAL.Models.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations.OrderConfig
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<Order_Item>
    {
        public void Configure(EntityTypeBuilder<Order_Item> builder)
        {

            builder.Property(oi => oi.quantity)
                   .IsRequired();

            builder.Property(oi => oi.unit_price)
                   .HasColumnType("decimal(10,2)");

            // العلاقات اتعرفت في OrderConfiguration و MedicationConfiguration
        }
    }
}
