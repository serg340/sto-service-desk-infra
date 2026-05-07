using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Configurations
{
    public class ImageAssetVariantConfiguration : IEntityTypeConfiguration<ImageAssetVariant>
    {
        public void Configure(EntityTypeBuilder<ImageAssetVariant> builder)
        {
            builder.HasOne(iav => iav.Image)
                .WithMany()
                .HasForeignKey(iav => iav.ImageAssetId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(iav => iav.ObjectName).IsRequired();
            builder.Property(iav => iav.SizeType).IsRequired();
            builder.Property(iav => iav.Width).IsRequired();
            builder.Property(iav => iav.Height).IsRequired();
        }
    }
}
