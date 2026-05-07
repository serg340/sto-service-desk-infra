using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Configurations
{
    public class ImageAssetConfiguration : IEntityTypeConfiguration<ImageAsset>
    {
        public void Configure(EntityTypeBuilder<ImageAsset> builder)
        {
            builder.HasOne(ia => ia.Asset)
                .WithMany()
                .HasForeignKey(ia => ia.AssetId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(ia => ia.Title).IsRequired().HasMaxLength(256);
            builder.Property(ia => ia.Author).IsRequired().HasMaxLength(256);
            builder.Property(ia => ia.Alt).HasMaxLength(512);

            builder.Property(mf => mf.CreatedAt).HasDefaultValue(DateTime.Now);
        }
    }
}
