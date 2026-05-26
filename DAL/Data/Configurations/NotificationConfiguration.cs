using DAL.Models;
using DAL.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Message)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(n => n.Type)
                   .HasConversion<string>();

            builder.Property(n => n.IsRead)
                   .HasDefaultValue(false);

            // Notification → ApplicationUser: Cascade
            // لو الـ User اتحذف الـ notifications بتتحذف معاه
            builder.HasOne(n => n.User)
                   .WithMany()
                   .HasForeignKey(n => n.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
