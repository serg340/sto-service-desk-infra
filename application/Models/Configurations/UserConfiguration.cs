using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(u => u.EmailConfirmed).HasDefaultValue(false);

            builder.Property(u => u.PasswordHash).IsRequired();

            builder.HasIndex(u => u.UserName).IsUnique(false);
            builder.Property(u => u.UserName)
                .IsRequired(false)
                .HasMaxLength(128);

            builder.HasIndex(u => u.PhoneNumber).IsUnique(true);
            builder.Property(u => u.PhoneNumber)
                .IsRequired(false)
                .HasMaxLength(32);

            builder.Property(u => u.PhoneNumberConfirmed).HasDefaultValue(false);


            builder.Property(u => u.CreatedAt).HasDefaultValue(DateTime.Now);

            builder.HasOne(u => u.Region)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RegionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(u => u.Sto)
                .WithMany(s => s.Mechanics)
                .HasForeignKey(u => u.StoId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
