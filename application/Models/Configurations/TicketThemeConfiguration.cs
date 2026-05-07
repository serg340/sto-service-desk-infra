using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STO_Desk_backend.Models.Entities;

namespace STO_Desk_backend.Models.Configurations
{
    public class TicketThemeConfiguration : IEntityTypeConfiguration<TicketTheme>
    {
        public void Configure(EntityTypeBuilder<TicketTheme> builder)
        {
            builder.Property(tt => tt.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.HasOne(tt => tt.Category)
                .WithMany(c => c.TicketThemes)
                .HasForeignKey(tt => tt.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
