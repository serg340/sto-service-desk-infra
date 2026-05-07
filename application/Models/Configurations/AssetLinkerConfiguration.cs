using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Configurations
{
    public class AssetLinkerConfiguration : IEntityTypeConfiguration<AssetLinker>
    {
        public void Configure(EntityTypeBuilder<AssetLinker> builder)
        {
            builder.HasOne(al => al.Asset)
                .WithMany()
                .HasForeignKey(al => al.AssetId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(al => al.EntityId).IsRequired();
            builder.Property(al => al.EntityType).IsRequired();
            
            // Polymorphic configuration has no explicit foreign key relationship configured for EntityId
            // as it connects to multiple tables (Tickets, RoleTickets, Stos) based on EntityType.
        }
    }
}
