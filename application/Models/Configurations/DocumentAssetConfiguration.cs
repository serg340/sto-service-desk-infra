using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Configurations
{
    public class DocumentAssetConfiguration : IEntityTypeConfiguration<DocumentAsset>
    {
        public void Configure(EntityTypeBuilder<DocumentAsset> builder)
        {
            builder.HasOne(da => da.Asset)
                .WithMany()
                .HasForeignKey(da => da.AssetId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(da => da.Title)
                .IsRequired()
                .HasMaxLength(256);
            builder.Property(da => da.Description)
                .IsRequired()
                .HasMaxLength(512);
        }
    }
}
