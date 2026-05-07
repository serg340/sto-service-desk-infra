using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Configurations
{
    public class RoleTicketThemeConfiguration : IEntityTypeConfiguration<RoleTicketTheme>
    {
        public void Configure(EntityTypeBuilder<RoleTicketTheme> builder)
        {
            builder.Property(rtt => rtt.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(rtt => rtt.TargetRole)
                .IsRequired();

            builder.HasOne(rtt => rtt.Category)
                .WithMany(c => c.RoleTicketThemes)
                .HasForeignKey(rtt => rtt.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
