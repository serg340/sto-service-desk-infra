using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STO_Desk_backend.Models.Entities;
using STO_Desk_backend.Models.Enums;

namespace STO_Desk_backend.Models.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(t => t.Body)
                .IsRequired()
                .HasMaxLength(512);

            builder.Property(t => t.Theme).IsRequired();

            builder.Property(t => t.Status).HasDefaultValue(TicketStatus.Pending);

            builder.Property(t => t.CreatedAt).HasDefaultValue(DateTime.Now);

            builder.HasOne(t => t.Client)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Mechanic)
                .WithMany(u => u.MechanicTickets)
                .HasForeignKey(t => t.MechanicId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(t => t.Sto)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.StoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Theme)
                .WithMany(tt => tt.Tickets)
                .HasForeignKey(t => t.ThemeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
