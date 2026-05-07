using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STO_Desk_backend.Models.Entities;
using STO_Desk_backend.Models.Enums;

namespace STO_Desk_backend.Models.Configurations
{
    public class RoleTicketConfiguration : IEntityTypeConfiguration<RoleTicket>
    {
        public void Configure(EntityTypeBuilder<RoleTicket> builder)
        {
            builder.Property(rt => rt.Title)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(rt => rt.Body)
                .IsRequired()
                .HasMaxLength(512);

            builder.Property(rt => rt.Theme).IsRequired();

            builder.Property(rt => rt.Status).HasDefaultValue(TicketStatus.Pending);

            builder.Property(rt => rt.CreatedAt).HasDefaultValue(DateTime.Now);

            builder.HasOne(rt => rt.User)
                .WithMany(u => u.RoleTickets)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rt => rt.Sto)
                .WithMany(s => s.RoleTickets)
                .HasForeignKey(rt => rt.StoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rt => rt.Reviewer)
                .WithMany(u => u.ReviewedRoleTickets)
                .HasForeignKey(rt => rt.ReviewerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(rt => rt.Theme)
                .WithMany(rtt => rtt.RoleTickets)
                .HasForeignKey(rt => rt.ThemeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
