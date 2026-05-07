using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Configurations
{
    public class StoConfiguration : IEntityTypeConfiguration<Sto>
    {
        public void Configure(EntityTypeBuilder<Sto> builder)
        {
            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(s => s.Body)
                .IsRequired()
                .HasMaxLength(512);

            builder.Property(s => s.CreatedAt).HasDefaultValue(DateTime.Now);

            builder.HasOne(s => s.Owner)
                .WithMany(u => u.OwnedStos)
                .HasForeignKey(s => s.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(s => s.Region)
                .WithMany(r => r.Stos)
                .HasForeignKey(s => s.RegionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
