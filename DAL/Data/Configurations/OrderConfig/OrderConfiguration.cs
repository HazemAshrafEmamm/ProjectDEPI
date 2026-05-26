using DAL.Models.OrderModule;
using DAL.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations.OrderConfig
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Order_date)
                   .IsRequired();

            builder.Property(o => o.Status)
                   .HasConversion<string>()
                   .HasDefaultValue(OrderStatus.Pending);

            builder.Property(o => o.Total)
                   .HasColumnType("decimal(10,2)");

            // Order → Order_Items: Cascade
            // لو الأوردر اتحذف Order_Items بتتحذف معاه
            builder.HasMany(o => o.Order_Item)
                   .WithOne(oi => oi.Order)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
