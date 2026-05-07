using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Configurations
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.Property(mf => mf.BucketName).IsRequired();
            builder.Property(mf => mf.FileName).IsRequired();
            builder.Property(mf => mf.ObjectName).IsRequired();
            builder.Property(mf => mf.Size).IsRequired();

            builder.Property(mf => mf.CreatedAt).HasDefaultValue(DateTime.Now);
        }
    }
}
